using System.Collections.Generic;

namespace Entities.DataTransferObjects
{
    public class TenantsForDay
    {        
        public string DayId { get; set; }
        public List<string> TenantId { get; set; }        
    }
}
