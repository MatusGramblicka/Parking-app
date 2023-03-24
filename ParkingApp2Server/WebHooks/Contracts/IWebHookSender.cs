using System.Threading;
using System.Threading.Tasks;

namespace WebHooks.Contracts;

public interface IWebHookSender
{
    Task<WebHookSendResponse> SendAsync(WebHookSendArgs args, CancellationToken cancellationToken);
    WebHookSendResponse Send(WebHookSendArgs args);
}