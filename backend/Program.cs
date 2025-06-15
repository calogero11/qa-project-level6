using Microsoft.AspNetCore.Identity;
using webapi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using webapi.Data.Seeders;
using webapi.Exceptions;
using webapi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureCors();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Web Api",
        Version = "v1"
    });
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token in the text input below."
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });
});

var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                              ?? throw new ConfigurationException("DefaultConnection");

builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseSqlite(defaultConnectionString));

builder.AddAuth();

builder.Services.AddHttpContextAccessor();

builder.Services.AddServices();

var app = builder.Build();

app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapIdentityApi<IdentityUser>();
app.DisableIdentityLoginEndpoint();
app.MapControllers();

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