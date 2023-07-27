using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlanIt.Identity.Application.Mediator.User.Commands;
using PlanIt.Identity.RestAPI.Models;

namespace PlanIt.Identity.RestAPI.Controllers;

[Route("user")]
public class UserController : ApiControllerBase
{
    /// <summary>
    /// Registers a User
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// POST /user/register
    /// </remarks>
    /// <response code="200">Success</response>
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
            UserName = requestModel.UserName,
            Password = requestModel.Password,
            Email = requestModel.Email,

            FirstName = requestModel.FirstName,
            LastName = requestModel.LastName,
            MiddleName = requestModel.MiddleName,
            Phone = requestModel.Phone,
            HomeAddress = requestModel.HomeAddress,

            BirthDate = requestModel.BirthDate
        };

        var result = await Mediator.Send(request);
        return result.Match<IActionResult>(
            success => Ok(),
            collision => Conflict(collision.Error));
    }
}