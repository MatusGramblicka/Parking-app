using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IRepositoryManager
    {
        IDayRepository Day { get; }
        ITenantRepository Tenant { get; }
        IWebHookRepository WebHook { get; }
        Task SaveAsync();
    }
}
