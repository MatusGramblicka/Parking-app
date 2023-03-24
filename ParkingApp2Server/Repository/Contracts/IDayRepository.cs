using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Repository.Contracts;

public interface IDayRepository
{
    Task<IEnumerable<Day>> GetAllDaysAsync(bool trackChanges);
    Task<Day> GetDayAsync(string NameDay, bool trackChanges);
    Task<IEnumerable<Day>> GetByNamesAsync(IEnumerable<string> dayNames, bool trackChanges);
}