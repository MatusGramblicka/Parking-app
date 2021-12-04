using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebHooks.Contracts;
using WebHooks.Extensions;
using WebHooks.Utils;

namespace WebHooks.Services
{
    public class WebHookSender : IWebHookSender
    {
        //HttpClient has been designed to be re-used for multiple calls, but if there is a specific authentication/authorization per client, then move it to send method
        //HttpClient is intended to be instantiated once and re-used throughout the life of an application. 2
        //Especially in server applications, creating a new HttpClient instance for every request will exhaust the number of sockets available under heavy loads.
        //This will result in SocketException errors.
        //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.1
        private const string REQUEST_TIMEOUT_MESSAGE = "Request Timeout";
        private readonly IHttpClientFactory _clientFactory;

        public WebHookSender(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<WebHookSendResponse> SendAsync(WebHookSendArgs args, CancellationToken cancellationToken)
        {
            var request = CreateWebHookRequestMessage(args);
            return await SendHttpRequestAsync(request, args.RequestTimeout, cancellationToken);
        }

        public WebHookSendResponse Send(WebHookSendArgs args)
        {
            var request = CreateWebHookRequestMessage(args);
            return AsyncHelper.RunSync(
                () => SendHttpRequestAsync(request, args.RequestTimeout, new CancellationToken()));
        }

        protected virtual HttpRequestMessage CreateWebHookRequestMessage(WebHookSendArgs webHookSenderArgs)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, webHookSenderArgs.WebHookUri);
            request.SignRequest(webHookSenderArgs.Data, webHookSenderArgs.SigningSecret,
                webHookSenderArgs.SignatureHeaderName);
            if (webHookSenderArgs.Headers != null)
            {
                request.AddAdditionalHeaders(webHookSenderArgs.Headers);
            }

            return request;
        }

        protected virtual async Task<WebHookSendResponse> SendHttpRequestAsync(HttpRequestMessage request,
            TimeSpan timeout, CancellationToken cancellationToken)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                client.Timeout = timeout;
                var response = await client.SendAsync(request, cancellationToken);
                return new WebHookSendResponse
                {
                    IsSucceed = response.IsSuccessStatusCode,
                    StatusCode = response.StatusCode,
                    ResponseContent = await response.Content.ReadAsStringAsync()
                };
            }
            catch (TaskCanceledException)
            {
                return new WebHookSendResponse
                {
                    StatusCode = HttpStatusCode.RequestTimeout,
                    ResponseContent = REQUEST_TIMEOUT_MESSAGE
                };
            }
            catch (Exception e)
            {
                return new WebHookSendResponse
                {
                    ResponseContent = e.Message
                };
            }
        }
    }
}
