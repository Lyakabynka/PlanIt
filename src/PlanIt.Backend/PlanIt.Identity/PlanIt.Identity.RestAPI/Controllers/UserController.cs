using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlanIt.Identity.Application.Mediator.User.Commands;
using PlanIt.Identity.RestAPI.Models;

namespace PlanIt.Identity.RestAPI.Controllers;

[Route("user")]
public class UserController : ApiControllerBase
{
    [HttpPost("register")]
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