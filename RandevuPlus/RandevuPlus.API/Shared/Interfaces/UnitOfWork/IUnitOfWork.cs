using RandevuPlus.API.Shared.Interfaces.Repositories;

namespace RandevuPlus.API.Shared.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IAppointmentRepository Appointments { get; }
        IAppointmentChangeRequestRepository AppointmentChangeRequests { get; }
        IAvailabilityRepository Availabilities { get; }
        ICourseRepository Courses { get; }
        ICoursePricingTierRepository CoursePricingTiers { get; }
        IInstructorRepository Instructors { get; }
        IInstructorReviewRepository InstructorReviews { get; }
        IMessageRepository Messages { get; }
        INotificationRepository Notifications { get; }  
        IPurchaseRepository Purchases { get; }
        IUserRepository Users { get; }
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<int> CommitAsync();
    }
}
