using FluentValidation;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Queries.GetPlanGroups;

public class GetPlanGroupsCommandValidator : AbstractValidator<GetPlanGroupsCommand>
{
    public GetPlanGroupsCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEqual(Guid.Empty);
    }
}