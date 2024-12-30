using Microsoft.AspNetCore.Identity;
using RandevuPlus.API.Shared.Constants;
using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.Infrastructure.Data.Seeders
{
    public class AppDbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.Add(new AppUser
                {
                    Id = new Guid(TestUserConstants.TestUserId),
                    UserName = TestUserConstants.TestUserUsername,
                    Email = TestUserConstants.TestUserEmail,
                    NormalizedUserName = TestUserConstants.TestUserUsername.ToUpper(),
                    NormalizedEmail = TestUserConstants.TestUserEmail.ToUpper(),
                    EmailConfirmed = true,
                    PasswordHash = new PasswordHasher<AppUser>().HashPassword(null, TestUserConstants.TestUserPassword),
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
