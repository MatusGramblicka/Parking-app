using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParkingApp2Server.Infrastructure
{
    public abstract class TextWebSocketSubprotocolBase
    {
        public virtual Task SendAsync(string message, Func<byte[], CancellationToken, Task> sendMessageBytesAsync, CancellationToken cancellationToken)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            return sendMessageBytesAsync(messageBytes, cancellationToken);
        }

        public virtual string Read(string webSocketMessage)
        {
            return webSocketMessage;
        }
    }
}
