using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.RequestFeatures;
using Entities.DataTransferObjects;
using Entities;
using Microsoft.EntityFrameworkCore;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;

namespace ParkingApp2Server.Controllers
{
    [Route("api/parking")]
    [ApiController]
    //[Authorize]
    public class ParkingController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly RepositoryContext _context;

        public ParkingController(IRepositoryManager repository, IMapper mapper, RepositoryContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [Route("/days")]
        public async Task<IActionResult> GetDays()
        {
            var days = await _repository.Day.GetAllDaysAsync(trackChanges: false);

            //var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            //return Ok(companiesDto);
            return Ok(days);
        }

        [HttpGet]
        [Route("/daysExplicit")]
        public async Task<IActionResult> GetDaysWithTenants()
        {
            var days = await _repository.Day.GetAllDaysAsync(trackChanges: false);
            var daysWithTenants = new List<DayWithTenant>();

            foreach (var day in days)
            {
                var tenantRes = new List<string>();
                var contextTenants = _context.Days.Include(a => a.Tenants);
                var tenantsColl = contextTenants.Where(z => z.DayId.Equals(day.DayId)).Select(s => s.Tenants).ToList();
                foreach (var tenantColl in tenantsColl)
                {
                    tenantRes = tenantColl.Select(da => da.TenantId).ToList();
                }

                var dayWithTenants = new DayWithTenant
                {
                    DayId = day.DayId,
                    Tenants = tenantRes
                };

                daysWithTenants.Add(dayWithTenants);

            }
            return Ok(daysWithTenants);
        }
        

        [HttpGet]
        [Route("/tenants")]
        public async Task<IActionResult> GetTenants([FromQuery] TenantParameters tenantParameters)
        {
            var tenants = await _repository.Tenant.GetAllTenantsAsync(tenantParameters, trackChanges: false);

            //var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return Ok(tenants);
        }

        [HttpGet]
        [Route("/day")]
        public async Task<IActionResult> GetDay([FromQuery] string dayId)
        {
            var day = await _repository.Day.GetDayAsync(dayId, trackChanges: false);

            //var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return Ok(day);
        }

        [HttpGet("tenant/{tenantId}")]
        //[Route("/tenant")]
        public async Task<IActionResult> GetTenant([FromRoute] string tenantId)
        {
            var tenant = await _repository.Tenant.GetTenantAsync(tenantId, trackChanges: false);

            if (tenant == null)
                return NotFound();
            //var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return Ok(tenant);
        }

        [HttpGet("tenant/days/{tenantId}")]
        //[Route("/tenant/day/{tenantId}")]
        public async Task<IActionResult> GetTenantForDay([FromRoute] string tenantId)
        {
            var tenant = await _repository.Tenant.GetTenantAsync(tenantId, trackChanges: false);

            if (tenant == null)
            {
                return NotFound();
            }

            /// todo put into separate method, there are duplicates
            var days = new List<string>();
            var contextDays = _context.Tenants.Include(a => a.Days);
            var daysColl = contextDays.Where(z => z.TenantId.Equals(tenantId)).Select(s => s.Days).ToList();
            foreach (var dayColl in daysColl)
            {
                days = dayColl.Select(da => da.DayId).ToList();
            }

            return Ok(days);
        }

        [HttpGet("tenants/day/{dayId}")]
        //[Route("/tenants/day/{dayId}")]
        public async Task<IActionResult> GetTenantsForDay([FromRoute] string dayId)
        {
            //var tenant = await _repository.Tenant.GetTenantAsync(dayId, trackChanges: false);

            //var day = await _repository.Day.GetDayAsync(dayId, trackChanges: false);

            //if (day == null)
            //{
            //    //_logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
            //    return NotFound();
            //}

            //https://stackoverflow.com/questions/52212247/entity-framework-core-returning-object-with-many-to-many-relationship
            var tenantRes = new List<string>();
            var contextTenants = _context.Days.Include(a => a.Tenants);
            var tenantsColl = contextTenants.Where(z => z.DayId.Equals(dayId)).Select(s => s.Tenants).ToList();
            foreach (var tenantColl in tenantsColl)
            {
                tenantRes = tenantColl.Select(da => da.TenantId).ToList();
            }

            return Ok(tenantRes);
        }

        [HttpPost]
        [Route("/tenant/create")]
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
        //[Route("/tenant/book")]
        public async Task<IActionResult> AddTenantToDay([FromBody] TenantDay tenantDay)
        {
            var tenant = await _repository.Tenant.GetTenantAsync(tenantDay.TenantId, trackChanges: true);

            if (tenant == null)
            {
                //_logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var day = await _repository.Day.GetDayAsync(tenantDay.DayId, trackChanges: true);

            if (day == null)
            {
                //_logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var days = new List<string>();
            var contextDays = _context.Tenants.Include(a => a.Days);
            var daysColl = contextDays.Where(z => z.TenantId.Equals(tenantDay.TenantId)).Select(s => s.Days).ToList();
            foreach (var dayColl in daysColl)
            {
                days = dayColl.Select(da => da.DayId).ToList();
            }

            if (days.Contains(tenantDay.DayId))
                return BadRequest();

            day.Tenants.Add(tenant);

            await _repository.SaveAsync();
            
            //var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return NoContent();
        }

        [HttpPut("tenant/free")]       
        public async Task<IActionResult> RemoveTenantFromDay([FromBody] TenantDay tenantDay)
        {
            var tenant = await _repository.Tenant.GetTenantAsync(tenantDay.TenantId, trackChanges: true);

            if (tenant == null)
            {
                //_logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var dayInDb = await _repository.Day.GetDayAsync(tenantDay.DayId, trackChanges: true);

            if (dayInDb == null)
            {
                //_logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            //Direct many-to-many usage: Remove a link
            //https://www.thereformedprogrammer.net/updating-many-to-many-relationships-in-ef-core-5-and-above/
            var day = _context.Days.Include(p => p.Tenants).Single(s => s.DayId == tenantDay.DayId);
            var tenToRemove = day.Tenants.Single(x => x.TenantId == tenantDay.TenantId);
            day.Tenants.Remove(tenToRemove);

            await _context.SaveChangesAsync();

            //var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return NoContent();
        }        

        [HttpDelete]
        [Route("/tenant/{tenantId}")]
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
}
