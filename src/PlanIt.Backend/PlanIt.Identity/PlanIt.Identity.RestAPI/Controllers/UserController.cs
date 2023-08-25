using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlanIt.Identity.Application.Mediatr.User.Commands;
using PlanIt.Identity.Application.Mediatr.User.Queries;
using PlanIt.Identity.RestAPI.Models;

namespace PlanIt.Identity.RestAPI.Controllers;

[Route("user")]
public class UserController : ApiControllerBase
{
    /// <summary>
    /// Register a User
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// POST /user/register
    /// </remarks>
    /// <param name="requestModel">RegisterRequestModel with necessary fields</param>
    /// <response code="200">Success</response>
    /// <response code="409">User with provided credentials already exists</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="400">Invalid parameters</response>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestModel requestModel)
    {
        var request = new RegisterCommand()
        {
            Username = requestModel.Username,
            Password = requestModel.Password,
            Email = requestModel.Email,
        };
        
        return await Mediator.Send(request);
    }

    /// <summary>
    /// Get the User profile
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// POST /user/profile
    /// </remarks>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="400">Invalid parameters</response>
    [Authorize]
    [HttpGet("profile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUsersProfile()
    {
        var request = new GetUserProfileQuery();

        return await Mediator.Send(request);
    }
}