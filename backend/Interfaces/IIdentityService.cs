using webapi.Models.Responses;

namespace webapi.Interfaces;

public interface IIdentityService
{
    Task<AuthInfo> GetAuthInfoAsync();
    
    Task<Guid> GetUserGuid();

    Task<string> GetUserName();
}