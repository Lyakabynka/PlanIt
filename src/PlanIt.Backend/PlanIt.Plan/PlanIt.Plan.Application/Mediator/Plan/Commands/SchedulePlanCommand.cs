using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using PlanIt.Plan.Application.Interfaces;
using PlanIt.Plan.Application.Mediator.Results;
using PlanIt.Plan.Domain.Entities;
using PlanIt.Plan.Domain.Enums;
using PlanIt.RabbitMq;

namespace PlanIt.Plan.Application.Mediator.Plan.Commands;

public class SchedulePlanCommand : 
    IRequest<OneOf<Success, NotFound, Forbidden, BadRequest>>
{
    public Guid PlanId { get; set; }

    public string? CronExpressionUtc { get; set; }
    public DateTime? ExecuteUtc { get; set; }

    public Guid UserId { get; set; }

    public ScheduledPlanType Type { get; set; }
}

public class SchedulePlanCommandHandler : 
    IRequestHandler<SchedulePlanCommand, OneOf<Success, NotFound, Forbidden,BadRequest>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IBackgroundJobClientV2 _backgroundJobClient;
    private readonly IRecurringJobManagerV2 _recurringJobManager;
    private readonly IPublishHelper _publishHelper;

    public SchedulePlanCommandHandler(IPublishHelper publishHelper, IBackgroundJobClientV2 backgroundJobClient,
        IApplicationDbContext dbContext, IRecurringJobManagerV2 recurringJobManager)
    {
        _publishHelper = publishHelper;
        _backgroundJobClient = backgroundJobClient;
        _dbContext = dbContext;
        _recurringJobManager = recurringJobManager;
    }

    public async Task<OneOf<Success, NotFound, Forbidden, BadRequest>> Handle(SchedulePlanCommand request,
        CancellationToken cancellationToken)
    {
        var plan = await _dbContext.Plans
            .Where(plan => plan.Id == request.PlanId)
            .AsTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (plan is null) return new NotFound();

        if (plan.UserId != request.UserId) return new Forbidden();

        var scheduledPlanId = Guid.NewGuid();
        string? hangfireId = null;
        switch (request.Type)
        {
            case ScheduledPlanType.Instant:
                await _publishHelper.PublishScheduledPlanTriggered(new ScheduledPlanTriggered()
                {
                    ScheduledPlanId = scheduledPlanId,
                    ScheduledPlanType = request.Type,
                    Id = plan.Id,
                    Name = plan.Name,
                    Information = plan.Information,
                    ExecutionPath = plan.ExecutionPath,
                    PlanType = plan.Type,
                    UserId = plan.UserId
                }, cancellationToken);
                break;
            case ScheduledPlanType.OneOff
                when request.ExecuteUtc is not null:
                hangfireId =
                    _backgroundJobClient.Schedule(
                        () =>
                            //sending using masstransit
                            _publishHelper.PublishScheduledPlanTriggered(
                                new ScheduledPlanTriggered()
                                {
                                    ScheduledPlanId = scheduledPlanId,
                                    ScheduledPlanType = request.Type,
                                    Id = plan.Id,
                                    Name = plan.Name,
                                    Information = plan.Information,
                                    ExecutionPath = plan.ExecutionPath,
                                    PlanType = plan.Type,
                                    UserId = plan.UserId
                                }, cancellationToken),
                        request.ExecuteUtc.Value.ToLocalTime());

                //needs to be deleted once executed in hangfire
                _dbContext.ScheduledPlans.Add(new ScheduledPlan()
                {
                    Id = scheduledPlanId,
                    HangfireId = hangfireId,
                    ExecuteUtc = request.ExecuteUtc,
                    PlanId = plan.Id
                });
                break;
            case ScheduledPlanType.Recurring
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
                                ScheduledPlanType = request.Type,
                                Id = plan.Id,
                                Name = plan.Name,
                                Information = plan.Information,
                                ExecutionPath = plan.ExecutionPath,
                                PlanType = plan.Type,
                                UserId = plan.UserId
                            }, cancellationToken),
                    request.CronExpressionUtc,
                    new RecurringJobOptions()
                    {
                        TimeZone = TimeZoneInfo.Utc
                    });
                break;
            //TODO: Startup
            default:
                return new BadRequest();
        }

        _dbContext.ScheduledPlans.Add(new ScheduledPlan()
        {
            Type = request.Type,
            HangfireId = hangfireId,
            ExecuteUtc = request.ExecuteUtc,
            CronExpressionUtc = request.CronExpressionUtc,
            PlanId = plan.Id
        });
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}