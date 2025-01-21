using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.Shared.Interfaces.Repositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<int> CountNotificationsAsync(Guid receiverId);
    }
}
