using webapi.Exceptions;

namespace webapi.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
                else
                {
                    policy.WithOrigins(builder.Configuration["AllowedFrontendUrl"] ?? throw new ConfigurationException("AllowedFrontendUrl"))
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
            });
        });
    }
}