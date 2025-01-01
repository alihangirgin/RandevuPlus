using RandevuPlus.API.Infrastructure.Data;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Repositories;

namespace RandevuPlus.API.Infrastructure.Repositories
{
    public class InstructorReviewRepository : Repository<InstructorReview>, IInstructorReviewRepository
    {
        public InstructorReviewRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
