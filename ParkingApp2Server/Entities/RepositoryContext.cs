using Entities.Configuration;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Entities
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            //modelBuilder.Entity<DayConfiguration>().HasData(dayList);

            modelBuilder.ApplyConfiguration(new DayConfiguration());
            modelBuilder.ApplyConfiguration(new TenantConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            
        }

        public DbSet<Day> Days { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
    }
}
