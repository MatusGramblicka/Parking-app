using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.Contracts
{
    public interface ITenantRepository
    {
        Task<PagedList<Tenant>> GetAllTenantsAsync(TenantParameters tenantParameters, bool trackChanges);
        Task<PagedList<Tenant>> GetTenantsAsync(string nameDay, TenantParameters tenantParameters, bool trackChanges);
        Task<Tenant> GetTenantAsync(string name, bool trackChanges);
        void CreateTenant(Tenant tenant);
        void DeleteTenant(Tenant tenant);
    }
}
