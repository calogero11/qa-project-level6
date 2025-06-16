using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using webapi.Interfaces;
using webapi.Models.Responses;

namespace webapi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController: ControllerBase
{
    private readonly IIdentityService identityService;
    private readonly IAuthenticationService authenticationService;
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;
    
    public AuthController(
        IIdentityService identityService,
        IAuthenticationService authenticationService,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        this.identityService = identityService;
        this.authenticationService = authenticationService;
        this.userManager = userManager;
        this.signInManager = signInManager;
    }
    
    [Authorize, HttpGet, Route("check")]
    public async Task<IActionResult> Get()
    {
        var authInfo = await identityService.GetAuthInfoAsync();
        
        return Ok(authInfo);
    }
    
    [HttpPost, Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var user = await userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null)
        {
            return Unauthorized();
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);
        if (!result.Succeeded)
        {
            return Unauthorized();
        }

        var roles = await  userManager.GetRolesAsync(user);
        var jwt = authenticationService.GenerateToken(user, roles.ToList());
        
        return Ok(new LoginResponse { AccessToken = jwt });
    }
}