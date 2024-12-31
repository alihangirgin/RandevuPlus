using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Infrastructure.Data;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Repository;

namespace RandevuPlus.API.Infrastructure.Repositories
{
    public class CoursePricingTierRepository : Repository<CoursePricingTier>, ICoursePricingTierRepository
    {
        public CoursePricingTierRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> DuplicateMinHourExistAsync(Guid courseId, int? minHours)
        {
            return await _dbSet.AnyAsync(cpt => cpt.MinHours == minHours && cpt.CourseId == courseId);
        }

        public async Task<bool> DuplicateMaxHourExistAsync(Guid courseId, int? maxHours)
        {
            return await _dbSet.AnyAsync(cpt => cpt.MaxHours == maxHours && cpt.CourseId == courseId);
        }

        public async Task<List<CoursePricingTier>> GetByCourseId(Guid courseId)
        {
            return await _dbSet.Where(x=> x.CourseId == courseId).ToListAsync();    
        }
    }
}
