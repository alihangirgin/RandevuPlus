using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Infrastructure.Data;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Repositories;

namespace RandevuPlus.API.Infrastructure.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Course>> GetCoursesByInstructorId(Guid instructorId)
        {
            return await _dbSet.Where(x=> x.InstructorId == instructorId).ToListAsync();    
        }
    }
}
