using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Dtos;
using RandevuPlus.API.Shared.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RandevuPlus.API.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GenerateJwtTokenDto> GenerateJwtTokenAsync(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.UTF8.GetBytes("81beca0d490431b65b22bfa2d86a2495d2492235daee928cb8d784ce751b42a8");
            var expiresIn = DateTime.UtcNow.AddHours(1);

            var roles = await _userManager.GetRolesAsync(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            }
                .Concat(roles.Select(role => new Claim(ClaimTypes.Role, role)))),
                Expires = expiresIn,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new GenerateJwtTokenDto(tokenHandler.WriteToken(token), expiresIn);
        }
    }
}
