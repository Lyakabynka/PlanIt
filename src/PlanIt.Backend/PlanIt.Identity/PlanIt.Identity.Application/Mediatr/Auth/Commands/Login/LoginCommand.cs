using PlanIt.Identity.Application.Features.Interfaces;
using PlanIt.Identity.Application.Response;

namespace PlanIt.Identity.Application.Mediatr.Auth.Commands.Login;

public class LoginCommand : IValidatableRequest<Result>
{
    public string Username { get; set; }
    public string Password { get; set; }
}