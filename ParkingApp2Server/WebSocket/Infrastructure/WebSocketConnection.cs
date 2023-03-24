using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocket.Contracts;

namespace WebSocket.Infrastructure;

public class WebSocketConnection
{
    private readonly int _receivePayloadBufferSize;
    private readonly int? _sendSegmentSize;

    private readonly System.Net.WebSockets.WebSocket _webSocket;
    private readonly ITextWebSocketSubprotocol _textSubProtocol;

    public Guid Id { get; } = Guid.NewGuid();

    public WebSocketCloseStatus? CloseStatus { get; private set; } = null;

    public string CloseStatusDescription { get; private set; } = null;

    public event EventHandler<string> ReceiveText;

    public event EventHandler<byte[]> ReceiveBinary;

    public WebSocketConnection(System.Net.WebSockets.WebSocket webSocket, ITextWebSocketSubprotocol textSubProtocol,
        int? sendSegmentSize, int receivePayloadBufferSize)
    {
        _webSocket = webSocket ?? throw new ArgumentNullException(nameof(webSocket));
        _textSubProtocol = textSubProtocol ?? throw new ArgumentNullException(nameof(textSubProtocol));
        _sendSegmentSize = sendSegmentSize;
        _receivePayloadBufferSize = receivePayloadBufferSize;
    }

    public Task SendAsync(string message, CancellationToken cancellationToken)
    {
        return _textSubProtocol.SendAsync(message, SendTextMessageBytesAsync, cancellationToken);
    }

    public Task SendAsync(byte[] message, CancellationToken cancellationToken)
    {
        return SendMessageBytesAsync(message, WebSocketMessageType.Binary, cancellationToken: cancellationToken);
    }

    public async Task ReceiveMessagesUntilCloseAsync()
    {
        try
        {
            var receivePayloadBuffer = new byte[_receivePayloadBufferSize];
            var webSocketReceiveResult =
                await _webSocket.ReceiveAsync(new ArraySegment<byte>(receivePayloadBuffer), CancellationToken.None);
            while (webSocketReceiveResult.MessageType != WebSocketMessageType.Close)
            {
                var webSocketMessage =
                    await ReceiveMessagePayloadAsync(webSocketReceiveResult, receivePayloadBuffer);
                if (webSocketReceiveResult.MessageType == WebSocketMessageType.Binary)
                {
                    OnReceiveBinary(webSocketMessage);
                }
                else
                {
                    OnReceiveText(Encoding.UTF8.GetString(webSocketMessage));
                }

                webSocketReceiveResult = await _webSocket.ReceiveAsync(new ArraySegment<byte>(receivePayloadBuffer),
                    CancellationToken.None);
            }

            CloseStatus = webSocketReceiveResult.CloseStatus.Value;
            CloseStatusDescription = webSocketReceiveResult.CloseStatusDescription;
        }
        catch (WebSocketException wsex) when (wsex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
        {
        }
    }

    private Task SendTextMessageBytesAsync(byte[] messageBytes, CancellationToken cancellationToken)
    {
        return SendMessageBytesAsync(messageBytes, WebSocketMessageType.Text, cancellationToken: cancellationToken);
    }

    private async Task SendMessageBytesAsync(byte[] messageBytes, WebSocketMessageType messageType,
        bool compressMessage = true, CancellationToken cancellationToken = default)
    {
        if (_webSocket.State == WebSocketState.Open)
        {
            if (_sendSegmentSize.HasValue && _sendSegmentSize.Value < messageBytes.Length)
            {
                var messageOffset = 0;
                var messageBytesToSend = messageBytes.Length;

                while (messageBytesToSend > 0)
                {
                    var messageSegmentSize = Math.Min(_sendSegmentSize.Value, messageBytesToSend);
                    var messageSegment =
                        new ArraySegment<byte>(messageBytes, messageOffset, messageSegmentSize);

                    messageOffset += messageSegmentSize;
                    messageBytesToSend -= messageSegmentSize;

                    await _webSocket.SendAsync(messageSegment, messageType,
                        GetMessageFlags(messageBytesToSend == 0, !compressMessage), cancellationToken);
                }
            }
            else
            {
                var messageSegment = new ArraySegment<byte>(messageBytes, 0, messageBytes.Length);

                await _webSocket.SendAsync(messageSegment, messageType, GetMessageFlags(true, !compressMessage),
                    cancellationToken);
            }
        }
    }

    private static WebSocketMessageFlags GetMessageFlags(bool endOfMessage, bool disableCompression)
    {
        var messageFlags = WebSocketMessageFlags.None;

        if (endOfMessage)
        {
            messageFlags |= WebSocketMessageFlags.EndOfMessage;
        }

        if (disableCompression)
        {
            messageFlags |= WebSocketMessageFlags.DisableCompression;
        }

        return messageFlags;
    }

    private async Task<byte[]> ReceiveMessagePayloadAsync(WebSocketReceiveResult webSocketReceiveResult,
        byte[] receivePayloadBuffer)
    {
        byte[] messagePayload = null;

        if (webSocketReceiveResult.EndOfMessage)
        {
            messagePayload = new byte[webSocketReceiveResult.Count];
            Array.Copy(receivePayloadBuffer, messagePayload, webSocketReceiveResult.Count);
        }
        else
        {
            using var messagePayloadStream = new MemoryStream();
            messagePayloadStream.Write(receivePayloadBuffer, 0, webSocketReceiveResult.Count);
            while (!webSocketReceiveResult.EndOfMessage)
            {
                webSocketReceiveResult = await _webSocket.ReceiveAsync(new ArraySegment<byte>(receivePayloadBuffer),
                    CancellationToken.None);
                messagePayloadStream.Write(receivePayloadBuffer, 0, webSocketReceiveResult.Count);
            }

            messagePayload = messagePayloadStream.ToArray();
        }

        return messagePayload;
    }

    private void OnReceiveText(string webSocketMessage)
    {
        var message = _textSubProtocol.Read(webSocketMessage);

        ReceiveText?.Invoke(this, message);
    }

    private void OnReceiveBinary(byte[] webSocketMessage)
    {
        ReceiveBinary?.Invoke(this, webSocketMessage);
    }
}