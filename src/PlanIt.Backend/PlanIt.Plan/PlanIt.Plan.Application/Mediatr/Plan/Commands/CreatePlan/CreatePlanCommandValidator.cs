using FluentValidation;

namespace PlanIt.Plan.Application.Mediatr.Plan.Commands.CreatePlan;

public class CreatePlanCommandValidator : AbstractValidator<CreatePlanCommand>
{
    public CreatePlanCommandValidator()
    {
        RuleFor(createPlanCommand => createPlanCommand.Name)
            .Length(4, 40);
        RuleFor(createPlanCommand => createPlanCommand.Information)
            .Length(0, 256);
        RuleFor(createPlanCommand => createPlanCommand.ExecutionPath)
            .Length(0, 256)
            
            .When(createPlanCommand => !string.IsNullOrWhiteSpace(createPlanCommand.ExecutionPath));
        RuleFor(createPlanCommand => createPlanCommand.UserId)
            .NotEqual(Guid.Empty);
    }
}