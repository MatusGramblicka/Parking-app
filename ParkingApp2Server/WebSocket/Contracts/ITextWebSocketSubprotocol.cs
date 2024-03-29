﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocket.Contracts;

public interface ITextWebSocketSubprotocol
{
    string SubProtocol { get; }

    Task SendAsync(string message, Func<byte[], CancellationToken, Task> sendMessageBytesAsync,
        CancellationToken cancellationToken);

    string Read(string rawMessage);
}