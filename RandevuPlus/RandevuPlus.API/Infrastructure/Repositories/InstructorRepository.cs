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

        public async Task<Instructor?> GetByUserIdAsync(Guid userId)
        {
            return await _dbSet.FirstOrDefaultAsync(i => i.UserId == userId);   
        }
    }
}
