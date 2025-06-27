using Microsoft.AspNetCore.Identity;
using WebApi.Data;
using Microsoft.EntityFrameworkCore;
using WebApi.Data.Seeders;
using WebApi.Exceptions;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureCors();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger();

var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                              ?? throw new ConfigurationException("DefaultConnection");

builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseSqlite(defaultConnectionString));

builder.AddAuth();

builder.Services.ConfigureRateLimiter();

builder.Services.AddHttpContextAccessor();

builder.Services.AddServices();

var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();
app.MapIdentityApi<IdentityUser>();
app.DisableIdentityLoginEndpoint();
app.MapControllers().RequireRateLimiting("ApiRateLimiter");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    var dbContext = services.GetRequiredService<DatabaseContext>();
    await dbContext.Database.MigrateAsync();

    await IdentitySeeder.SeedRolesAsync(services);
    await IdentitySeeder.SeedUsersAsync(services);
    await FeedSeeder.SeedFeedsAsync(dbContext);
}

app.Run();
