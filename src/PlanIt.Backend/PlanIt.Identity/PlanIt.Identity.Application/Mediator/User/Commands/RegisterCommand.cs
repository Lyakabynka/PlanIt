using BCrypt.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using PlanIt.Identity.Application.Abstractions.Interfaces;
using PlanIt.Identity.Application.Mediator.Results;
using PlanIt.Identity.Domain.Entities;

namespace PlanIt.Identity.Application.Mediator.User.Commands;

public class RegisterCommand : IRequest<OneOf<Success, Collision>>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, OneOf<Success, Collision>>
{
    private readonly IApplicationDbContext _dbContext;

    public RegisterCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<OneOf<Success, Collision>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _dbContext.Users.AnyAsync(u=>u.Username == request.Username, cancellationToken))
        {
            return new Collision(
                new Error("Username", "User with given username already exists"));
        }
        
        if (await _dbContext.Users.AnyAsync(u=>u.Email == request.Email, cancellationToken))
        {
            return new Collision(
                new Error("Email", "User with given email already exists"));
        }

        var user = new Domain.Entities.User()
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password, HashType.SHA512),
        };

        await _dbContext.Users.AddAsync(user, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}

