using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ITenantRepository
    {
        Task<PagedList<Tenant>> GetAllTenantsAsync(TenantParameters tenantParameters, bool trackChanges);
        Task<PagedList<Tenant>> GetTenantsAsync(string nameDay, TenantParameters tenantParameters, bool trackChanges);
        Task<Tenant> GetTenantAsync(string name, bool trackChanges);
        void CreateTenant(Tenant tenant);
        //Task<Tenant> GetTenantAsync(string dayName, string name, bool trackChanges);
        //void CreateTenantForDay(string nameDay, Tenant tenant);
        void DeleteTenant(Tenant tenant);
    }
}
