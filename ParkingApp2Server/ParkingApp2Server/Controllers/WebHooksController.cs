using AutoMapper;
using Entities.DTO;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParkingApp2Server.Controllers
{
    [Route("api/webhooks")]
    [ApiController]
    [Authorize]
    public class WebHooksController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public WebHooksController(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<WebHookSubscription>>> GetSubscriptions()
        {
            var webHookSubscriptionsFromDB = await _repository.WebHook.GetAllWebHookSubscriptionsAsync(trackChanges: false);
            var webHookSubscriptions = _mapper.Map<IEnumerable<WebHookSubscriptionDto>>(webHookSubscriptionsFromDB);

            return Ok(webHookSubscriptions);
        }

        [HttpGet("{id}", Name = "WebHookSubscriptionById")]
        public async Task<ActionResult<WebHookSubscription>> GetSubscription([FromRoute] Guid id)
        {
            var webHookSubscriptionFromDB = await _repository.WebHook.GetWebHookSubscriptionAsync(id, trackChanges: false);
            if (webHookSubscriptionFromDB == null)
            {
                return NotFound();
            }
            var webHookSubscription = _mapper.Map<WebHookSubscriptionDto>(webHookSubscriptionFromDB);

            return Ok(webHookSubscription);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody] WebHookSubscriptionForCreationDto webHookSubscriptionForCreation)
        {
            var webHookSubscription = _mapper.Map<WebHookSubscription>(webHookSubscriptionForCreation);

            _repository.WebHook.CreateWebHookSubscription(webHookSubscription);
            await _repository.SaveAsync();

            var webHookSubscriptionToReturn = _mapper.Map<WebHookSubscriptionDto>(webHookSubscription);

            return CreatedAtRoute("WebHookSubscriptionById", new { id = webHookSubscriptionToReturn.Id }, webHookSubscriptionToReturn);
        }

        [HttpPut("{webHookSubscriptionId}")]
        public async Task<IActionResult> UpdateSubscription([FromRoute] Guid webHookSubscriptionId, WebHookSubscriptionForUpdateDto webHookSubscriptionForUpdate)
        {
            var webHookSubscriptionFromDB = await _repository.WebHook.GetWebHookSubscriptionAsync(webHookSubscriptionId, trackChanges: true);
            if (webHookSubscriptionFromDB == null)
            {
                return NotFound();
            }

            var webHookSubscription = _mapper.Map<WebHookSubscription>(webHookSubscriptionForUpdate);

            _repository.WebHook.CreateWebHookSubscription(webHookSubscription);
            await _repository.SaveAsync();

            var webHookSubscriptionToReturn = _mapper.Map<WebHookSubscriptionDto>(webHookSubscription);

            return CreatedAtRoute("WebHookSubscriptionById", new { id = webHookSubscriptionToReturn.Id }, webHookSubscriptionToReturn);
        }

        [HttpDelete("{webHookSubscriptionId}")]
        public async Task<IActionResult> DeleteSubscription([FromRoute] Guid webHookSubscriptionId)
        {
            var webHookSubscriptionFromDB = await _repository.WebHook.GetWebHookSubscriptionAsync(webHookSubscriptionId, trackChanges: false);
            if (webHookSubscriptionFromDB == null)
            {
                return NotFound();
            }
            _repository.WebHook.DeleteWebHookSubscription(webHookSubscriptionFromDB);
            await _repository.SaveAsync();

            return NoContent();
        }

    }
}
