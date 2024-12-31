using RandevuPlus.API.Infrastructure.Data;
using RandevuPlus.API.Infrastructure.Repositories;
using RandevuPlus.API.Shared.Interfaces.Repository;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        private AvailabilityRepository _availabilityRepository;
        private CourseRepository _courseRepository;
        private CoursePricingTierRepository _coursePricingTierRepository;
        private InstructorRepository _instructorRepository;
        private UserRepository _userRepository;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public IAvailabilityRepository Availabilities => _availabilityRepository = _availabilityRepository ?? new AvailabilityRepository(_dbContext);
        public ICourseRepository Courses => _courseRepository = _courseRepository ?? new CourseRepository(_dbContext);
        public ICoursePricingTierRepository CoursePricingTiers => _coursePricingTierRepository = _coursePricingTierRepository ?? new CoursePricingTierRepository(_dbContext);   
        public IInstructorRepository Instructors => _instructorRepository = _instructorRepository ?? new InstructorRepository(_dbContext);
        public IUserRepository Users => _userRepository = _userRepository ?? new UserRepository(_dbContext);

        public async Task<int> Commit()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
