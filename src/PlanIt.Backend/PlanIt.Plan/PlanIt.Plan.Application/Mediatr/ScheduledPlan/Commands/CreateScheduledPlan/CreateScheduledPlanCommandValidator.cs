using FluentValidation;

namespace PlanIt.Plan.Application.Mediatr.ScheduledPlan.Commands.CreateScheduledPlan;

public class CreateScheduledPlanCommandValidator : AbstractValidator<CreateScheduledPlanCommand>
{
    public CreateScheduledPlanCommandValidator()
    {
        RuleFor(command => command.PlanId)
            .NotEqual(Guid.Empty);
        RuleFor(command => command.UserId)
            .NotEqual(Guid.Empty);
    }
}