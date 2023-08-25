using Microsoft.AspNetCore.Mvc;
using PlanIt.Identity.Application.Mediatr.Auth.Commands;
using PlanIt.Identity.Application.Mediatr.Auth.Commands.Login;
using PlanIt.Identity.Application.Mediatr.Auth.Commands.Logout;
using PlanIt.Identity.Application.Mediatr.Auth.Commands.Refresh;
using PlanIt.Identity.RestAPI.Models;

namespace PlanIt.Identity.RestAPI.Controllers;

[Route("auth")]
public class AuthController : ApiControllerBase
{
    /// <summary>
    /// Authorizes User
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// GET /auth/login
    /// </remarks>
    /// <param name="requestModel">LoginRequestModel with necessary fields</param>
    /// <response code="200">Success</response>
    /// <response code="401">Invalid username or/and password</response>
    /// <response code="400">Invalid parameters</response>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel requestModel)
    {
        //TODO: validation
        var request = new LoginCommand()
        {
            Username = requestModel.Username,
            Password = requestModel.Password
        };

        return await Mediator.Send(request);
    }

    /// <summary>
    /// Refreshes User's JWT Token
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// PUT /auth/refresh
    /// </remarks>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="400">Invalid parameters</response>
    [HttpPut("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Refresh()
    {
        var request = new RefreshCommand();
        
        return await Mediator.Send(request);
    }

    /// <summary>
    /// Log outs User
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// DELETE /auth/logout
    /// </remarks>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="400">Invalid parameters</response>
    [HttpDelete("logout")]
    public async Task<IActionResult> Logout()
    {
        var request = new LogoutCommand();

        return await Mediator.Send(request);
    }
}