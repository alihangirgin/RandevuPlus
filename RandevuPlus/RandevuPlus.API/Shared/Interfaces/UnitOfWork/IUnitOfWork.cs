using RandevuPlus.API.Shared.Interfaces.Repository;

namespace RandevuPlus.API.Shared.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICourseRepository Courses { get; }
        ICoursePricingTierRepository CoursePricingTiers { get; }
        IInstructorRepository Instructors { get; }
        IUserRepository Users { get; }
        Task<int> Commit();
    }
}
