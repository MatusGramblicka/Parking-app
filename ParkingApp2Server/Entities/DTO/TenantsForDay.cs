using System.Collections.Generic;

namespace Entities.DTO;

public class TenantsForDay
{        
    public string DayId { get; set; }
    public List<string> TenantIds { get; set; }        
}