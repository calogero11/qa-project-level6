using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using webapi.Interfaces;
using webapi.Models.Responses;

namespace webapi.Services;

public class IdentityService: IIdentityService
{
    private readonly ClaimsPrincipal claimsPrincipal;
    private readonly UserManager<IdentityUser> userManager;
    private readonly IdentityUser user;

    public IdentityService(IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
    {
        claimsPrincipal = httpContextAccessor.HttpContext?.User ??
                          throw new UnauthorizedAccessException();

        user = userManager.GetUserAsync(claimsPrincipal).Result 
            ?? throw new UnauthorizedAccessException();

        this.userManager = userManager;
    }

    public async Task<AuthInfo> GetAuthInfoAsync() =>
        new()
        {
            UserGuid = await GetUserGuid(),
            Email = await GetUserEmail(),
            Roles = []
        };
    

    public async Task<Guid> GetUserGuid()
    {
        var userGuidString = await userManager.GetUserIdAsync(user);
        
        if (!Guid.TryParse(userGuidString, out var userGuid))
        {
            throw new UnauthorizedAccessException();
        }
                
        return userGuid;
    }

    public async Task<string> GetUserName()
    {
        var userName = await userManager.GetUserNameAsync(user);
        
        if (userName is null)
        {
            throw new UnauthorizedAccessException();
        }
            
        return userName;
    }

    private async Task<string> GetUserEmail()
    {
        var email = await userManager.GetEmailAsync(user);
        
        if (email is null)
        {
            throw new UnauthorizedAccessException();
        }

        return email;
    }
}