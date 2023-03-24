using System;
using System.Collections.Generic;

namespace WebHooks.Contracts;

public class WebHookSendArgs
{
    public Guid SubscriptionId { get; set; }
    public string WebHookUri { get; set; }
    public string SigningSecret { get; set; }
    public string SignatureHeaderName { get; set; }
    public Dictionary<string, string> Headers { get; set; }
    public string Data { get; set; }
    public TimeSpan RequestTimeout { get; set; }
}