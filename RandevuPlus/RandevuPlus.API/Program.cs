using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Infrastructure.Services;
using RandevuPlus.API.Infrastructure.UnitOfWork;
using RandevuPlus.API.Shared.Extensions;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IUserService, UserService>();

var connectionString = builder.Configuration.GetConnectionString("RandevuPlusDb");
builder.Services.AddDbContext(connectionString);
builder.Services.ConfigureJwtBearer(builder.Configuration);
builder.Services.AddFeatures();

var app = builder.Build();

await app.InitializeDbContextAsync();   


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
