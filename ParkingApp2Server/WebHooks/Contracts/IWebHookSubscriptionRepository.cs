using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebHooks.Contracts;

public interface IWebHookSubscriptionRepository
{
    Task<WebHookSubscription> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<List<WebHookSubscription>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(WebHookSubscription subscription, CancellationToken cancellationToken);
}