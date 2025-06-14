using webapi.Services;
using webapi.Interfaces;

namespace webapi.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IFeedService, FeedService>();
        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}