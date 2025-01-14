using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.Shared.Interfaces.Repositories
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<List<Appointment>> GetInstructorAppointmentsAsync(Guid instructorId);
        Task<List<Appointment>> GetUserAppointmentsAsync(Guid userId);
        Task<List<Appointment>> GetInstructorAppointmentsByDateAsync(Guid instructorId, DateTime startDate, DateTime endDate);
        Task<List<Appointment>> GetUserAppointmentsByDateAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task<List<Instructor>> SearchUsersAppointedInstructorsAsync(Guid userId, string prefix);
        Task<List<AppUser>> SearchInstructorsAppointedUsersAsync(Guid instructorId, string prefix);
        Task<bool> CheckAppointmentAsync(Guid userId, Guid instructorId);
        Task<List<Appointment>> GetEndedAppointmentsAsync();
    }
}
