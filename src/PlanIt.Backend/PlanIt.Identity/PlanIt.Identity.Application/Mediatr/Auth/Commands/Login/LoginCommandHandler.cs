using BCrypt.Net;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PlanIt.Identity.Application.Features.Interfaces;
using PlanIt.Identity.Application.Mediatr.Results.Shared;
using PlanIt.Identity.Application.Response;
using PlanIt.Identity.Application.Services;
using PlanIt.Identity.Domain.Entities;

namespace PlanIt.Identity.Application.Mediatr.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly JwtProvider _jwtProvider;
    private readonly HttpContext _context;
    private readonly CookieProvider _cookieProvider;

    public LoginCommandHandler(
        IApplicationDbContext dbContext,
        JwtProvider jwtProvider,
        IHttpContextAccessor accessor,
        CookieProvider cookieProvider)
    {
        _dbContext = dbContext;
        _jwtProvider = jwtProvider;
        _context = accessor.HttpContext!;
        _cookieProvider = cookieProvider;
    }

    public async Task<Result> Handle(LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstAsync(user => user.Username == request.Username, cancellationToken);

        //determining user's platform
        var userAgent = _context
            .Request
            .Headers["User-Agent"]
            .ToString()
            .ToLower();

        var session = new RefreshSession()
        {
            //TODO: add expiration time and delete expired sessions
            UserId = user.Id,
            RefreshToken = Guid.NewGuid(),
            UserAgent = userAgent
        };

        _dbContext.RefreshSessions.Add(session);

        await _dbContext.SaveChangesAsync(cancellationToken);

        var jwtToken = _jwtProvider.CreateToken(user);

        _cookieProvider.AddJwtCookieToResponse(_context.Response, jwtToken);
        _cookieProvider.AddRefreshCookieToResponse(_context.Response, session.RefreshToken.ToString());

        return Result.Create(new UserVm()
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            IsEmailConfirmed = user.IsEmailConfirmed,
            //user's current agent
            UserAgent = session.UserAgent
        });
    }
}