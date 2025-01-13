using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Infrastructure.Data;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Enums;
using RandevuPlus.API.Shared.Interfaces.Repositories;

namespace RandevuPlus.API.Infrastructure.Repositories
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Appointment>> GetInstructorAppointmentsAsync(Guid instructorId)
        {
            return await _dbSet.Include(x => x.Course).Include(x => x.Instructor)
                .Where(x => x.InstructorId == instructorId && x.Status != AppointmentStatus.Draft).ToListAsync();
        }
        public async Task<List<Appointment>> GetUserAppointmentsAsync(Guid userId)
        {
            return await _dbSet.Include(x => x.Course).Include(x => x.User)
                .Where(x => x.UserId == userId && x.Status != AppointmentStatus.Draft).ToListAsync();
        }
        public async Task<List<Appointment>> GetInstructorAppointmentsByDateAsync(Guid instructorId, DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Include(x => x.Course).Include(x => x.Instructor)
                .Where(x => x.InstructorId == instructorId && x.Date >= startDate && x.Date <= endDate && x.Status != AppointmentStatus.Draft).ToListAsync();
        }
        public async Task<List<Appointment>> GetUserAppointmentsByDateAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Include(x => x.Course).Include(x => x.User)
                .Where(x => x.UserId == userId && x.Date >= startDate && x.Date <= endDate && x.Status != AppointmentStatus.Draft).ToListAsync();
        }

        public async Task<List<Instructor>> SearchUsersAppointedInstructorsAsync(Guid userId, string prefix)
        {
            return await _dbSet.Include(x => x.Instructor)
                .Where(x => x.UserId == userId && x.Status != AppointmentStatus.Draft)
                //.Where(x => x.UserId == userId && x.Instructor.Name.Contains(prefix) && x.Status != AppointmentStatus.Draft)
                .Select(x => x.Instructor).ToListAsync();
        }

        public async Task<List<AppUser>> SearchInstructorsAppointedUsersAsync(Guid instructorId, string prefix)
        {
            return await _dbSet.Include(x => x.User)
                .Where(x => x.InstructorId == instructorId && (x.User.UserName != null && x.User.UserName.Contains(prefix)) && x.Status != AppointmentStatus.Draft)
                .Select(x => x.User).ToListAsync();
        }

        public async Task<bool> CheckAppointmentAsync(Guid userId, Guid instructorId)
        {
            return await _dbSet.AnyAsync(x => x.UserId == userId && x.InstructorId == instructorId && x.Status != AppointmentStatus.Draft);
               
        }
    }
}
