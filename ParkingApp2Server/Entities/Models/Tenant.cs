using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Tenant
    {
        //[Column("TenantId")]
        //public Guid Id { get; set; }

        [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string TenantId { get; set; }

        public ICollection<Day> Days { get; set; } = new List<Day>();

        //[ForeignKey(nameof(Day))]
        //public string NameDay { get; set; }
    }
}
