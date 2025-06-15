using webapi.Models.Responses;

namespace webapi.Interfaces;

public interface IIdentityService
{
    Task<AuthInfo> GetAuthInfoAsync();
    
    Task<Guid> GetUserGuidAsync();

    Task<string> GetUserNameAsync();
}