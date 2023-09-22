using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Mediatr.PlanGroup.Queries.GetPlanGroups;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Queries.GetPlanGroup;

public class GetPlanGroupCommandValidator : AbstractValidator<GetPlanGroupCommand>
{
    public GetPlanGroupCommandValidator(IApplicationDbContext _dbContext)
    {
        RuleFor(c => c.UserId)
            .NotEqual(Guid.Empty);

        RuleFor(c => c.PlanGroupId)
            .NotEqual(Guid.Empty)
            .DependentRules(() =>
            {
                RuleFor(c => c.PlanGroupId)
                    .MustAsync(async (planGroupId, cancellationToken) =>
                    {
                        return await _dbContext.PlanGroups.Where(pg => pg.Id == planGroupId)
                            .AnyAsync(cancellationToken);
                    })
                    .WithMessage("PlanGroup does not exist");
            });
    }
}