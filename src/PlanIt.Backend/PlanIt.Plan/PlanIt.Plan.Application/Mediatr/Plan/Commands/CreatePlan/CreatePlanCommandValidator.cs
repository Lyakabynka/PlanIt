using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;

namespace PlanIt.Plan.Application.Mediatr.Plan.Commands.CreatePlan;

public class CreatePlanCommandValidator : AbstractValidator<CreatePlanCommand>
{
    public CreatePlanCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(createPlanCommand => createPlanCommand.Name)
            .Length(4, 40)
            .DependentRules(() =>
            {
                RuleFor(c => c)
                    .MustAsync(async (c, cancellationToken) =>
                    {
                        return !await dbContext.Plans
                            .Where(p => p.UserId == c.UserId && p.Name == c.Name)
                            .AnyAsync(cancellationToken);
                    })
                    .WithMessage("Plan already exists")
                    .WithName("Name");
            });
        RuleFor(createPlanCommand => createPlanCommand.Information)
            .Length(0, 256);
        RuleFor(createPlanCommand => createPlanCommand.ExecutionPath)
            .Length(0, 256)
            .When(createPlanCommand => !string.IsNullOrWhiteSpace(createPlanCommand.ExecutionPath));
        RuleFor(createPlanCommand => createPlanCommand.UserId)
            .NotEqual(Guid.Empty);
    }
}