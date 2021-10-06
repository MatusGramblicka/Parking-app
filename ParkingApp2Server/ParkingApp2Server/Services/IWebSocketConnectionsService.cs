using System;
using System.Threading;
using System.Threading.Tasks;
using ParkingApp2Server.Infrastructure;

namespace ParkingApp2Server.Services
{
    public interface IWebSocketConnectionsService
    {
        void AddConnection(WebSocketConnection connection);

        void RemoveConnection(Guid connectionId);

        Task SendToAllAsync(string message, CancellationToken cancellationToken);
    }
}
