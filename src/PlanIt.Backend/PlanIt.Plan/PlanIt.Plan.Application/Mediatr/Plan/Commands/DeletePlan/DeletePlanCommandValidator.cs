using FluentValidation;

namespace PlanIt.Plan.Application.Mediatr.Plan.Commands.DeletePlan;

public class DeletePlanCommandValidator : AbstractValidator<DeletePlanCommand>
{
    public DeletePlanCommandValidator()
    {
        RuleFor(command => command.PlanId)
            .NotEqual(Guid.Empty);
        RuleFor(command => command.UserId)
            .NotEqual(Guid.Empty);
    }
}