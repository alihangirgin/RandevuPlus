using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.Shared.Interfaces.Repository
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<List<Appointment>> GetInstructorAppointmentsByDateAsync(Guid instructorId, DateTime startDate, DateTime endDate);
        Task<List<Appointment>> GetUserAppointmentsByDateAsync(Guid userId, DateTime startDate, DateTime endDate);
    }
}
