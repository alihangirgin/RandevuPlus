using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.Shared.Interfaces.Repositories
{
    public interface IInstructorRepository : IRepository<Instructor>
    {
        Task<Instructor?> GetByUserIdAsync(Guid userId);
    }
}
