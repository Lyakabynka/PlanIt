using BCrypt.Net;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OneOf;
using PlanIt.Identity.Application.Interfaces;
using PlanIt.Identity.Application.Mediator.Results;
using PlanIt.Identity.Application.Services;
using PlanIt.Identity.Domain.Entities;

namespace PlanIt.Identity.Application.Mediator.Auth.Commands;

public class LoginCommand : IRequest<OneOf<Success, InvalidCredentials>>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, OneOf<Success, InvalidCredentials>>
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
    
    public async Task<OneOf<Success, InvalidCredentials>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.UserName == request.UserName, cancellationToken);
        if (user is null ||
            !BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.PasswordHash, HashType.SHA512))
        {
            return new InvalidCredentials(
                new Error("UserName", "Incorrect username or/and password"));
        }

        //finding existing session by user id
        var existingSession = await _dbContext.RefreshSessions
            .AsTracking()
            .FirstOrDefaultAsync(
                session => session.UserId == user.Id,
                cancellationToken);
        //if session does not exist - create new session
        if (existingSession is null)
        {
            existingSession = new RefreshSession()
            {
                UserId = user.Id,
                RefreshToken = Guid.NewGuid()
            };
            _dbContext.RefreshSessions.Add(existingSession);
        }
        //if session exists - update existing one
        else
        {
            existingSession.RefreshToken = Guid.NewGuid();
            existingSession.UpdatedAt = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        var jwtToken = _jwtProvider.CreateToken(user);
        
        _cookieProvider.AddJwtCookieToResponse(_context.Response, jwtToken);
        _cookieProvider.AddRefreshCookieToResponse(_context.Response,existingSession.RefreshToken.ToString());

        return new Success();
    }
}


