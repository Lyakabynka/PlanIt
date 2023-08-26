using FluentValidation;

namespace PlanIt.Plan.Application.Mediatr.Plan.Commands.UpdatePlan;

public class UpdatePlanCommandValidator : AbstractValidator<UpdatePlanCommand>
{
    public UpdatePlanCommandValidator()
    {
        RuleFor(command => command.PlanId)
            .NotEqual(Guid.Empty);
        RuleFor(command => command.Name)
            .Length(4, 40);
        RuleFor(command => command.Information)
            .Length(0, 256);
        RuleFor(command => command.ExecutionPath)
            .Length(0, 256)
            .When(command => !string.IsNullOrWhiteSpace(command.ExecutionPath));

        RuleFor(command => command.UserId)
            .NotEqual(Guid.Empty);
    }
}