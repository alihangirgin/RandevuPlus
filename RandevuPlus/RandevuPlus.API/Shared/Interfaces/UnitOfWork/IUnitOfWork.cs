using RandevuPlus.API.Shared.Interfaces.Repository;

namespace RandevuPlus.API.Shared.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IAppointmentRepository Appointments { get; }
        IAvailabilityRepository Availabilities { get; }
        ICourseRepository Courses { get; }
        ICoursePricingTierRepository CoursePricingTiers { get; }
        IInstructorRepository Instructors { get; }
        IPurchaseRepository Purchases { get; }
        IUserRepository Users { get; }
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<int> CommitAsync();
    }
}
