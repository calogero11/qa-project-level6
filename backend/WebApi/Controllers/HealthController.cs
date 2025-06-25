using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
public class HealthController: ControllerBase
{
    /// <summary>
    /// Returns the health status of the server
    /// </summary>
    /// <returns>A simple string indicating server health</returns>
    /// <response code="200">Server is healthy</response>
    [HttpGet, Route("health")]
    public string Get()
    {
        return "Healthy Server :)";
    }
}