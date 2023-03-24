using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models;

public class Day
{
    [Required(ErrorMessage = "NameDay is a required field.")]
    [MaxLength(4, ErrorMessage = "Maximum length for the NameDay is 4 characters.")]
    public string DayId { get; set; }

    public ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();
}