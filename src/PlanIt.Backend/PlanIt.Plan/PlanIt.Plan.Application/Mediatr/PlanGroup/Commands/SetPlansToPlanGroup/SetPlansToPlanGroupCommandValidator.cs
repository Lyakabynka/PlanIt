using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Commands.SetPlansToPlanGroup;

public class SetPlansToPlanGroupCommandValidator : AbstractValidator<SetPlansToPlanGroupCommand>
{
    public SetPlansToPlanGroupCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor((c => c.SetPlanToPlanGroupRequestModels))
            .NotNull()
            .DependentRules(() =>
            {
                RuleFor(c => c.SetPlanToPlanGroupRequestModels)
                    .Cascade(CascadeMode.Stop)
                    .MustAsync(async (setPlanToPlanGroupRequestModels, cancellationToken) =>
                    {
                        var hasDuplicates = setPlanToPlanGroupRequestModels
                            .GroupBy(m => m.Index)
                            .Any(m => m.Count() > 1);

                        return !hasDuplicates;
                    })
                    .WithMessage("Index must be unique")
                    .MustAsync(async (setPlanToPlanGroupRequestModels, cancellationToken) =>
                    {
                        var planIds = setPlanToPlanGroupRequestModels
                            .Select(rm => rm.PlanId)
                            .Distinct()
                            .ToList();

                        var allPlansExist =
                            planIds.Count == await dbContext.Plans
                                .Where(plan => planIds.Contains(plan.Id))
                                .CountAsync(cancellationToken);

                        return allPlansExist;
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
                        return await dbContext.PlanGroups
                            .Where(pg => pg.Id == planGroupId)
                            .AnyAsync(cancellationToken);
                    })
                    .WithMessage("PlanGroup does not exist");
            });

        RuleFor(c => c.UserId)
            .NotEqual(Guid.Empty);
    }
}