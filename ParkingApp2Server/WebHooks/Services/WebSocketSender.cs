﻿using System.Threading.Tasks;
using WebHooks.Contracts;
using WebSocket.Contracts;

namespace WebHooks.Services;

public class WebSocketSender : IWebSocketSender
{
    private readonly IWebSocketConnectionsService _webSocketConnectionsService;

    public WebSocketSender(IWebSocketConnectionsService webSocketConnectionsService)
    {
        _webSocketConnectionsService = webSocketConnectionsService;
    }

    public async Task SendWebSocketMessage(string message)
    {
        await _webSocketConnectionsService.SendToAllAsync(message, default);
    }
}