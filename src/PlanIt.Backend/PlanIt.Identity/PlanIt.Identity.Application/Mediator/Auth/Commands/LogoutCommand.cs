using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OneOf;
using PlanIt.Identity.Application.Interfaces;
using PlanIt.Identity.Application.Mediator.Results;
using PlanIt.Identity.Application.Services;

namespace PlanIt.Identity.Application.Mediator.Auth.Commands;

public class LogoutCommand : IRequest<OneOf<Success,Unauthorized>> {}

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, OneOf<Success,Unauthorized>>
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
    
    public async Task<OneOf<Success,Unauthorized>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = _cookieProvider.GetRefreshTokenFromCookie(_context.Request);
        if (refreshToken is null)
        {
            return new Unauthorized(
                new Error("RefreshToken", "Unable to extract refresh token from cookies"));
        }

        var existingSession = await _dbContext.RefreshSessions
            .FirstOrDefaultAsync(session => session.RefreshToken == refreshToken, cancellationToken);
        if (existingSession is null)
        {
            return new Unauthorized(
                new Error("RefreshSession", "Refresh session was not found"));
        }

        //delete refresh from database
        _dbContext.RefreshSessions.Remove(existingSession);

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        //delete from cookies
        _cookieProvider.DeleteJwtTokenFromCookies(_context.Response);
        _cookieProvider.DeleteRefreshTokenFromCookies(_context.Response);
        
        return new Success();
    }
}
