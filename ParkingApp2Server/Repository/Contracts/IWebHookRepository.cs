using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IWebHookRepository
    {
        Task<IEnumerable<WebHookSubscription>> GetAllWebHookSubscriptionsAsync(bool trackChanges);        
        Task<WebHookSubscription> GetWebHookSubscriptionAsync(Guid webHookId, bool trackChanges);
        void CreateWebHookSubscription(WebHookSubscription webHook);
        void DeleteWebHookSubscription(WebHookSubscription webHook);
    }
}
