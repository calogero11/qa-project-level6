using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
using WebApi.Models.Responses;

namespace WebApi.Controllers;

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
    
    /// <summary>
    /// Retrieves authentication information for the current user
    /// </summary>
    /// <remarks> Requires authorization </remarks>
    /// <returns>Returns the authentication details</returns>
    /// <response code="200">Authentication info retrieved successfully</response>
    /// <response code="401">Unauthorized access</response>
    [Authorize, HttpGet, Route("check")]
    public async Task<IActionResult> Get()
    {
        var authInfo = await identityService.GetAuthInfoAsync();
        
        return Ok(authInfo);
    }
    
    /// <summary>
    /// Authenticates a user and returns a JWT access token
    /// </summary>
    /// <param name="loginRequest">The user login credentials (email and password)</param>
    /// <returns>Returns an access token if authentication succeeds</returns>
    /// <response code="200">Authentication successful. Returns JWT token</response>
    /// <response code="401">Authentication failed. Invalid email or password</response>
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