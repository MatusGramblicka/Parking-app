using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebSocket.Contracts;
using WebSocket.Infrastructure;

namespace WebSocket;

public class WebSocketConnectionsService : IWebSocketConnectionsService
{
    private readonly ConcurrentDictionary<Guid, WebSocketConnection> _connections = new();

    public void AddConnection(WebSocketConnection connection)
    {
        _connections.TryAdd(connection.Id, connection);
    }

    public void RemoveConnection(Guid connectionId)
    {
        _connections.TryRemove(connectionId, out var connection);
    }

    public Task SendToAllAsync(string message, CancellationToken cancellationToken)
    {
        var connectionsTasks = new List<Task>();
        foreach (var connection in _connections.Values)
        {
            connectionsTasks.Add(connection.SendAsync(message, cancellationToken));
        }

        return Task.WhenAll(connectionsTasks);
    }
}