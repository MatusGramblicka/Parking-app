using System.Collections.Generic;

namespace Entities.DataTransferObjects
{
    public class DayWithTenant
    {
        public string DayId { get; set; }
        public List<string> Tenants { get; set; } = new List<string>();

    }
}
