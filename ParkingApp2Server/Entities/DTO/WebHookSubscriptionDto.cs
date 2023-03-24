using Entities.Enums;
using System;
using System.Collections.Generic;

namespace Entities.DTO;

public class WebHookSubscriptionDto
{
    public Guid Id { get; set; }
    public string WebHookUri { get; set; }
    public string SigningSecret { get; set; }
    public string SignatureHeaderName { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new();
    public int MaxSendAttemptCount { get; set; }
    public bool IsActive { get; set; }
    public FailureHandlingStrategy FailureHandlingStrategyFlags { get; set; }
}