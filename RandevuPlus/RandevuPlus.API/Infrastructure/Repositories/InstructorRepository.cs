using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Infrastructure.Data;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Repositories;

namespace RandevuPlus.API.Infrastructure.Repositories
{
    public class InstructorRepository : Repository<Instructor>, IInstructorRepository
    {
        public InstructorRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Instructor?> GetByUserIdAsync(Guid userId, List<string>? includes = null)
        {
            var query = _dbSet.AsQueryable();
            if (includes != null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }
            return await query.FirstOrDefaultAsync(i => i.UserId == userId);   
        }
    }
}
