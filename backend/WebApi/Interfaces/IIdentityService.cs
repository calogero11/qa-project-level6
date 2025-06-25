using WebApi.Models.Responses;

namespace WebApi.Interfaces;

public interface IIdentityService
{
    Task<AuthInfo> GetAuthInfoAsync();
    
    Task<Guid> GetUserGuidAsync();

    Task<string> GetUserNameAsync();

    Task<List<string>> GetUserRolesAsync();

}