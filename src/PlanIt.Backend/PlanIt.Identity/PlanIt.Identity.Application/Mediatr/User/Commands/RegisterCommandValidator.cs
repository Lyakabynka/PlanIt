using System.Net.Mail;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using PlanIt.Identity.Application.Features.Interfaces;

namespace PlanIt.Identity.Application.Mediatr.User.Commands;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(c => c.Username)
            .Length(4, 20);

        RuleFor(c => c.Password)
            .Length(8, 40);

        RuleFor(c => c.Email)
            .Length(8, 80)
            .Must((email) =>
            {
                try
                {
                    var mailAddress = new MailAddress(email);
                    return true;
                }
                catch
                {
                    return false;
                }
            });

        RuleFor(c => c.Username)
            .MustAsync(async (username, cancellationToken) =>
            {
                return !await dbContext.Users
                    .Where(user => user.Username == username)
                    .AnyAsync(cancellationToken);
            })
            .WithMessage("User with this username already exists");
        
        RuleFor(c => c.Email)
            .MustAsync(async (email, cancellationToken) =>
            {
                return !await dbContext.Users
                    .Where(user => user.Email == email)
                    .AnyAsync(cancellationToken);
            })
            .WithMessage("User with this email already exists");
    }
}