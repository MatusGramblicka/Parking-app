using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Entities.Configuration;

public class DayConfiguration : IEntityTypeConfiguration<Day>
{
    public void Configure(EntityTypeBuilder<Day> builder)
    {
        var dayList = new List<Day>();
        var firstDay = new DateTime(2024, 1, 1);
        for (var i = 0; i < 366; i++)
        {
            var nextDay = firstDay.AddDays(i);
            var nextDayStringRepr = nextDay.ToString("ddMM");
            dayList.Add(new Day
            {
                DayId = nextDayStringRepr,
                Tenants = new List<Tenant>()
            });
        }

        builder.HasData
        (
            dayList
        );
    }
}