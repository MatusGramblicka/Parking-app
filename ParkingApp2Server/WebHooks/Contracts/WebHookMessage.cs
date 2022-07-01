using Entities.DTO;
using System;
using System.Collections.Generic;

namespace WebHooks.Contracts
{
    public class WebHookMessage : IMessage
    {
        public List<WebHookSubscriptionDto> WebHookSubscriptions { get; set; }
        public WebHookPayload Value { get; set; }
        public Guid CorrelationId { get; set; }
    }
}