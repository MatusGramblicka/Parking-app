using Entities.DTO;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorProducts.Client.HttpRepository;

public interface IWebHookRepository
{
    Task CreateWebhook(WebHookSubscriptionForCreationDto webhook);
    Task<List<WebHookSubscription>> GetWebhooks();
    Task DeleteWebhook(Guid webhookId);
}