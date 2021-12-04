using System.Threading;
using System.Threading.Tasks;

namespace WebHooks.Contracts
{
    public interface IWebHookCallManager
    {
        Task ExecuteWebHookCallAsync(WebHookCallContext context, CancellationToken cancellationToken);

        void ExecuteWebHookCall(WebHookCallContext context);
    }
}
