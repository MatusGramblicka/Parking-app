using Microsoft.Extensions.Logging;
using WebHooks.Contracts;

namespace WebHooks.Services
{
    public class WebHookCallManagerFactory : IWebHookCallManagerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IWebHookSender _webHookSender;

        public WebHookCallManagerFactory(IWebHookSender webHookSender, ILoggerFactory loggerFactory)
        {
            _webHookSender = webHookSender;
            _loggerFactory = loggerFactory;
        }

        public IWebHookCallManager GetNew()
        {
            return new WebHookCallManager(_webHookSender, _loggerFactory.CreateLogger<WebHookCallManager>());
        }
    }
}
