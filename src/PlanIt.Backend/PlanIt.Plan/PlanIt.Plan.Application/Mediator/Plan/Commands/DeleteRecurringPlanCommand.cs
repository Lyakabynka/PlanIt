using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using PlanIt.Plan.Application.Interfaces;
using PlanIt.Plan.Application.Mediator.Results;

namespace PlanIt.Plan.Application.Mediator.Plan.Commands;

public class DeleteRecurringPlanCommand : IRequest<OneOf<Success, NotFound, Forbidden>>
{
    public Guid OneOffPlanId { get; set; }
    
    public Guid UserId { get; set; }
}

public class DeleteRecurringPlanCommandHandler : IRequestHandler<DeleteRecurringPlanCommand, OneOf<Success, NotFound, Forbidden>>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteRecurringPlanCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OneOf<Success, NotFound, Forbidden>> Handle(DeleteRecurringPlanCommand request, CancellationToken cancellationToken)
    {
        var recurringPlan = await _dbContext.RecurringPlans
            .Where(oneOffPlan => oneOffPlan.Id == request.OneOffPlanId)
            .Include(oneOffPlan => oneOffPlan.Plan)
            .FirstOrDefaultAsync(cancellationToken);
        if (recurringPlan is null) return new NotFound();

        if (recurringPlan.Plan.UserId != request.UserId) return new Forbidden();

        _dbContext.RecurringPlans.Remove(recurringPlan);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}