namespace webapi.Extensions;

public static class WebApplicationExtensions
{
    public static void DisableIdentityLoginEndpoint(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            if (context.Request.Path.Equals("/login", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Endpoint disabled");
                return;
            }
            await next();
        });
    }
}