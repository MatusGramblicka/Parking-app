using Entities.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProducts.Client.HttpRepository;

public interface ITenantDayRepo
{
    //Task<Tenant> GetTenant(string tenantId);
    Task<IQueryable<string>> GetTenantDays(string tenantId);
    Task<IQueryable<string>> GetDaysForTenant(string dayId);
    Task BookDay(TenantDay tenant);
    Task FreeDay(TenantDay tenant);
    Task BookAllDaysFortenant(TenantSingle tenantSingle);
    Task RemoveAllBookedDaysFromUser(TenantSingle tenantSingle);
    Task<IQueryable<TenantsForDay>> GetMultipleDaysForTenant(List<string> days);
}