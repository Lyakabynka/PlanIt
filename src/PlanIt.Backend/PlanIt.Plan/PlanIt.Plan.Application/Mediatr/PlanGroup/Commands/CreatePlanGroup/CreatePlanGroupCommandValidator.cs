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
                RuleFor(c => c.Name)
                    .MustAsync(async (name, cancellationToken) =>
                    {
                        return !await dbContext.PlanGroups.Where(pg => pg.Name == name).AnyAsync(cancellationToken);
                    })
                    .WithMessage("PlanGroup with given name already exists");
            });
    }
}