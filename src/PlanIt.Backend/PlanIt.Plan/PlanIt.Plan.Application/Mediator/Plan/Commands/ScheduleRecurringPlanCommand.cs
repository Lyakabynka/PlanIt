using Cronos;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using PlanIt.Plan.Application.Interfaces;
using PlanIt.Plan.Application.Mediator.Plan.Notifications;
using PlanIt.Plan.Application.Mediator.Results;
using PlanIt.Plan.Domain.Entities;

namespace PlanIt.Plan.Application.Mediator.Plan.Commands;

public class ScheduleRecurringPlanCommand : IRequest<OneOf<Success, NotFound, Forbidden, BadRequest>>
{
    public Guid PlanId { get; set; }
    public string CronExpressionUtc { get; set; }

    public Guid UserId { get; set; }
}

public class ScheduleRecurringPlanCommandHandler : IRequestHandler<ScheduleRecurringPlanCommand,
    OneOf<Success, NotFound, Forbidden, BadRequest>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IRecurringJobManagerV2 _recurringJobManager;
    private readonly IPublisher _publisher;

    public ScheduleRecurringPlanCommandHandler(IApplicationDbContext dbContext,
        IRecurringJobManagerV2 recurringJobManager, IPublisher publisher)
    {
        _dbContext = dbContext;
        _recurringJobManager = recurringJobManager;
        _publisher = publisher;
    }

    public async Task<OneOf<Success, NotFound, Forbidden, BadRequest>> Handle(ScheduleRecurringPlanCommand request,
        CancellationToken cancellationToken)
    {
        var plan = await _dbContext.Plans
            .Where(plan => plan.Id == request.PlanId)
            .AsTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (plan is null) return new NotFound();

        if (plan.UserId != request.UserId) return new Forbidden();

        if (CronExpression.Parse(request.CronExpressionUtc, CronFormat.IncludeSeconds) is null)
            return new BadRequest(new Error("CronExpressionUtc", "Invalid cron expression"));

        var recurringPlanId = Guid.NewGuid();
        _recurringJobManager.AddOrUpdate(
            recurringPlanId.ToString(),
            () => _publisher.Publish(
                new PlanTransmissionRequested
                {
                    Plan = plan
                }, cancellationToken),
            request.CronExpressionUtc,
            new RecurringJobOptions()
            {
                TimeZone = TimeZoneInfo.Utc
            });


        _dbContext.RecurringPlans.Add(new RecurringPlan
        {
            Id = recurringPlanId,
            CronExpressionUtc = request.CronExpressionUtc,
            PlanId = plan.Id,
        });
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}