using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using PlanIt.Plan.Application.Interfaces;
using PlanIt.Plan.Application.Mediator.Results;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.Application.Mediator.Plan.Commands;

public class DeleteScheduledPlanCommand : IRequest<OneOf<Success, NotFound, Forbidden>>
{
    public Guid ScheduledPlanId { get; set; }

    public Guid UserId { get; set; }
}

public class
    DeleteSchedulePlanCommandHandler : IRequestHandler<DeleteScheduledPlanCommand, OneOf<Success, NotFound, Forbidden>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IBackgroundJobClientV2 _backgroundJobClient;
    private readonly IRecurringJobManagerV2 _recurringJobManager;

    public DeleteSchedulePlanCommandHandler(
        IApplicationDbContext dbContext,
        IBackgroundJobClientV2 backgroundJobClient,
        IRecurringJobManagerV2 recurringJobManager)
    {
        _dbContext = dbContext;
        _backgroundJobClient = backgroundJobClient;
        _recurringJobManager = recurringJobManager;
    }

    public async Task<OneOf<Success, NotFound, Forbidden>> Handle(DeleteScheduledPlanCommand request,
        CancellationToken cancellationToken)
    {
        var scheduledPlan = await _dbContext.ScheduledPlans
            .Where(sp => sp.Id == request.ScheduledPlanId)
            .Include(sp => sp.Plan)
            .FirstOrDefaultAsync(cancellationToken);
        if (scheduledPlan is null) return new NotFound();

        if (scheduledPlan.Plan.UserId != request.UserId) return new Forbidden();
        
        _dbContext.ScheduledPlans.Remove(scheduledPlan);
        switch (scheduledPlan.Type)
        {
            case ScheduledPlanType.OneOff:
                _backgroundJobClient.Delete(scheduledPlan.HangfireId);
                break;
            case ScheduledPlanType.Recurring:
                _recurringJobManager.RemoveIfExists(scheduledPlan.HangfireId);
                break;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return new Success();
    }
}