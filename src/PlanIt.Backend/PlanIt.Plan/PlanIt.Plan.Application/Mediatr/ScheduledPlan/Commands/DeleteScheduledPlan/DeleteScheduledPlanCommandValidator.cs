using FluentValidation;

namespace PlanIt.Plan.Application.Mediatr.ScheduledPlan.Commands.DeleteScheduledPlan;

public class DeleteScheduledPlanCommandValidator : AbstractValidator<DeleteScheduledPlanCommand>
{
    public DeleteScheduledPlanCommandValidator()
    {
        RuleFor(command => command.ScheduledPlanId)
            .NotEqual(Guid.Empty);
        RuleFor(command => command.UserId)
            .NotEqual(Guid.Empty);
    }
}