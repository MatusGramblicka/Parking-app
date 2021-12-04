using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class WebHookRepository : RepositoryBase<WebHookSubscription>, IWebHookRepository
    {
        public WebHookRepository(RepositoryContext repositoryContext)
               : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<WebHookSubscription>> GetAllWebHookSubscriptionsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
              .OrderBy(e => e.Id)
              .ToListAsync();        

        public async Task<WebHookSubscription> GetWebHookSubscriptionAsync(Guid webHookSubscriptionId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(webHookSubscriptionId), trackChanges)
            .SingleOrDefaultAsync();        

        public void CreateWebHookSubscription(WebHookSubscription webHookSubscription)
        {
            Create(webHookSubscription);
        }

        public void DeleteWebHookSubscription(WebHookSubscription webHookSubscription)
        {
            Delete(webHookSubscription);
        }
    }
}
