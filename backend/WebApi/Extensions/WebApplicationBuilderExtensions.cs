using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WebApi.Data;
using WebApi.Exceptions;
using WebApi.Models.Configuration;

namespace WebApi.Extensions;

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
    
    public static void AddAuth(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddIdentityApiEndpoints<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddDefaultTokenProviders();


        var jwtConfiguration = builder.Configuration.GetSection("jwtSettings");
        var jwtSettings = jwtConfiguration.Get<JwtSettings>() ?? throw new ConfigurationException("JwtSettings");

        builder.Services.Configure<JwtSettings>(jwtConfiguration);
        
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key!)),

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
    }
}