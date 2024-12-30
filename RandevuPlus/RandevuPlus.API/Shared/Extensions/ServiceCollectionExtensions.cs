using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RandevuPlus.API.Shared.Models;
using System.Text;
using FluentValidation;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RandevuPlus.API.Infrastructure.Data;
using RandevuPlus.API.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Infrastructure.Data.Seeders;

namespace RandevuPlus.API.Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureJwtBearer(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration
                .GetRequiredSection("Jwt")
                .Get<JwtOptions>(binderOptions => binderOptions.BindNonPublicProperties = true);

            services.AddSingleton(jwtOptions);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtBearerOptions =>
                {
                    var signingKeyBytes = Encoding.UTF8.GetBytes(jwtOptions.SigningKey);

                    jwtBearerOptions.SaveToken = true;
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
                    };
                });

            services.AddAuthorization();
        }
        public static void AddFeatures(this IServiceCollection services)
        {
            var assemblyToScan = typeof(Program).Assembly;
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assemblyToScan));
            services.AddValidatorsFromAssembly(assemblyToScan);
            services.AddSingleton<IMapper>(new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(assemblyToScan))));
        }

        public static void AddDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

            services.AddIdentity<AppUser, IdentityRole<Guid>>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        }

        public static async Task InitializeDbContextAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.MigrateAsync();
            await AppDbSeeder.SeedAsync(dbContext);
        }
    }
}
