using AutoMapper;
using Entities;
using Entities.Configuration;
using Entities.DTO;
using Entities.Enums;
using Entities.Models;
using Entities.RequestFeatures;
using Entities.WebSocket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Repository.Contracts;
using SlimBus.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebHooks.Contracts;

namespace ParkingApp2Server.Controllers;

[Route("api/parking")]
[ApiController]
[Authorize]
public class ParkingController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly RepositoryContext _context;
    private readonly PriviledgedUsersConfiguration _priviledgedUsersSettings;
    private readonly UserManager<User> _userManager;
    private readonly IWebHookPayloadProcessor _webHookPayloadProcessor;
    private readonly IWebSocketSender _webSocketSender;
    private readonly IWebHookSubscriptionsProvider _webHookSubscriptionsProvider;
    private readonly IMessagePublisher _messagePublisher;

    public ParkingController(IRepositoryManager repository,
        IMapper mapper,
        RepositoryContext context,
        IOptions<PriviledgedUsersConfiguration> priviledgedUsersSettings,
        UserManager<User> userManager,
        IWebHookPayloadProcessor webHookPayloadProcessor,
        IWebSocketSender webSocketSender,
        IWebHookSubscriptionsProvider webHookSubscriptionsProvider,
        IMessagePublisher messagePublisher)

    {
        _repository = repository;
        _mapper = mapper;
        _context = context;
        _priviledgedUsersSettings = priviledgedUsersSettings.Value;
        _userManager = userManager;
        _webHookPayloadProcessor = webHookPayloadProcessor;
        _webSocketSender = webSocketSender;
        _webHookSubscriptionsProvider = webHookSubscriptionsProvider;
        _messagePublisher = messagePublisher;
    }

    [HttpGet("/days")]
    public async Task<IActionResult> GetDays()
    {
        var days = await _repository.Day.GetAllDaysAsync(trackChanges: false);

        return Ok(days);
    }

    [HttpGet("/tenants")]
    public async Task<IActionResult> GetTenants([FromQuery] TenantParameters tenantParameters)
    {
        var tenants = await _repository.Tenant.GetAllTenantsAsync(tenantParameters, trackChanges: false);

        return Ok(tenants);
    }

    [HttpGet("/daysWithTenants")]
    public IActionResult GetDaysWithTheirTenants()
    {
        var context = _context.Days.Include(a => a.Tenants).AsNoTracking();

        var daysWithTenants = context.Select(x => new TenantsForDay
        {
            DayId = x.DayId,
            TenantIds = x.Tenants.Select(t => t.TenantId).ToList()
        }).ToList();

        return Ok(daysWithTenants);
    }

    [HttpGet("/tenantsWithDays")]
    public ActionResult<List<TenantWithDay>> GetTenantsWithTheirDays()
    {
        var context = _context.Tenants.Include(a => a.Days).AsNoTracking();

        var tenantWithDays = context.Select(x => new TenantWithDay
        {
            TenantId = x.TenantId,
            Days = x.Days.Select(t => t.DayId).ToList()
        }).ToList();

        return Ok(tenantWithDays);
    }

    [HttpGet("tenant/{tenantId}/days")]
    public async Task<ActionResult<List<string>>> GetDaysOfTenant([FromRoute] string tenantId)
    {
        var tenant = await _repository.Tenant.GetTenantAsync(tenantId, trackChanges: false);

        if (tenant == null)
        {
            return NotFound();
        }

        var tenantWithDays = _context.Tenants
            .Where(w => w.TenantId == tenantId)
            .Include(a => a.Days)
            .AsNoTracking();

        var days = tenantWithDays.SelectMany(s => s.Days.Select(t => t.DayId)).ToList();

        return Ok(days);
    }

    [HttpGet("day/{dayId}/tenants")]
    public async Task<ActionResult<List<string>>> GetTenantsOfDay([FromRoute] string dayId)
    {
        var day = await _repository.Day.GetDayAsync(dayId, trackChanges: false);

        if (day == null)
        {
            return NotFound();
        }

        var context = _context.Days
            .Where(w => w.DayId == dayId)
            .Include(a => a.Tenants)
            .AsNoTracking();

        var tenants = context.SelectMany(s => s.Tenants.Select(t => t.TenantId)).ToList();

        return Ok(tenants);
    }

    [HttpPost("tenants/multipledays")]
    public ActionResult<List<TenantsForDay>> GetTenantsForMultipleDays([FromBody] List<string> days)
    {
        var context = _context.Days
            .Where(x => days.Contains(x.DayId))
            .Include(a => a.Tenants)
            .AsNoTracking();

        var daysWithTenants = context.Select(x => new TenantsForDay
        {
            DayId = x.DayId,
            TenantIds = x.Tenants.Select(t => t.TenantId).ToList()
        }).OrderBy(o=>o.DayId.Substring(2,2)).ToList();

        return Ok(daysWithTenants);
    }

    [HttpPost("/tenant/create")]
    public async Task<IActionResult> CreateTenant([FromBody] Tenant tenant)
    {
        var tenantInDb = await _repository.Tenant.GetTenantAsync(tenant.TenantId, trackChanges: false);
        if (tenantInDb != null)
        {
            return BadRequest("Tenant already exists");
        }

        _repository.Tenant.CreateTenant(tenant);
        await _repository.SaveAsync();

        return StatusCode(201);
    }

    [HttpPut("tenant/book")]
    public async Task<IActionResult> AddTenantToDay([FromBody] TenantDay tenantDay)
    {
        var tenant = await _repository.Tenant.GetTenantAsync(tenantDay.TenantId, trackChanges: true);

        if (tenant == null)
        {
            return NotFound();
        }

        var day = await _repository.Day.GetDayAsync(tenantDay.DayId, trackChanges: true);

        if (day == null)
        {
            return NotFound();
        }

        var contextTenants = _context.Days
            .Where(z => z.DayId == tenantDay.DayId)
            .Include(a => a.Tenants)
            .AsNoTracking()
            .SelectMany(s => s.Tenants.Select(t => t.TenantId))
            .ToList();

        if (contextTenants.Count >= _priviledgedUsersSettings.MaxCount)
        {
            return BadRequest();
        }

        var contextDays = _context.Tenants
            .Where(z => z.TenantId == tenantDay.TenantId)
            .Include(a => a.Days)
            .AsNoTracking()
            .SelectMany(s => s.Days.Select(d => d.DayId))
            .ToList();

        if (contextDays.Contains(tenantDay.DayId))
        {
            return BadRequest();
        }

        day.Tenants.Add(tenant);
        await _repository.SaveAsync();

        var message = JsonConvert.SerializeObject(new WebSocketMessageDayChange
        {
            Message = WebSocketMessage.ParkingPlaceChange.ToString(),
            TenantId = tenantDay.TenantId
        });

        await _webSocketSender.SendWebSocketMessage(message);

        var subscriptions = await _webHookSubscriptionsProvider.GetSubscriptionsAsync(new CancellationToken());
        if (subscriptions.Count > 0)
        {
            await _messagePublisher.PublishAsync(new WebHookMessage
            {
                CorrelationId = Guid.NewGuid(),
                Value = new WebHookPayload
                {
                    Data = message
                },
                WebHookSubscriptions = subscriptions
            });
        }

        return NoContent();
    }

    [HttpPut("tenant/book/all")]
    public async Task<IActionResult> AddTenantToAllDays([FromBody] TenantSingle tenantSingle)
    {
        var allUsers = _userManager.Users.AsNoTracking();
        var priviledgeUsersCount = allUsers.Count(c => c.Priviledged);

        if (priviledgeUsersCount >= _priviledgedUsersSettings.MaxCount)
        {
            return BadRequest("New privileged user cannot be created, no more free space");
        }

        var tenant = await _repository.Tenant.GetTenantAsync(tenantSingle.TenantId, trackChanges: true);

        if (tenant == null)
        {
            return NotFound();
        }

        var contextDaysWithTenants = _context.Days.Include(a => a.Tenants);

        foreach (var day in contextDaysWithTenants)
        {
            var tenantsColl = day.Tenants
                .Select(da => da.TenantId)
                .ToList();

            if (tenantsColl.Contains(tenantSingle.TenantId) ||
                tenantsColl.Count >= _priviledgedUsersSettings.MaxCount)
            {
                continue;
            }

            day.Tenants.Add(tenant);
        }

        await _repository.SaveAsync();

        var message = JsonConvert.SerializeObject(new WebSocketMessageDayChange
        {
            Message = WebSocketMessage.ParkingPlaceChange.ToString(),
            TenantId = tenantSingle.TenantId
        });

        await _webSocketSender.SendWebSocketMessage(message);

        var subscriptions = await _webHookSubscriptionsProvider.GetSubscriptionsAsync(new CancellationToken());
        if (subscriptions.Count > 0)
        {
            await _messagePublisher.PublishAsync(new WebHookMessage
            {
                CorrelationId = Guid.NewGuid(),
                Value = new WebHookPayload
                {
                    Data = message
                },
                WebHookSubscriptions = subscriptions
            });
        }

        return NoContent();
    }

    [HttpPut("tenant/free/all")]
    public async Task<IActionResult> FreeTenantFromAllDays([FromBody] TenantSingle tenantSingle)
    {
        var tenant = await _repository.Tenant.GetTenantAsync(tenantSingle.TenantId, trackChanges: true);

        if (tenant == null)
        {
            return NotFound();
        }

        var days = _context.Days.Include(p => p.Tenants);

        foreach (var day in days)
        {
            day.Tenants.Remove(tenant);
        }

        await _context.SaveChangesAsync();

        var message = JsonConvert.SerializeObject(new WebSocketMessageDayChange
        {
            Message = WebSocketMessage.ParkingPlaceChange.ToString(),
            TenantId = tenantSingle.TenantId
        });

        await _webSocketSender.SendWebSocketMessage(message);

        var subscriptions = await _webHookSubscriptionsProvider.GetSubscriptionsAsync(new CancellationToken());
        if (subscriptions.Count > 0)
        {
            await _messagePublisher.PublishAsync(new WebHookMessage
            {
                CorrelationId = Guid.NewGuid(),
                Value = new WebHookPayload
                {
                    Data = message
                },
                WebHookSubscriptions = subscriptions
            });
        }

        return NoContent();
    }

    [HttpPut("tenant/free")]
    public async Task<IActionResult> RemoveTenantFromDay([FromBody] TenantDay tenantDay)
    {
        var tenant = await _repository.Tenant.GetTenantAsync(tenantDay.TenantId, trackChanges: true);
        if (tenant == null)
        {
            return NotFound();
        }

        var dayInDb = await _repository.Day.GetDayAsync(tenantDay.DayId, trackChanges: true);
        if (dayInDb == null)
        {
            return NotFound();
        }

        //Direct many-to-many usage: Remove a link https://www.thereformedprogrammer.net/updating-many-to-many-relationships-in-ef-core-5-and-above/
        var day = _context.Days
            .Where(s => s.DayId == tenantDay.DayId)
            .Include(p => p.Tenants)
            .Single();
        var tenantToRemove = day.Tenants
            .SingleOrDefault(x => x.TenantId == tenantDay.TenantId);

        if (tenantToRemove == null)
        {
            return BadRequest();
        }

        day.Tenants.Remove(tenantToRemove);
        await _context.SaveChangesAsync();

        var message = JsonConvert.SerializeObject(new WebSocketMessageDayChange
        {
            Message = WebSocketMessage.ParkingPlaceChange.ToString(),
            TenantId = tenantDay.TenantId
        });

        await _webSocketSender.SendWebSocketMessage(message);

        var subscriptions = await _webHookSubscriptionsProvider.GetSubscriptionsAsync(new CancellationToken());
        if (subscriptions.Count > 0)
        {
            await _messagePublisher.PublishAsync(new WebHookMessage
            {
                CorrelationId = Guid.NewGuid(),
                Value = new WebHookPayload
                {
                    Data = message
                },
                WebHookSubscriptions = subscriptions
            });
        }

        return NoContent();
    }

    [HttpDelete("/tenant/{tenantId}")]
    public async Task<IActionResult> DeleteTenant([FromRoute] string tenantId)
    {
        var tenant = await _repository.Tenant.GetTenantAsync(tenantId, trackChanges: true);

        if (tenant == null)
        {
            return NotFound();
        }

        _repository.Tenant.DeleteTenant(tenant);
        await _repository.SaveAsync();

        return NoContent();
    }
}