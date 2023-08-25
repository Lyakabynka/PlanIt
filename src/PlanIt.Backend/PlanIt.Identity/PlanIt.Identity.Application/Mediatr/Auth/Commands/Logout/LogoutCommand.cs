using MediatR;
using PlanIt.Identity.Application.Response;

namespace PlanIt.Identity.Application.Mediatr.Auth.Commands.Logout;

public class LogoutCommand : IRequest<Result>
{
}