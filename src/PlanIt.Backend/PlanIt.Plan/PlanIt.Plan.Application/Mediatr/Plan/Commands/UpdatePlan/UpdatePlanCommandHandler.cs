using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.Application.Mediatr.Plan.Commands.UpdatePlan;

public class UpdatePlanCommandHandler : IRequestHandler<UpdatePlanCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IBackgroundJobClientV2 _backgroundJobClient;
    private readonly IRecurringJobManagerV2 _recurringJobClient;

    public UpdatePlanCommandHandler(IApplicationDbContext dbContext, IBackgroundJobClientV2 backgroundJobClient,
        IRecurringJobManagerV2 recurringJobClient)
    {
        _dbContext = dbContext;
        _backgroundJobClient = backgroundJobClient;
        _recurringJobClient = recurringJobClient;
    }

    public async Task<Result> Handle(UpdatePlanCommand request,
        CancellationToken cancellationToken)
    {
        var plan = await _dbContext.Plans
            .Where(plan => plan.Id == request.PlanId)
            .Include(plan => plan.ScheduledPlans)
            .AsTracking()
            .FirstAsync(cancellationToken);

        if (plan.UserId != request.UserId) 
            return Result.FormForbidden();

        plan.Name = request.Name;
        plan.Information = request.Information;
        plan.ExecutionPath = request.ExecutionPath;
        plan.Type = request.Type;

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

        await _dbContext.SaveChangesAsync(cancellationToken);

        plan.ScheduledPlans = default!;

        return Result.Create(plan);
    }
}