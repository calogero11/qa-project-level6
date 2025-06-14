using Microsoft.AspNetCore.Identity;
using webapi.Data;
using webapi.Services;
using webapi.Interfaces;

namespace webapi.Extensions;

public static class ServicesExtension
{
    public static void AddAuth(this IServiceCollection services)
    {
        services
            .AddIdentityApiEndpoints<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddDefaultTokenProviders();

        services.AddAuthorization();
    }
    
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IFeedService, FeedService>();
        services.AddScoped<IIdentityService, IdentityService>();
    }
}