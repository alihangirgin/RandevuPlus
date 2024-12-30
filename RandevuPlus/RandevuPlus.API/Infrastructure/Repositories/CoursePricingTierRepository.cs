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
    }
}
