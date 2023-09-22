using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;

namespace PlanIt.Plan.Application.Mediatr.Plan.Commands.UpdatePlan;

public class UpdatePlanCommandValidator : AbstractValidator<UpdatePlanCommand>
{
    public UpdatePlanCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(command => command.PlanId)
            .NotEqual(Guid.Empty);
        RuleFor(command => command.Name)
            .Length(4, 40)
            .DependentRules(() =>
            {
                RuleFor(c => c.Name)
                    .MustAsync(async (name, cancellationToken) =>
                    {
                        return await dbContext.Plans.Where(p => p.Name == name).AnyAsync(cancellationToken);
                    })
                    .WithMessage("Plan does not exist");
            });
        RuleFor(command => command.Information)
            .Length(0, 256);
        RuleFor(command => command.ExecutionPath)
            .Length(0, 256)
            .When(command => !string.IsNullOrWhiteSpace(command.ExecutionPath));

        RuleFor(command => command.UserId)
            .NotEqual(Guid.Empty);
    }
}