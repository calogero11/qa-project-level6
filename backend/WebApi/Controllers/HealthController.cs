using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
public class HealthController: ControllerBase
{
    [HttpGet, Route("health")]
    public string Get()
    {
        return "Healthy Server :)";
    }
}