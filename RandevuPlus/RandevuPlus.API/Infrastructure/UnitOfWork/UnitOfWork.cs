using Microsoft.EntityFrameworkCore.Storage;
using RandevuPlus.API.Infrastructure.Data;
using RandevuPlus.API.Infrastructure.Repositories;
using RandevuPlus.API.Shared.Interfaces.Repositories;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private IDbContextTransaction _transaction;

        private AppointmentRepository _appointmentRepository;
        private AppointmentChangeRequestRepository _appointmentChangeRequestRepository;
        private AvailabilityRepository _availabilityRepository;
        private CourseRepository _courseRepository;
        private CoursePricingTierRepository _coursePricingTierRepository;
        private InstructorRepository _instructorRepository;
        private InstructorReviewRepository _instructorReviewRepository;
        private MessageRepository _messageRepository;
        private NotificationRepository _notificationRepository;
        private PurchaseRepository _purchaseRepository;
        private UserRepository _userRepository;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public IAppointmentRepository Appointments => _appointmentRepository = _appointmentRepository ?? new AppointmentRepository(_dbContext);
        public IAppointmentChangeRequestRepository AppointmentChangeRequests => _appointmentChangeRequestRepository = _appointmentChangeRequestRepository ?? new AppointmentChangeRequestRepository(_dbContext);
        public IAvailabilityRepository Availabilities => _availabilityRepository = _availabilityRepository ?? new AvailabilityRepository(_dbContext);
        public ICourseRepository Courses => _courseRepository = _courseRepository ?? new CourseRepository(_dbContext);
        public ICoursePricingTierRepository CoursePricingTiers => _coursePricingTierRepository = _coursePricingTierRepository ?? new CoursePricingTierRepository(_dbContext);
        public IInstructorRepository Instructors => _instructorRepository = _instructorRepository ?? new InstructorRepository(_dbContext);
        public IInstructorReviewRepository InstructorReviews => _instructorReviewRepository = _instructorReviewRepository ?? new InstructorReviewRepository(_dbContext);
        public IMessageRepository Messages => _messageRepository = _messageRepository ?? new MessageRepository(_dbContext);
        public INotificationRepository Notifications => _notificationRepository = _notificationRepository ?? new NotificationRepository(_dbContext);
        public IPurchaseRepository Purchases => _purchaseRepository = _purchaseRepository ?? new PurchaseRepository(_dbContext);
        public IUserRepository Users => _userRepository = _userRepository ?? new UserRepository(_dbContext);

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
