using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeyCloak.WebAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok("This api Running");
    }
}
