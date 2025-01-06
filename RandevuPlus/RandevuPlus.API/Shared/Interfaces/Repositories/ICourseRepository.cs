using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.Shared.Interfaces.Repositories
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<List<Course>> GetCoursesByInstructorId(Guid instructorId);
    }
}
