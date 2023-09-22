using FluentValidation;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Queries.GetPlanGroups;

public class GetPlanGroupVmsCommandValidator : AbstractValidator<GetPlanGroupVmsCommand>
{
    public GetPlanGroupVmsCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEqual(Guid.Empty);
    }
}