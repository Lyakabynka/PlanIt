using Hangfire;
using Hangfire.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using PlanIt.Plan.Application.Interfaces;
using PlanIt.Plan.Application.Mediator.Results;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.Application.Mediator.Plan.Commands;

public class DeletePlanCommand : IRequest<OneOf<Success, NotFound, Forbidden>>
{
    public Guid PlanId { get; set; }

    public Guid UserId { get; set; }
}

public class DeletePlanCommandHandler : IRequestHandler<DeletePlanCommand, OneOf<Success, NotFound, Forbidden>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IBackgroundJobClientV2 _backgroundJobClient;
    private readonly IRecurringJobManagerV2 _recurringJobClient;

    public DeletePlanCommandHandler(IApplicationDbContext dbContext, IBackgroundJobClientV2 backgroundJobClient,
        IRecurringJobManagerV2 recurringJobClient)
    {
        _dbContext = dbContext;
        _backgroundJobClient = backgroundJobClient;
        _recurringJobClient = recurringJobClient;
    }

    public async Task<OneOf<Success, NotFound, Forbidden>> Handle(DeletePlanCommand request,
        CancellationToken cancellationToken)
    {
        var plan = await _dbContext.Plans
            .Where(plan => plan.Id == request.PlanId)
            .Include(plan=>plan.ScheduledPlans)
            .FirstOrDefaultAsync(cancellationToken);
        if (plan is null) return new NotFound();

        if (plan.UserId != request.UserId) return new Forbidden();

        // deleting scheduled/recurring jobs from hangfire

        foreach (var scheduledPlan in plan.ScheduledPlans)
        {
            switch (scheduledPlan.Type)
            {
                case ScheduledPlanType.OneOff:
                    _backgroundJobClient.Delete(scheduledPlan.HangfireId);
                    break;
                case ScheduledPlanType.Recurring:
                    _recurringJobClient.RemoveIfExists(scheduledPlan.HangfireId);
                    break;
            }
        }

        // removing plan from Database along with active plans ( Cascade Behavior )
        _dbContext.Plans.Remove(plan);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}