using RandevuPlus.API.Infrastructure.Data;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Repositories;

namespace RandevuPlus.API.Infrastructure.Repositories
{
    public class AppointmentChangeRequestRepository : Repository<AppointmentChangeRequest>, IAppointmentChangeRequestRepository
    {
        public AppointmentChangeRequestRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
