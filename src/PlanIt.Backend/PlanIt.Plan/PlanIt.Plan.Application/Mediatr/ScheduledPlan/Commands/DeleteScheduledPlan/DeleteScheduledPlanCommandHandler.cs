using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.Application.Mediatr.ScheduledPlan.Commands.DeleteScheduledPlan;


public class DeleteSchedulePlanCommandHandler : IRequestHandler<DeleteScheduledPlanCommand, Result>
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

    public async Task<Result> Handle(DeleteScheduledPlanCommand request,
        CancellationToken cancellationToken)
    {
        var scheduledPlan = await _dbContext.ScheduledPlans
            .Where(sp => sp.Id == request.ScheduledPlanId)
            .Include(sp => sp.Plan)
            .FirstAsync(cancellationToken);
        
        if (scheduledPlan.Plan.UserId != request.UserId)
            return Result.FormForbidden();
        
        _dbContext.ScheduledPlans.Remove(scheduledPlan);
        switch (scheduledPlan.Type)
        {
            case ScheduleType.OneOff:
                _backgroundJobClient.Delete(scheduledPlan.HangfireId);
                break;
            case ScheduleType.Recurring:
                _recurringJobManager.RemoveIfExists(scheduledPlan.HangfireId);
                break;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Create(new {});
    }
}