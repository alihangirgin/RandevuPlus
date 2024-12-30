using RandevuPlus.API.Infrastructure.Data;
using RandevuPlus.API.Infrastructure.Repositories;
using RandevuPlus.API.Shared.Interfaces.Repository;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private InstructorRepository _instructorRepository;
        private UserRepository _userRepository; 
        public UnitOfWork(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

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
