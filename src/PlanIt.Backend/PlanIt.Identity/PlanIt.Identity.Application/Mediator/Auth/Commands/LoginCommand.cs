using BCrypt.Net;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OneOf;
using PlanIt.Identity.Application.Abstractions.Interfaces;
using PlanIt.Identity.Application.Abstractions.Validation;
using PlanIt.Identity.Application.Mediator.Results;
using PlanIt.Identity.Application.Mediator.Results.Shared;
using PlanIt.Identity.Application.Services;
using PlanIt.Identity.Domain.Entities;

namespace PlanIt.Identity.Application.Mediator.Auth.Commands;

public class LoginCommand : IRequest<OneOf<Success<UserVm>, Unauthorized>>
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(c => c.Username)
            .Length(4, 20);

        RuleFor(c => c.Password)
            .Length(8, 40);

        RuleFor(c => c)
            .MustAsync(async (c, cancellationToken) =>
            {
                var userCredentials = await dbContext.Users
                    .Where(user => user.Username == c.Username)
                    .Select(user => new
                    {
                        user.Username,
                        user.PasswordHash
                    })
                    .FirstOrDefaultAsync(cancellationToken);
                return userCredentials is not null &&
                       BCrypt.Net.BCrypt.EnhancedVerify(c.Password, userCredentials.PasswordHash, HashType.SHA512);
            })
            .WithMessage("Invalid username or/and password.")
            .WithName("Authorization");
    }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, OneOf<Success<UserVm>, Unauthorized>>
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
    
    public async Task<OneOf<Success<UserVm>, Unauthorized>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(user => user.Username == request.Username, cancellationToken);
        if (user is null ||
            !BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.PasswordHash, HashType.SHA512))
        {
            return new Unauthorized(
                new Error("Username", "Incorrect username or/and password"));
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
        _cookieProvider.AddRefreshCookieToResponse(_context.Response, existingSession.RefreshToken.ToString());

        return new Success<UserVm>(new UserVm()
        {
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            IsEmailConfirmed = user.IsEmailConfirmed
        });
    }
}