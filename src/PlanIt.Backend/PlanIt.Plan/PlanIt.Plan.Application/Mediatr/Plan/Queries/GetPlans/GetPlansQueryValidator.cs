using FluentValidation;

namespace PlanIt.Plan.Application.Mediatr.Plan.Queries.GetPlans;

public class GetPlansQueryValidator : AbstractValidator<GetPlansQuery>
{
    public GetPlansQueryValidator()
    {
        RuleFor(query => query.UserId)
            .NotEqual(Guid.Empty);
    }
}