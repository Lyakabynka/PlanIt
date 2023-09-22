using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.Application.Mediatr.Plan.Commands.DeletePlan;

public class DeletePlanCommandHandler : IRequestHandler<DeletePlanCommand, Result>
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

    public async Task<Result> Handle(DeletePlanCommand request,
        CancellationToken cancellationToken)
    {
        var plan = await _dbContext.Plans
            .Where(plan => plan.Id == request.PlanId)
            .Include(plan=>plan.ScheduledPlans)
            .FirstAsync(cancellationToken);

        if (plan.UserId != request.UserId) return Result.FormForbidden();
        // deleting scheduled/recurring jobs from hangfire

        foreach (var scheduledPlan in plan.ScheduledPlans)
        {
            switch (scheduledPlan.Type)
            {
                case ScheduleType.OneOff:
                    _backgroundJobClient.Delete(scheduledPlan.HangfireId);
                    break;
                case ScheduleType.Recurring:
                    _recurringJobClient.RemoveIfExists(scheduledPlan.HangfireId);
                    break;
            }
        }

        // removing plan from Database along with active plans ( Cascade Behavior )
        _dbContext.Plans.Remove(plan);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Create(new {});
    }
}