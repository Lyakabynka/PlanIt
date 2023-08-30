using FluentValidation;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Commands.AddPlanToPlanGroup;

public class AddPlanToPlanGroupCommandValidator : AbstractValidator<AddPlanToPlanGroupCommand>
{
    public AddPlanToPlanGroupCommandValidator()
    {
        RuleFor(c => c.PlanId)
            .NotEqual(Guid.Empty);
        RuleFor(c => c.PlanGroupId)
            .NotEqual(Guid.Empty);
        
        RuleFor(c => c.UserId)
            .NotEqual(Guid.Empty);
    }
}