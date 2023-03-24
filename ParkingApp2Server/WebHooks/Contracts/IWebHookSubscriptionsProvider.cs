using Entities.DTO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebHooks.Contracts;

public interface IWebHookSubscriptionsProvider
{
    Task<List<WebHookSubscriptionDto>> GetSubscriptionsAsync(CancellationToken cancellationToken,
        bool activeOnly = true);

    //List<WebHookSubscriptionDto> GetSubscriptions(bool activeOnly = true);
}