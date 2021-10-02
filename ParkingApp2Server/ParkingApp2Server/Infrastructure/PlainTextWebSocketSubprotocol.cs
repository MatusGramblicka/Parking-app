namespace ParkingApp2Server.Infrastructure
{
    public class PlainTextWebSocketSubprotocol : TextWebSocketSubprotocolBase, ITextWebSocketSubprotocol
    {
        public string SubProtocol => "aspnetcore-ws.plaintext";
    }
}
