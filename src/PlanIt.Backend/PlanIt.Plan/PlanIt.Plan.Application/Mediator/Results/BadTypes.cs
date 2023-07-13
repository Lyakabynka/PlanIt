using System.Runtime.InteropServices;

namespace PlanIt.Plan.Application.Mediator.Results;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct NotFound { }

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct Forbidden { }

public record Error(string ReasonField, string Message);

public struct InvalidCredentials
{
    public Error Error { get; set; }
    public InvalidCredentials(Error error) =>
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

public struct BadRequest
{
    public Error Error { get; set; }
    public BadRequest(Error error) =>
        (Error) = (error);
}