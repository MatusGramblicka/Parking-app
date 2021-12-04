using Entities.DTO;
using System;

namespace WebHooks.Contracts
{
    public class WebHookCallContext
    {
        public WebHookPayload Payload { get; set; }
        public WebHookSubscriptionDto SubscriptionDetails { get; set; }
        public TimeSpan CallTimeout { get; set; }
    }
}
