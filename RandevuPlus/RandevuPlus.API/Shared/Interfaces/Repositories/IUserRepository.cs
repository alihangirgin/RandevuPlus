using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.Shared.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<AppUser> AddAsync(AppUser user);
        Task<bool> CheckAsync(Guid id);
        Task<List<AppUser>> SearchUsersAsync(string prefix);
    }
}
