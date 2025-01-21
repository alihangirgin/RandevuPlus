using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Infrastructure.Data;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Repositories;

namespace RandevuPlus.API.Infrastructure.Repositories
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<int> CountNotificationsAsync(Guid receiverId)
        {
            return await _dbSet.CountAsync(x => x.ReceiverId == receiverId && !x.IsRead);
        }
    }
}
