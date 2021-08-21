using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IDayRepository
    {
        Task<IEnumerable<Day>> GetAllDaysAsync(bool trackChanges);
        Task<Day> GetDayAsync(string NameDay, bool trackChanges);
        //void CreateDay(Day day);
        Task<IEnumerable<Day>> GetByNamesAsync(IEnumerable<string> dayNames, bool trackChanges);

        //void AddTenantToDay(Day day, Tenant tenant);
        //void DeleteDay(Day day);
    }
}
