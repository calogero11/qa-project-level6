using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Interfaces;

namespace webapi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController: ControllerBase
{
    private readonly IIdentityService identityService;
    
    public AuthController(IIdentityService identityService)
    {
        this.identityService = identityService;
    }
    
    [Authorize, HttpGet, Route("check")]
    public async Task<IActionResult> Get()
    {
        var authInfo = await identityService.GetAuthInfoAsync();
        
        return Ok(authInfo);
    }
}