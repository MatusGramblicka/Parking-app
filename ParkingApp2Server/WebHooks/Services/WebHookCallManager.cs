using Common.Utils.Utils;
using Entities.Enums;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Timeout;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebHooks.Contracts;

namespace WebHooks.Services
{
    public class WebHookCallManager : IWebHookCallManager
    {
        private readonly ILogger<WebHookCallManager> _logger;
        private readonly IWebHookSender _webHookSender;

        public WebHookCallManager(IWebHookSender webHookSender, ILogger<WebHookCallManager> logger)
        {
            _webHookSender = webHookSender;
            _logger = logger;
        }

        public async Task ExecuteWebHookCallAsync(WebHookCallContext context, CancellationToken cancellationToken)
        {
            var outputPolicy = QueryTimeoutPolicyAsync<WebHookSendResponse>(context.CallTimeout)
                .WrapAsync(ExponentialRetryPolicyAsync<WebHookSendResponse>(_logger, r => !r.IsSucceed,
                    context.SubscriptionDetails.MaxSendAttemptCount));

            var arg = new WebHookSendArgs
            {
                Data = context.Payload.Data,
                Headers = context.SubscriptionDetails.Headers,
                RequestTimeout = context.CallTimeout,
                SignatureHeaderName = context.SubscriptionDetails.SignatureHeaderName,
                SigningSecret = context.SubscriptionDetails.SigningSecret,
                SubscriptionId = context.SubscriptionDetails.Id,
                WebHookUri = context.SubscriptionDetails.WebHookUri
            };
            try
            {
                await outputPolicy.ExecuteAsync(ct => _webHookSender.SendAsync(arg, ct), cancellationToken);
            }
            catch (Exception ex)
            {
                if (context.SubscriptionDetails.FailureHandlingStrategyFlags.HasFlag(FailureHandlingStrategy
                    .LogFailure))
                {
                    _logger.LogError(ex,
                        $"Unable to execute WebHooks call for subscription: {context.SubscriptionDetails.Id}");
                }

                if (context.SubscriptionDetails.FailureHandlingStrategyFlags.HasFlag(FailureHandlingStrategy
                    .DeactivateSubscription))
                {
                    //noop
                }
            }
        }

        public void ExecuteWebHookCall(WebHookCallContext context)
        {
            AsyncHelper.RunSync(() => ExecuteWebHookCallAsync(context, new CancellationToken()));
        }

        private AsyncPolicy<T> QueryTimeoutPolicyAsync<T>(TimeSpan timeout)
        {
            return Policy.TimeoutAsync<T>(timeout, TimeoutStrategy.Optimistic);
        }

        private AsyncPolicy<T> ExponentialRetryPolicyAsync<T>(ILogger retryLogger, Predicate<T> untilNotValidPredicate,
            int retryCount = 5)
        {
            return Policy.HandleResult<T>(r => untilNotValidPredicate(r)).RetryAsync(retryCount,
                async (outcome, retryNumber, context) =>
                {
                    var factor = Math.Pow(2, retryNumber - 1);
                    var delay = TimeSpan.FromMilliseconds(100 * factor);
                    retryLogger.LogInformation(
                        $"Next retry attempt after:{delay.TotalMilliseconds} ms, previous attempt result: {outcome.Result}");
                    await Task.Delay(delay).ConfigureAwait(false);
                });
        }
    }
}
