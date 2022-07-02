using System;

namespace WebHooks.Contracts
{
    public interface IMessage
    {
        Guid CorrelationId { get; set; }
    }
}