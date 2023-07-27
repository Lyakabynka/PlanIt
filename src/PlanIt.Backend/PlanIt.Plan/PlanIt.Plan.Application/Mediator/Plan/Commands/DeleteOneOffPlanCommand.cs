using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using PlanIt.Plan.Application.Interfaces;
using PlanIt.Plan.Application.Mediator.Results;

namespace PlanIt.Plan.Application.Mediator.Plan.Commands;

public class DeleteOneOffPlanCommand : IRequest<OneOf<Success, NotFound, Forbidden>>
{
    public Guid OneOffPlanId { get; set; }
    
    public Guid UserId { get; set; }
}

public class DeleteOneOffPlanCommandHandler : IRequestHandler<DeleteOneOffPlanCommand, OneOf<Success, NotFound, Forbidden>>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteOneOffPlanCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OneOf<Success, NotFound, Forbidden>> Handle(DeleteOneOffPlanCommand request, CancellationToken cancellationToken)
    {
        var oneOffPlan = await _dbContext.OneOffPlans
            .Where(oneOffPlan => oneOffPlan.Id == request.OneOffPlanId)
            .Include(oneOffPlan => oneOffPlan.Plan)
            .FirstOrDefaultAsync(cancellationToken);
        if (oneOffPlan is null) return new NotFound();

        if (oneOffPlan.Plan.UserId != request.UserId) return new Forbidden();

        _dbContext.OneOffPlans.Remove(oneOffPlan);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}