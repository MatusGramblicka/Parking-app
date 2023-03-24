using Entities.Configuration;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorProducts.Client.Pages;

public partial class Socket
{
    public WebSocketConfiguration WebSocketConfiguration { get; set; }

    [Inject] public IOptions<WebSocketConfiguration> WebSocketSettings { get; set; }

    //https://gist.github.com/SteveSandersonMS/5aaff6b010b0785075b0a08cc1e40e01
    private CancellationTokenSource _disposalTokenSource = new();
    private ClientWebSocket webSocket = new();
    private string _message = "Websocket!";
    private string _log = "";

    protected override async Task OnInitializedAsync()
    {
        WebSocketConfiguration = WebSocketSettings.Value;
        await webSocket.ConnectAsync(new Uri(WebSocketConfiguration.Connection), _disposalTokenSource.Token);
        _ = ReceiveLoop();
    }

    async Task SendMessageAsync()
    {
        _log += $"Sending: {_message}\n";
        var dataToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(_message));
        await webSocket.SendAsync(dataToSend, WebSocketMessageType.Text, true, _disposalTokenSource.Token);
    }

    async Task ReceiveLoop()
    {
        var buffer = new ArraySegment<byte>(new byte[1024]);
        while (!_disposalTokenSource.IsCancellationRequested)
        {
            // Note that the received block might only be part of a larger message. If this applies in your scenario,
            // check the received.EndOfMessage and consider buffering the blocks until that property is true.
            // Or use a higher-level library such as SignalR.
            var received = await webSocket.ReceiveAsync(buffer, _disposalTokenSource.Token);
            var receivedAsText = Encoding.UTF8.GetString(buffer.Array, 0, received.Count);
            _log += $"Received: {receivedAsText}\n";
            StateHasChanged();
        }
    }

    public void Dispose()
    {
        _disposalTokenSource.Cancel();
        _ = webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bye", CancellationToken.None);
    }
}