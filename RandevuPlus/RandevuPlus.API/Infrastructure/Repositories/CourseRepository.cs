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
    }
}
