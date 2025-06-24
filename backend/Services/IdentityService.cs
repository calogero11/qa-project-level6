using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using webapi.Interfaces;
using webapi.Models.Responses;

namespace webapi.Services;

public class IdentityService: IIdentityService
{
    private readonly ClaimsPrincipal claimsPrincipal;
    private readonly UserManager<IdentityUser> userManager;
   
    public IdentityService(IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
    {
         claimsPrincipal = httpContextAccessor.HttpContext?.User ??
                          throw new UnauthorizedAccessException();

        this.userManager = userManager;
    }

    public async Task<AuthInfo> GetAuthInfoAsync() =>
        new()
        {
            UserGuid = await GetUserGuidAsync(),
            Email = await GetUserEmailAsync(),
            Roles = await GetUserRolesAsync()
        };
    
    public async Task<Guid> GetUserGuidAsync()
    {
        var user =  await userManager.GetUserAsync(claimsPrincipal);
        
        if (user is null || !Guid.TryParse(user.Id, out var userGuid))
        {
            throw new UnauthorizedAccessException();
        }
                
        return userGuid;
    }

    public async Task<string> GetUserNameAsync()
    {
        var user =  await userManager.GetUserAsync(claimsPrincipal);
        
        if (user?.UserName is null)
        {
            throw new UnauthorizedAccessException();
        }
            
        return user.UserName;
    }

    private async Task<string> GetUserEmailAsync()
    {
        var user =  await userManager.GetUserAsync(claimsPrincipal);
        
        if (user?.Email is null)
        {
            throw new UnauthorizedAccessException();
        }

        return user.Email;
    }

    public async Task<List<string>> GetUserRolesAsync()
    {
        var user = await userManager.GetUserAsync(claimsPrincipal);
        
        if (user is null)
        {
            throw new UnauthorizedAccessException();
        }
        
        var roles = await userManager.GetRolesAsync(user);
        
        return roles.ToList();
    }
}