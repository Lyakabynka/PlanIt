using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Commands.RemovePlanFromPlanGroup;

public class RemovePlanFromPlanGroupCommandValidator : AbstractValidator<RemovePlanFromPlanGroupCommand>
{
    public RemovePlanFromPlanGroupCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(c => c.PlanId)
            .NotEqual(Guid.Empty)
            .DependentRules(() =>
            {
                RuleFor(c => c.PlanId)
                    .MustAsync(async (planId, cancellationToken) =>
                    {
                        return await dbContext.Plans.Where(p => p.Id == planId).AnyAsync(cancellationToken);
                    })
                    .WithMessage("Plan does not exist");
            });
        RuleFor(c => c.PlanGroupId)
            .NotEqual(Guid.Empty)
            .DependentRules(() =>
            {
                RuleFor(c => c.PlanGroupId)
                    .MustAsync(async (planGroupId, cancellationToken) =>
                    {
                        return await dbContext.PlanGroups.Where(pg => pg.Id == planGroupId).AnyAsync(cancellationToken);
                    })
                    .WithMessage("PlanGroup does not exist");
            });
        RuleFor(c => c)
            .MustAsync(async (c, cancellationToken) =>
            {
                return await dbContext.PlanPlanGroups
                    .Where(ppg => ppg.PlanId == c.PlanId && ppg.PlanGroupId == c.PlanGroupId)
                    .AnyAsync(cancellationToken);
            })
            .WithMessage("PlanPlanGroup does not exist");

        RuleFor(c => c.UserId)
            .NotEqual(Guid.Empty);
    }
}