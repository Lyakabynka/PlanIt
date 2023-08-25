using PlanIt.Identity.Application.Features.Interfaces;
using PlanIt.Identity.Application.Response;

namespace PlanIt.Identity.Application.Mediatr.User.Commands;

public class RegisterCommand : IValidatableRequest<Result>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}