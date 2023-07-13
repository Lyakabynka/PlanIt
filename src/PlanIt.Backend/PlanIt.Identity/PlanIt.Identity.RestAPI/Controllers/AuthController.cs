using Microsoft.AspNetCore.Mvc;
using PlanIt.Identity.Application.Mediator.Auth.Commands;
using PlanIt.Identity.RestAPI.Models;

namespace PlanIt.Identity.RestAPI.Controllers;

[Route("auth")]
public class AuthController : ApiControllerBase
{
    [HttpGet("login")]
    public async Task<IActionResult> Login([FromQuery] LoginRequestModel requestModel)
    {
        //TODO: validation
        var request = new LoginCommand()
        {
            UserName = requestModel.UserName,
            Password = requestModel.Password
        };

        var result = await Mediator.Send(request);
        return result.Match<IActionResult>(
            success => Ok(),
            invalidCredentials => Unauthorized());
    }

    [HttpPut("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var request = new RefreshCommand();
        
        var result = await Mediator.Send(request);
        return result.Match<IActionResult>(
            success => Ok(),
            unauthorized => Unauthorized(unauthorized.Error));
    }

    [HttpDelete("logout")]
    public async Task<IActionResult> Logout()
    {
        var request = new LogoutCommand();

        var result = await Mediator.Send(request);
        return result.Match<IActionResult>(
            success => Ok(),
            unauthorized => Unauthorized(unauthorized.Error));
    }
}