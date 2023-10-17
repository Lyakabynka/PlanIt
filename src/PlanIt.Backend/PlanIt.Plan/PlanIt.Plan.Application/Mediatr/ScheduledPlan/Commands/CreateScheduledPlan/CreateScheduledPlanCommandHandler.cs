using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PlanIt.Messaging;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Mediatr.Plan.Queries.GetPlans;
using PlanIt.Plan.Application.Response;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.Application.Mediatr.ScheduledPlan.Commands.CreateScheduledPlan;

public class CreateScheduledPlanCommandHandler :
    IRequestHandler<CreateScheduledPlanCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IBackgroundJobClientV2 _backgroundJobClient;
    private readonly IRecurringJobManagerV2 _recurringJobManager;
    private readonly IPublishHelper _publishHelper;

    public CreateScheduledPlanCommandHandler(IPublishHelper publishHelper, IBackgroundJobClientV2 backgroundJobClient,
        IApplicationDbContext dbContext, IRecurringJobManagerV2 recurringJobManager)
    {
        _publishHelper = publishHelper;
        _backgroundJobClient = backgroundJobClient;
        _dbContext = dbContext;
        _recurringJobManager = recurringJobManager;
    }

    public async Task<Result> Handle(CreateScheduledPlanCommand request,
        CancellationToken cancellationToken)
    {
        var plan = await _dbContext.Plans
            .Where(plan => plan.Id == request.PlanId)
            .FirstAsync(cancellationToken);

        if (plan.UserId != request.UserId) 
            return Result.FormForbidden();
        
        var scheduledPlanId = Guid.NewGuid();
        string? hangfireId = null;
        switch (request.Type)
        {
            case ScheduleType.Instant:
                await _publishHelper.PublishScheduledPlanTriggered(new ScheduledPlanTriggered()
                {
                    ScheduledPlanId = scheduledPlanId,
                    ScheduleType = request.Type,
                    Plan = new PlanVm()
                    {
                        Id = plan.Id,
                        Name = plan.Name,
                        //If arguments are not null, substitute information with arguments
                        Information = string.IsNullOrWhiteSpace(request.Arguments) ? plan.Information : request.Arguments,
                        ExecutionPath = plan.ExecutionPath,
                        Type = plan.Type, 
                    },
                    UserId = plan.UserId
                }, cancellationToken);
                break;
            case ScheduleType.OneOff
                when request.ExecuteUtc is not null:
                hangfireId =
                    _backgroundJobClient.Schedule(
                        () =>
                            //sending using masstransit
                            _publishHelper.PublishScheduledPlanTriggered(
                                new ScheduledPlanTriggered()
                                {
                                    ScheduledPlanId = scheduledPlanId,
                                    ScheduleType = request.Type,
                                    Plan = new PlanVm()
                                    {
                                        Id = plan.Id,
                                        Name = plan.Name,
                                        //If arguments are not null, substitute information with arguments
                                        Information = string.IsNullOrWhiteSpace(request.Arguments) ? plan.Information : request.Arguments,
                                        ExecutionPath = plan.ExecutionPath,
                                        Type = plan.Type, 
                                    },
                                    UserId = plan.UserId
                                }, cancellationToken),
                        request.ExecuteUtc.Value.ToLocalTime());

                //needs to be deleted once executed in hangfire
                _dbContext.ScheduledPlans.Add(new Domain.Entities.ScheduledPlan()
                {
                    Id = scheduledPlanId,
                    Type = request.Type,
                    HangfireId = hangfireId,
                    ExecuteUtc = request.ExecuteUtc,
                    PlanId = plan.Id
                });
                break;
            case ScheduleType.Recurring
                when request.CronExpressionUtc is not null:

                hangfireId = scheduledPlanId.ToString();

                _recurringJobManager.AddOrUpdate(
                    hangfireId,
                    () =>
                        //method which sends message to determined queue ( recurring plan queue )
                        _publishHelper.PublishScheduledPlanTriggered(
                            new ScheduledPlanTriggered()
                            {
                                ScheduledPlanId = scheduledPlanId,
                                ScheduleType = request.Type,
                                Plan = new PlanVm()
                                {
                                    Id = plan.Id,
                                    Name = plan.Name,
                                    //If arguments are not null, substitute information with arguments
                                    Information = string.IsNullOrWhiteSpace(request.Arguments) ? plan.Information : request.Arguments,
                                    ExecutionPath = plan.ExecutionPath,
                                    Type = plan.Type, 
                                },
                                UserId = plan.UserId
                            }, cancellationToken),
                    request.CronExpressionUtc,
                    new RecurringJobOptions()
                    {
                        TimeZone = TimeZoneInfo.Utc
                    });
                
                _dbContext.ScheduledPlans.Add(new Domain.Entities.ScheduledPlan()
                {
                    Id = scheduledPlanId,
                    Type = request.Type,
                    HangfireId = hangfireId,
                    CronExpressionUtc = request.CronExpressionUtc,
                    PlanId = plan.Id
                });
                break;
            //TODO: Startup
        }
        
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Create(new {});
    }
}