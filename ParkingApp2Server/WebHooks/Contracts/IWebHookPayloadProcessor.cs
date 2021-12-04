using Entities.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebHooks.Contracts
{
    public interface IWebHookPayloadProcessor
    {
        Task SendWebHookAsync(List<WebHookSubscriptionDto> webHookSubscriptions, WebHookPayload value);
    }
}
