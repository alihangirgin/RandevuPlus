using Microsoft.AspNetCore.Identity;
using RandevuPlus.API.Shared.Dtos;

namespace RandevuPlus.API.Shared.Interfaces
{
    public interface IUserService
    {
        GenerateJwtTokenDto GenerateJwtToken(IdentityUser user);
    }
}
