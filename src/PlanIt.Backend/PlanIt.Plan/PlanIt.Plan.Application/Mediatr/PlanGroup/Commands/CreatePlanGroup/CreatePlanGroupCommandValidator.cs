using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Commands.CreatePlanGroup;

public class CreatePlanGroupCommandValidator : AbstractValidator<CreatePlanGroupCommand>
{
    public CreatePlanGroupCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(c => c.Name)
            .Length(4, 40)
            .DependentRules(() =>
            {
                RuleFor(c => c)
                    .MustAsync(async (c, cancellationToken) =>
                    {
                        return !await dbContext.PlanGroups
                            .Where(pg => pg.UserId == c.UserId && pg.Name == c.Name)
                            .AnyAsync(cancellationToken);
                    })
                    .WithMessage("PlanGroup already exists")
                    .WithName("Name");
            });
    }
}