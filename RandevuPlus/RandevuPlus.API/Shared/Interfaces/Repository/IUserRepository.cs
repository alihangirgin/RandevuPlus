using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.Shared.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task<AppUser> AddAsync(AppUser user);
    }
}
