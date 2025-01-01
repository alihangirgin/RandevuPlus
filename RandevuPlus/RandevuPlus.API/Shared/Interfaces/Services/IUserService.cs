using Microsoft.AspNetCore.Identity;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Dtos;

namespace RandevuPlus.API.Shared.Interfaces.Services
{
    public interface IUserService
    {
        Task<GenerateJwtTokenDto> GenerateJwtTokenAsync(AppUser user);
    }
}
