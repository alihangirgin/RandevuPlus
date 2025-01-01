using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.Shared.Interfaces.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<int> CountInboxAsync(Guid receiverId);
    }
}
