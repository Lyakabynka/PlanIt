using FluentValidation;

namespace PlanIt.Plan.Application.Mediatr.ScheduledPlan.Queries.GetScheduledPlans;

public class GetScheduledPlansQueryValidator : AbstractValidator<GetScheduledPlansQuery>
{
    public GetScheduledPlansQueryValidator()
    {
        RuleFor(q => q.PlanId)
            .NotEqual(Guid.Empty);
        RuleFor(q => q.UserId)
            .NotEqual(Guid.Empty);
    }
}