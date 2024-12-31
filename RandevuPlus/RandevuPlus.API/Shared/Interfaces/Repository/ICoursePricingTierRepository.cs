using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.Shared.Interfaces.Repository
{
    public interface ICoursePricingTierRepository : IRepository<CoursePricingTier>
    {
        Task<bool> DuplicateMinHourExistAsync(Guid courseId, int? minHours);
        Task<bool> DuplicateMaxHourExistAsync(Guid courseId, int? maxHours);
        Task<List<CoursePricingTier>> GetByCourseId(Guid courseId);
    }
}
