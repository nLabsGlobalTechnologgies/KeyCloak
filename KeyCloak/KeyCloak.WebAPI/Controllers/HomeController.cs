using KeyCloak.WebAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace KeyCloak.WebAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class HomeController(ILogger<HomeController> logger) : ControllerBase
{
    [HttpGet]
    public IActionResult Home()
    {
        return Ok("Home Page");
    }

    [HttpGet]
    [Authorize(Policy = "admins")]
    public IActionResult AuthenticationAdmin()
    {
        return Ok("Test auth for admin group works ...");
    }

    [HttpGet]
    [Authorize(Policy = "noaccess")]
    public IActionResult AuthenticationNoAccess()
    {
        //Test that your identity does not have this claim attaced
        return Ok("Test auth - you have done something wrong if you can access this site.");
    }

    [HttpGet]
    [Authorize(Policy = "users")]
    public async Task<IActionResult> AuthenticationAsync()
    {
        //Find claims for the current user
        ClaimsPrincipal currentUser = this.User;
        //Get username, for keycloak you need to regex this to get the clean username
        var currentUserName = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
        //logs an error so it's easier to find - thanks debug.
        logger.LogError(currentUserName);

        //Debug this line of code if you want to validate the content jwt.io
        string accessToken = await HttpContext.GetTokenAsync("access_token") ?? "";
        string idToken = await HttpContext.GetTokenAsync("id_token") ?? "";
        string refreshToken = await HttpContext.GetTokenAsync("refresh_token") ?? "";


        /*
         * Token exchange implementation
         * Uncomment section below
         */
        /*
        //Call a token exchange to call another service in keycloak
        //Remember to implement a logger with the default constructor for more visibility
        TokenExchange exchange = new TokenExchange();
        //Do a refresh token, if the service you need to call has a short lived token time
        var newAccessToken = await exchange.GetRefreshTokenAsync(refreshToken);
        var serviceAccessToken = await exchange.GetTokenExchangeAsync(newAccessToken);
        //Use the access token to call the service that exchanged the token
        //Example:
        // MyService myService = new MyService/();
        //var myService = await myService.GetDataAboutSomethingAsync(serviceAccessToken):
        */

        //Get all claims for roles that you have been granted access to 
        IEnumerable<Claim> roleClaims = User.FindAll(ClaimTypes.Role);
        IEnumerable<string> roles = roleClaims.Select(r => r.Value);
        foreach (var role in roles)
        {
            logger.LogError(role);
        }

        //Another way to display all role claims
        var currentClaims = currentUser.FindAll(ClaimTypes.Role).ToList();
        foreach (var claim in currentClaims)
        {
            logger.LogError(claim.ToString());
        }

        return Ok("Test auth works for members in users...");

    }

    [HttpGet]
    public IActionResult Privacy()
    {
        return Ok("Use this page to detail your site's privacy policy.");
    }

    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return Ok(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
