using SlimMessageBus;
using System.Threading.Tasks;
using WebHooks.Contracts;

namespace WebHooks.Infrastructure;

public class WebHookMessageEventsBusAdapter : IConsumer<WebHookMessage>
{
    private readonly IWebHookPayloadProcessor _webHookPayloadProcessor;

    public WebHookMessageEventsBusAdapter(IWebHookPayloadProcessor webHookPayloadProcessor)
    {
        _webHookPayloadProcessor = webHookPayloadProcessor;
    }

    public Task OnHandle(WebHookMessage message)
    {
        return Task.Run(() =>
        {
            _webHookPayloadProcessor.SendWebHookAsync(message.WebHookSubscriptions, message.Value);
        });
    }
}