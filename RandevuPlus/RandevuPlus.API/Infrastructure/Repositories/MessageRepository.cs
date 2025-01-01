using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Infrastructure.Data;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Repositories;

namespace RandevuPlus.API.Infrastructure.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> CountInboxAsync(Guid receiverId)
        {
            return await _dbSet.CountAsync(x => x.ReceiverId == receiverId && !x.IsRead && !x.IsRemovedFromReceiver);
        }
    }
}
