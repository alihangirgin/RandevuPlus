using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.Shared.Interfaces.Repositories
{
    public interface IAvailabilityRepository : IRepository<Availability>
    {
        Task<Availability?> GetAvailabilityByDateAsync(Guid instructorId, DateTime date);
        Task<List<Availability>> GetAvailabilitiesByDateAsync(Guid instructorId, DateTime startDate, DateTime endDate);
    }
}
