using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocket.Infrastructure;

public abstract class TextWebSocketSubprotocolBase
{
    public virtual Task SendAsync(string message, Func<byte[], CancellationToken, Task> sendMessageBytesAsync,
        CancellationToken cancellationToken)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);

        return sendMessageBytesAsync(messageBytes, cancellationToken);
    }

    public virtual string Read(string webSocketMessage)
    {
        return webSocketMessage;
    }
}