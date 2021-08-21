using Entities.DataTransferObjects;
using Entities.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorProducts.Client.HttpRepository
{
    public class TenantDayRepo : ITenantDayRepo
    {
        private readonly HttpClient _client;

        public TenantDayRepo(HttpClient client)
        {
            _client = client;           
        }

        public async Task<Tenant> GetTenant(string tenantId)
        {
            var tenant = await _client.GetFromJsonAsync<Tenant>($"parking/tenant/{tenantId}");

            return tenant;
        }

        public async Task<List<string>> GetTenantDays(string tenantId)
        {
            var tenantDays = await _client.GetFromJsonAsync<List<string>>($"parking/tenant/days/{tenantId}");

            return tenantDays;
        }

        public async Task<List<string>> GetDaysForTenant(string dayId)
        {
            var tenantsForDay = await _client.GetFromJsonAsync<List<string>>($"parking/tenants/day/{dayId}");

            return tenantsForDay;
        }      

        public async Task BookDay(TenantDay tenant)
            => await _client.PutAsJsonAsync("parking/tenant/book", tenant);

        public async Task FreeDay(TenantDay tenant)
            => await _client.PutAsJsonAsync("parking/tenant/free", tenant);
    }
}
