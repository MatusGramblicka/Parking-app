using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class TenantRepository : RepositoryBase<Tenant>, ITenantRepository
    {
        public TenantRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<PagedList<Tenant>> GetAllTenantsAsync(TenantParameters tenantParameters, bool trackChanges)
        {
            var tenants = await FindAll(trackChanges)
              .OrderBy(e => e.TenantId)
              .ToListAsync();

            return PagedList<Tenant>
              .ToPagedList(tenants, tenantParameters.PageNumber, tenantParameters.PageSize);
        }

        public async Task<PagedList<Tenant>> GetTenantsAsync(string nameDay, TenantParameters tenantParameters, bool trackChanges)
        {
            var tenants = await FindByCondition(e => e.Days.Equals(nameDay), trackChanges)
              .OrderBy(e => e.TenantId)
              .ToListAsync();

            return PagedList<Tenant>
              .ToPagedList(tenants, tenantParameters.PageNumber, tenantParameters.PageSize);
        }
        public async Task<Tenant> GetTenantAsync(string name, bool trackChanges) =>
            await FindByCondition(e => e.TenantId.Equals(name), trackChanges)
            .SingleOrDefaultAsync();

        //public async Task<Tenant> GetTenantAsync(string dayName, string name, bool trackChanges) =>
        //    await FindByCondition(e => e.Days.Equals(dayName) && e.TenantId.Equals(name), trackChanges)
        //    .SingleOrDefaultAsync();

        public void CreateTenant(Tenant tenant)
        {            
            Create(tenant);
        }

        //public void CreateTenantForDay(string nameDay, Tenant tenant)
        //{
        //    tenant.Days.Add(nameDay);
        //    Create(tenant);
        //}

        public void DeleteTenant(Tenant tenant)
        {
            Delete(tenant);
        }
    }
}
