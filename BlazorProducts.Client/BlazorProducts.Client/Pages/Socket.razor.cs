using System;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using Entities.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components;

namespace BlazorProducts.Client.Pages
{
    public partial class Socket
    {
        public WebSocketConfiguration WebSocketConfiguration { get; set; }

        [Inject]
        public IOptions<WebSocketConfiguration> WebSocketSettings { get; set; }

        //https://gist.github.com/SteveSandersonMS/5aaff6b010b0785075b0a08cc1e40e01
        private CancellationTokenSource disposalTokenSource = new CancellationTokenSource();
        private ClientWebSocket webSocket = new ClientWebSocket();
        private string message = "Websocket!";
        private string log = "";

        protected override async Task OnInitializedAsync()
        {
            WebSocketConfiguration = WebSocketSettings.Value;
            await webSocket.ConnectAsync(new Uri(WebSocketConfiguration.Connection), disposalTokenSource.Token);
            _ = ReceiveLoop();
        }

        async Task SendMessageAsync()
        {
            log += $"Sending: {message}\n";
            var dataToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
            await webSocket.SendAsync(dataToSend, WebSocketMessageType.Text, true, disposalTokenSource.Token);
        }

        async Task ReceiveLoop()
        {
            var buffer = new ArraySegment<byte>(new byte[1024]);
            while (!disposalTokenSource.IsCancellationRequested)
            {
                // Note that the received block might only be part of a larger message. If this applies in your scenario,
                // check the received.EndOfMessage and consider buffering the blocks until that property is true.
                // Or use a higher-level library such as SignalR.
                var received = await webSocket.ReceiveAsync(buffer, disposalTokenSource.Token);
                var receivedAsText = Encoding.UTF8.GetString(buffer.Array, 0, received.Count);
                log += $"Received: {receivedAsText}\n";
                StateHasChanged();
            }
        }

        public void Dispose()
        {
            disposalTokenSource.Cancel();
            _ = webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bye", CancellationToken.None);
        }
    }
}
