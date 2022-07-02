using Repository.Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class DayRepository : RepositoryBase<Day>, IDayRepository
    {
        public DayRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Day>> GetAllDaysAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(c => c.DayId)
                .ToListAsync();

        public async Task<Day> GetDayAsync(string NameDay, bool trackChanges) =>
            await FindByCondition(c => c.DayId.Equals(NameDay), trackChanges)
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<Day>> GetByNamesAsync(IEnumerable<string> dayNames, bool trackChanges) =>
            await FindByCondition(x => dayNames.Contains(x.DayId), trackChanges)
                .ToListAsync();
    }
}