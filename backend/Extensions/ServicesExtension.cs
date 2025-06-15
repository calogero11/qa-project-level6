using webapi.Services;
using webapi.Interfaces;

namespace webapi.Extensions;

public static class ServicesExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IFeedService, FeedService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
    }
}