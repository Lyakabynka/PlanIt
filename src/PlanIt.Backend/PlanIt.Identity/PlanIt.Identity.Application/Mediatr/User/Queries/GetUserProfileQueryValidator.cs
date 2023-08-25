using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PlanIt.Identity.Application.Features.Interfaces;

namespace PlanIt.Identity.Application.Mediatr.User.Queries;

public class GetUserProfileQueryValidator : AbstractValidator<GetUserProfileQuery>
{
    public GetUserProfileQueryValidator(IApplicationDbContext dbContext)
    {
        RuleFor(query => query.UserId)
            .NotEqual(Guid.Empty)
            .MustAsync(async (userId, cancellationToken) => 
                await dbContext.Users.Where(user=>user.Id == userId).AnyAsync(cancellationToken))
            .WithMessage("User does not exist");
    }
}