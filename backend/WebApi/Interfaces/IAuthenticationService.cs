using Microsoft.AspNetCore.Identity;

namespace WebApi.Interfaces;

public interface IAuthenticationService
{
    public string GenerateToken(IdentityUser user, List<string> roles);
}