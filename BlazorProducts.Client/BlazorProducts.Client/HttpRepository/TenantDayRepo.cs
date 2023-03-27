using Entities.DTO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorProducts.Client.HttpRepository;

public class TenantDayRepo : ITenantDayRepo
{
    private readonly HttpClient _client;

    public TenantDayRepo(HttpClient client)
    {
        _client = client;
    }

    //public async Task<Tenant> GetTenant(string tenantId)
    //{
    //    var tenant = await _client.GetFromJsonAsync<Tenant>($"parking/tenant/{tenantId}");

    //    return tenant;
    //}

    public async Task<IQueryable<string>> GetTenantDays(string tenantId)
    {
        var tenantDays = await _client.GetFromJsonAsync<IQueryable<string>>($"parking/tenant/{tenantId}/days");

        return tenantDays;
    }

    public async Task<IQueryable<string>> GetDaysForTenant(string dayId)
    {
        var tenantsForDay = await _client.GetFromJsonAsync<IQueryable<string>>($"parking/day/{dayId}/tenants");

        return tenantsForDay;
    }

    public async Task<IQueryable<TenantsForDay>> GetMultipleDaysForTenant(List<string> days)
    {
        var result = await _client.PostAsJsonAsync($"parking/tenants/multipledays", days);
        var content = await result.Content.ReadAsStringAsync();

        var tenantsForDays = JsonConvert.DeserializeObject<IQueryable<TenantsForDay>>(content);

        return tenantsForDays;
    }

    public async Task BookDay(TenantDay tenant)
        => await _client.PutAsJsonAsync("parking/tenant/book", tenant);

    public async Task FreeDay(TenantDay tenant)
        => await _client.PutAsJsonAsync("parking/tenant/free", tenant);

    public async Task BookAllDaysFortenant(TenantSingle tenantSingle)
    {
        await _client.PutAsJsonAsync("parking/tenant/book/all", tenantSingle);
    }

    public async Task RemoveAllBookedDaysFromUser(TenantSingle tenantSingle)
    {
        await _client.PutAsJsonAsync("parking/tenant/free/all", tenantSingle);
    }
}