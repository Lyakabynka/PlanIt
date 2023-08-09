using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;

namespace PlanIt.Identity.Application.Mediator.Results;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct NotFound
{
}

public record Error(string ErrorField, string ErrorMessage);

public struct BadRequest
{
    public ProblemDetails Error { get; set; }

    public BadRequest(ProblemDetails error) =>
        (Error) = (error);
}

public struct Unauthorized
{
    public Error Error { get; set; }

    public Unauthorized(Error error) =>
        (Error) = (error);
}

public struct Collision
{
    public Error Error { get; set; }

    public Collision(Error error) =>
        (Error) = (error);
}