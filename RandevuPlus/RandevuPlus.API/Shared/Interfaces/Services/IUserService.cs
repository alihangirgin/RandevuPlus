using Microsoft.AspNetCore.Identity;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Dtos;
using RandevuPlus.API.Shared.Enums;

namespace RandevuPlus.API.Shared.Interfaces.Services
{
    public interface IUserService
    {
        Task<GenerateJwtTokenDto> GenerateJwtTokenAsync(AppUser user);
        List<string> GetOnlineUsers();
        UserStatus GetUserStatus(Guid userId);
    }
}
