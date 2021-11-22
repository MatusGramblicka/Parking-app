using Entities.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorProducts.Client.HttpRepository
{
    public interface ITenantDayRepo
    {
        //Task<Tenant> GetTenant(string tenantId);
        Task<List<string>> GetTenantDays(string tenantId);
        Task<List<string>> GetDaysForTenant(string dayId);
        Task BookDay(TenantDay tenant);
        Task FreeDay(TenantDay tenant);
        Task BookAllDaysFortenant(TenantSingle tenantSingle);
        Task RemoveAllBookedDaysFromUser(TenantSingle tenantSingle);
        Task<List<TenantsForDay>> GetMultipleDaysForTenant(List<string> days);
    }
}
