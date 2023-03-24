using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models;

public class Tenant
{
    [Required(ErrorMessage = "Employee name is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
    public string TenantId { get; set; }

    public ICollection<Day> Days { get; set; } = new List<Day>();
}