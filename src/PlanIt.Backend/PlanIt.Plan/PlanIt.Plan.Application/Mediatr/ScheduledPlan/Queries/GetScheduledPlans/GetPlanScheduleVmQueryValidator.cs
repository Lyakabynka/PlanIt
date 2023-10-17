using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;

namespace PlanIt.Plan.Application.Mediatr.ScheduledPlan.Queries.GetScheduledPlans;

public class GetPlanScheduleVmQueryValidator : AbstractValidator<GetPlanScheduleVmQuery>
{
    public GetPlanScheduleVmQueryValidator(IApplicationDbContext dbContext)
    {
        RuleFor(q => q.PlanId)
            .NotEqual(Guid.Empty)
            .DependentRules(() =>
            {
                RuleFor(c => c.PlanId)
                    .MustAsync(async (planId, cancellationToken) =>
                    {
                        return await dbContext.Plans
                            .Where(p => p.Id == planId)
                            .AnyAsync(cancellationToken);
                    })
                    .WithMessage("Plan does not exist");
            });;
        RuleFor(q => q.UserId)
            .NotEqual(Guid.Empty);
    }
}