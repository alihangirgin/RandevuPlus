using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Infrastructure.Data;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Repository;

namespace RandevuPlus.API.Infrastructure.Repositories
{
    public class AvailabilityRepository : Repository<Availability>, IAvailabilityRepository
    {
        public AvailabilityRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<Availability?> GetAvailabilityByDateAsync(Guid instructorId, DateTime date)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.InstructorId == instructorId && x.Date.Year == date.Year && x.Date.Month == date.Month && x.Date.Day == date.Day);
        }

        public async Task<List<Availability>> GetAvailabilitiesByDateAsync(Guid instructorId, DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(x => x.InstructorId == instructorId && x.Date >= startDate && x.Date <= endDate).ToListAsync();
        }
    }
}
