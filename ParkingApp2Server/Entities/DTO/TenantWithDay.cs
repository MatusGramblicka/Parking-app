using System.Collections.Generic;

namespace Entities.DTO
{
    public class TenantWithDay
    {
        public string TenantId { get; set; }
        public List<string> Days { get; set; } = new List<string>();
    }
}