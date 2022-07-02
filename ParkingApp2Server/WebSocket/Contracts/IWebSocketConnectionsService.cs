using System;
using System.Threading;
using System.Threading.Tasks;
using WebSocket.Infrastructure;

namespace WebSocket.Contracts
{
    public interface IWebSocketConnectionsService
    {
        void AddConnection(WebSocketConnection connection);

        void RemoveConnection(Guid connectionId);

        Task SendToAllAsync(string message, CancellationToken cancellationToken);
    }
}
