using BCrypt.Net;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PlanIt.Identity.Application.Features.Interfaces;

namespace PlanIt.Identity.Application.Mediatr.Auth.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(c => c.Username)
            .Length(4, 20)
            .DependentRules(() =>
            {
                RuleFor(c => c.Password)
                    .Length(8, 40)
                    .DependentRules(() =>
                    {
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
                            .WithMessage("Invalid username or/and password")
                            .WithName("Authorization");
                    });
            });
    }
}
