using Microsoft.AspNetCore.Identity;

namespace webapi.Interfaces;

public interface IAuthenticationService
{
    public string GenerateToken(IdentityUser user, List<string> roles);
}