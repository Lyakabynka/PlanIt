using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;

namespace PlanIt.Plan.Application.Mediatr.Plan.Commands.DeletePlan;

public class DeletePlanCommandValidator : AbstractValidator<DeletePlanCommand>
{
    public DeletePlanCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(c => c.PlanId)
            .NotEqual(Guid.Empty)
            .DependentRules(() =>
            {
                RuleFor((c) => c.PlanId)
                    .MustAsync(async (planId, cancellationToken) =>
                    {
                        return await dbContext.Plans.Where(p => p.Id == planId).AnyAsync(cancellationToken);
                    })
                    .WithMessage("Plan does not exist");
            });
        RuleFor(c => c.UserId)
            .NotEqual(Guid.Empty);
    }
}