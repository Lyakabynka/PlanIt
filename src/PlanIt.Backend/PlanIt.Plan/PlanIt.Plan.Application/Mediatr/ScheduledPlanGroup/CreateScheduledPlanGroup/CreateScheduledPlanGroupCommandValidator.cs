using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;

namespace PlanIt.Plan.Application.Mediatr.ScheduledPlanGroup.CreateScheduledPlanGroup;

public class CreateScheduledPlanGroupCommandValidator: AbstractValidator<CreateScheduledPlanGroupCommand>
{
    public CreateScheduledPlanGroupCommandValidator(IApplicationDbContext dbContext)
    {
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
        
        
    }
}