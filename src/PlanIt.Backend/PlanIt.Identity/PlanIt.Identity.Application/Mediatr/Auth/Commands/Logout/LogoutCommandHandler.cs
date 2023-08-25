using System.Runtime.InteropServices.JavaScript;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PlanIt.Identity.Application.Features.Interfaces;
using PlanIt.Identity.Application.Response;
using PlanIt.Identity.Application.Services;

namespace PlanIt.Identity.Application.Mediatr.Auth.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly JwtProvider _jwtProvider;
    private readonly HttpContext _context;
    private readonly CookieProvider _cookieProvider;

    public LogoutCommandHandler(
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

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = _cookieProvider.GetRefreshTokenFromCookie(_context.Request);
        if (refreshToken is null)
        {
            return Result.FormUnauthorized();
        }

        var existingSession = await _dbContext.RefreshSessions
            .FirstOrDefaultAsync(session => session.RefreshToken == refreshToken, cancellationToken);
        if (existingSession is null)
        {
            return Result.FormUnauthorized();
        }

        //delete refresh from database
        _dbContext.RefreshSessions.Remove(existingSession);

        await _dbContext.SaveChangesAsync(cancellationToken);

        //delete from cookies
        _cookieProvider.DeleteJwtTokenFromCookies(_context.Response);
        _cookieProvider.DeleteRefreshTokenFromCookies(_context.Response);

        return Result.Create(new { });
    }
}