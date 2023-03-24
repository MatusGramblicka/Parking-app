using System.Threading.Tasks;

namespace WebHooks.Contracts;

public interface IWebSocketSender
{
    Task SendWebSocketMessage(string message);
}