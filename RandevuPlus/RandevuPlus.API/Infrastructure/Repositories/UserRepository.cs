using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Infrastructure.Data;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Repositories;

namespace RandevuPlus.API.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<AppUser> AddAsync(AppUser user)
        {
            await _context.Users.AddAsync(user);
            return user;
        }

        public async Task<bool> CheckAsync(Guid id)
        {
            return await _context.Users.AnyAsync(x => x.Id == id);
        }

        public async Task<List<AppUser>> SearchUsersAsync(string prefix)
        {
            return await _context.Users.Where(x => x.FullName.Contains(prefix)).Take(10).ToListAsync();
        }

        public IQueryable<AppUser> GetQueryable()
        {
            return _context.Users.AsQueryable();
        }

        public async Task<AppUser?> UpdateAsync(AppUser user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (existingUser == null)
                return null;

            _context.Users.Update(user);
            return user;
        }
    }
}
