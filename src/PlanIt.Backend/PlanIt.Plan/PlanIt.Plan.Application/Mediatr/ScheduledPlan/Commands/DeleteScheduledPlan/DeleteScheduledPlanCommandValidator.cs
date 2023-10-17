using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;

namespace PlanIt.Plan.Application.Mediatr.ScheduledPlan.Commands.DeleteScheduledPlan;

public class DeleteScheduledPlanCommandValidator : AbstractValidator<DeleteScheduledPlanCommand>
{
    public DeleteScheduledPlanCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(command => command.ScheduledPlanId)
            .NotEqual(Guid.Empty)
            .DependentRules(() =>
            {
                RuleFor(c => c.ScheduledPlanId)
                    .MustAsync(async (scheduledPlanId, cancellationToken) =>
                    {
                        return await dbContext.ScheduledPlans
                            .Where(p => p.Id == scheduledPlanId)
                            .AnyAsync(cancellationToken);
                    })
                    .WithMessage("ScheduledPlan does not exist");
            });
        RuleFor(command => command.UserId)
            .NotEqual(Guid.Empty);
    }
}