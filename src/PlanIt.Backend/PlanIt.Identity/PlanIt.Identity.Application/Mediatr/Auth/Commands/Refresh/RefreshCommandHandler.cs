using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PlanIt.Identity.Application.Features.Interfaces;
using PlanIt.Identity.Application.Response;
using PlanIt.Identity.Application.Services;

namespace PlanIt.Identity.Application.Mediatr.Auth.Commands.Refresh;

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly JwtProvider _jwtProvider;
    private readonly HttpContext _context;
    private readonly CookieProvider _cookieProvider;

    public RefreshCommandHandler(
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

    public async Task<Result> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = _cookieProvider.GetRefreshTokenFromCookie(_context.Request);
        if (refreshToken is null)
        {
            return Result.FormUnauthorized();
        }

        var existingSession = await _dbContext.RefreshSessions
            .AsTracking()
            .FirstOrDefaultAsync(session => session.RefreshToken == refreshToken, cancellationToken);
        if (existingSession is null)
        {
            return Result.FormUnauthorized();
        }

        existingSession.RefreshToken = Guid.NewGuid();
        existingSession.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == existingSession.UserId, cancellationToken);
        if (user is null)
        {
            return Result.FormUnauthorized();
        }

        var jwtToken = _jwtProvider.CreateToken(user);

        _cookieProvider.AddJwtCookieToResponse(_context.Response,
            jwtToken);
        _cookieProvider.AddRefreshCookieToResponse(_context.Response,
            existingSession.RefreshToken.ToString());

        return Result.Create(new { });
    }
}