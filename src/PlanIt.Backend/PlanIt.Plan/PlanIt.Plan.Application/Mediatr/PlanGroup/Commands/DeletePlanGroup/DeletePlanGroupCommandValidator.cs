using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Commands.DeletePlanGroup;

public class DeletePlanGroupCommandValidator : AbstractValidator<DeletePlanGroupCommand>
{
    public DeletePlanGroupCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(c => c.PlanGroupId)
            .NotEqual(Guid.Empty)
            .DependentRules(() =>
            {
                RuleFor(c => c.PlanGroupId)
                    .MustAsync(async (planGroupId, cancellationToken) =>
                    {
                        return await dbContext.PlanGroups
                            .Where(pg=>pg.Id == planGroupId)
                            .AnyAsync(cancellationToken);
                    });
            });

        RuleFor(c => c.UserId)
            .NotEqual(Guid.Empty);
    }
}