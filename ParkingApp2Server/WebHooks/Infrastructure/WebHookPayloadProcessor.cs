using Entities.DTO;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebHooks.Contracts;
using WebHooks.Utils;

namespace WebHooks.Infrastructure
{
    public class WebHookPayloadProcessor : IWebHookPayloadProcessor/*, IDisposable*/
    {
        private readonly TimeSpan
            _callTimeout = TimeSpan.FromSeconds(10); //TODO move to websockets global configuration when available

        private readonly CancellationTokenSource _cts;
        private readonly ILogger<WebHookPayloadProcessor> _logger;
        private readonly BlockingCollection<WebHookCallContext> _queue = new BlockingCollection<WebHookCallContext>();
        private readonly IWebHookCallManagerFactory _webHookCallManagerFactory;
        //private readonly IWebHookSubscriptionsProvider _webHookSubscriptionsProvider;
        private readonly ConcurrentBag<Task> _workers = new ConcurrentBag<Task>();

        public WebHookPayloadProcessor(ILogger<WebHookPayloadProcessor> logger,
            //IWebHookSubscriptionsProvider webHookSubscriptionsProvider,
            IWebHookCallManagerFactory webHookCallManagerFactory)
        {
            _logger = logger;
            //_webHookSubscriptionsProvider = webHookSubscriptionsProvider;
            _webHookCallManagerFactory = webHookCallManagerFactory;
            _cts = new CancellationTokenSource();
            for (var i = 0; i < Environment.ProcessorCount * 2; i++)
            {
                var task = WebHookPayloadWorker(_cts.Token);
                _workers.Add(task);
            }

            _logger.LogInformation("WebHookPayloadProcessor service running.");
        }
        
        public async Task SendWebHookAsync(List<WebHookSubscriptionDto> webHookSubscriptions, WebHookPayload value)
        {            
            //var subscriptions = _webHookSubscriptionsProvider.GetSubscriptions();
            //var subscriptions = await _webHookSubscriptionsProvider.GetSubscriptionsAsync(new CancellationToken());
            webHookSubscriptions.ForEach(s => _queue.Add(new WebHookCallContext
            {
                CallTimeout = _callTimeout,
                Payload = value,
                SubscriptionDetails = s
            }));
            await WebHookPayloadWorker(_cts.Token);
            //subscriptions.Clear();
        }        

        private WebHookCallContext GetNextRequest(CancellationToken ct, int timeout)
        {
            return _queue.TryTake(out var nextRequest, timeout, ct) ? nextRequest : null;
        }

        private Task WebHookPayloadWorker(CancellationToken ct)
        {
            return Task.Run(async () =>
            {
                _logger.LogTrace($"WebHookPayloadWorker [{Task.CurrentId}], starting loop");

                //while (!ct.IsCancellationRequested)
                while(_queue.Count > 0)
                {
                    try
                    {
                        var currentRequest = GetNextRequest(ct, 5000);

                        if (currentRequest == null) //no waiting request
                        {
                            _logger.LogTrace($"WebHookPayloadWorker [{Task.CurrentId}], no request in queue");
                            continue;
                        }

                        var callManager = _webHookCallManagerFactory.GetNew();
                        await callManager.ExecuteWebHookCallAsync(currentRequest, ct);
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"WebHookPayloadWorker [{Task.CurrentId}], unhandled exception");
                        //TODO throw exception?
                    }
                }

                _logger.LogTrace($"WebHookPayloadWorker [{Task.CurrentId}], finishing loop");
            }, ct);
        }

        //private async Task StopAsync()
        //{
        //    _queue.CompleteAdding();
        //    _cts?.Cancel();
        //    await Task.WhenAll(_workers);
        //    _logger.LogInformation("WebHookPayloadProcessor service is stopping.");
        //}

        //public void Dispose()
        //{
        //    AsyncHelper.RunSync(StopAsync);
        //    _logger.LogInformation("API WebHookPayloadProcessor is completed");
        //}
    }
}
