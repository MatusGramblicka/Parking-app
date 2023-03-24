using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Entities.Configuration;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasData
        (
            new Tenant
            {
                TenantId = "Admin",
                Days = new List<Day>()
            },
            new Tenant
            {
                TenantId = "Test",
                Days = new List<Day>()
            }
        );
    }
}