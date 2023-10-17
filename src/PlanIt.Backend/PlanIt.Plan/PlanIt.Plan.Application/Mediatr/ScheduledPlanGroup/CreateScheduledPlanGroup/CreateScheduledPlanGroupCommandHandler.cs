using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PlanIt.Messaging;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Mediatr.Plan.Queries.GetPlans;
using PlanIt.Plan.Application.Mediatr.PlanGroup.Queries.GetPlanGroup;
using PlanIt.Plan.Application.Response;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.Application.Mediatr.ScheduledPlanGroup.CreateScheduledPlanGroup;

public class CreateScheduledPlanGroupCommandHandler: IRequestHandler<CreateScheduledPlanGroupCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IBackgroundJobClientV2 _backgroundJobClient;
    private readonly IRecurringJobManagerV2 _recurringJobManager;
    private readonly IPublishHelper _publishHelper;
    
    public CreateScheduledPlanGroupCommandHandler(IApplicationDbContext dbContext, IBackgroundJobClientV2 backgroundJobClient, IRecurringJobManagerV2 recurringJobManager, IPublishHelper publishHelper)
    {
        _dbContext = dbContext;
        _backgroundJobClient = backgroundJobClient;
        _recurringJobManager = recurringJobManager;
        _publishHelper = publishHelper;
    }

    public async Task<Result> Handle(CreateScheduledPlanGroupCommand request, CancellationToken cancellationToken)
    {
        var planGroup = await _dbContext.PlanGroups
            .Where(pg => pg.Id == request.PlanGroupId)
            .Include(pg=>pg.PlanPlanGroups)
            .ThenInclude(ppg=>ppg.Plan)
            .Select(pg => new
            {
                Id = pg.Id,
                
                PlanPlanGroups = pg.PlanPlanGroups
                    .Select(ppg=> new PlanPlanGroupVm()
                    {
                       Id = ppg.Id,
                       Index = ppg.Index,
                       Plan = new PlanVm()
                       {
                           Id = ppg.Plan.Id,
                           Name = ppg.Plan.Name,
                           Information = ppg.Plan.Information,
                           ExecutionPath = ppg.Plan.ExecutionPath,
                           Type = ppg.Plan.Type,
                       }
                    }).ToList(),

                UserId = pg.UserId,
            })
            .FirstAsync(cancellationToken);

        if (planGroup.UserId != request.UserId) 
            return Result.FormForbidden();
        
        var scheduledPlanGroupId = Guid.NewGuid();
        string? hangfireId = null;
        switch (request.Type)
        {
            case ScheduleType.Instant:
                await _publishHelper.PublishScheduledPlanGroupTriggered(new ScheduledPlanGroupTriggered()
                {
                    ScheduledPlanGroupId = scheduledPlanGroupId,
                    ScheduleType = request.Type,
                    
                    PlanGroupId = planGroup.Id,
                    PlanPlanGroups = planGroup.PlanPlanGroups,
                    
                    UserId = planGroup.UserId,
                }, cancellationToken);
                break;
            case ScheduleType.OneOff
                when request.ExecuteUtc is not null:
                hangfireId =
                    _backgroundJobClient.Schedule(
                        () =>
                            //sending using masstransit
                            _publishHelper.PublishScheduledPlanGroupTriggered(
                                new ScheduledPlanGroupTriggered()
                                {
                                    ScheduledPlanGroupId = scheduledPlanGroupId,
                                    ScheduleType = request.Type,
                                    
                                    PlanGroupId = planGroup.Id,
                                    PlanPlanGroups = planGroup.PlanPlanGroups,
                                    
                                    UserId = planGroup.UserId,
                                }, cancellationToken),
                        request.ExecuteUtc.Value.ToLocalTime());

                //needs to be deleted once executed in hangfire
                _dbContext.ScheduledPlanGroups.Add(new Domain.Entities.ScheduledPlanGroup()
                {
                    Id = scheduledPlanGroupId,
                    Type = request.Type,
                    HangfireId = hangfireId,
                    ExecuteUtc = request.ExecuteUtc,
                    PlanGroupId = planGroup.Id
                });
                break;
            case ScheduleType.Recurring
                when request.CronExpressionUtc is not null:

                hangfireId = scheduledPlanGroupId.ToString();

                _recurringJobManager.AddOrUpdate(
                    hangfireId,
                    () =>
                        //method which sends message to determined queue ( scheduled PlanGroup queue )
                        _publishHelper.PublishScheduledPlanGroupTriggered(
                            new ScheduledPlanGroupTriggered()
                            {
                                ScheduledPlanGroupId = scheduledPlanGroupId,
                                ScheduleType = request.Type,
                                    
                                PlanGroupId = planGroup.Id,
                                PlanPlanGroups = planGroup.PlanPlanGroups,
                                    
                                UserId = planGroup.UserId,
                            }, cancellationToken),
                    request.CronExpressionUtc,
                    new RecurringJobOptions()
                    {
                        TimeZone = TimeZoneInfo.Utc
                    });
                
                _dbContext.ScheduledPlanGroups.Add(new Domain.Entities.ScheduledPlanGroup()
                {
                    Id = scheduledPlanGroupId,
                    Type = request.Type,
                    HangfireId = hangfireId,
                    CronExpressionUtc = request.CronExpressionUtc,
                    PlanGroupId = planGroup.Id
                });
                break;
            //TODO: Startup
        }
        
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Create(new {});
    }
}