using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeyCloak.WebAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    [Authorize(Roles = "admin")]
    [HttpGet]
    public IActionResult AccesDenied()
    {
        return Ok("Access Denied");
    }
}
