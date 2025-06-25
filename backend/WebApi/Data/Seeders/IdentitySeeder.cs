using Microsoft.AspNetCore.Identity;

namespace WebApi.Data.Seeders;

public static class IdentitySeeder
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roles = new List<string>
        {
            "Admin"
        };

        foreach (var role in roles)
        {
            await CreateRoleAsync(roleManager, role);
        }
    }
    
    public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var users = new List<string>
        {
            "admin@admin.com",
            "test1@test.com",
            "test2@test.com",
            "test3@test.com",
            "test4@test.com",
            "test5@test.com",
            "test6@test.com",
            "test7@test.com",
            "test8@test.com",
            "test9@test.com"
        };
        
        foreach (var user in users)
        {
            await CreateUserAsync(userManager, user);      
        }
    }
    
    private static async Task CreateUserAsync(UserManager<IdentityUser> userManager, string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        
        if (user == null)
        {
            user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };
        
            var result = await userManager.CreateAsync(user, "Password123.");
            if (result.Succeeded && email == "admin@admin.com")
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }   
    }

    private static async Task CreateRoleAsync(RoleManager<IdentityRole> roleManager, string role)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

}