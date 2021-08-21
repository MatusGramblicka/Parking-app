using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Entities.Configuration
{
    class DayConfiguration : IEntityTypeConfiguration<Day>
    {
        public void Configure(EntityTypeBuilder<Day> builder)
        {
            List<Day> dayList = new List<Day>();
            DateTime firstDay = new DateTime(2024, 1, 1);
            for (int i = 0; i < 366; i++)
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
                //for (int i = 0; i<366; i++)
                //{
                //    new Day
                //    {
                //        DayId = "01",
                //        Tenants = new List<Tenant>()
                //    }
                //},
                dayList

                //new Day
                //{
                //    DayId = "01",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "02",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "03",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "04",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "05",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "06",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "07",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "08",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "09",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "10",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "11",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "12",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "13",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "14",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "15",
                //    Tenants = new List<Tenant>()
                //}, new Day
                //{
                //    DayId = "16",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "17",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "18",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "19",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "20",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "21",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "22",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "23",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "24",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "25",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "26",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "27",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "28",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "29",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "30",
                //    Tenants = new List<Tenant>()
                //},
                //new Day
                //{
                //    DayId = "31",
                //    Tenants = new List<Tenant>()
                //}
            );
        }
    }
}
