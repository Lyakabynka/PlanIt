using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PlanIt.Plan.RestAPI.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    private IMediator _mediator;

    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    protected internal Guid UserId => User.Identity.IsAuthenticated
        ? Guid.Parse(User.FindFirstValue("userId")!)
        : Guid.Empty;
}