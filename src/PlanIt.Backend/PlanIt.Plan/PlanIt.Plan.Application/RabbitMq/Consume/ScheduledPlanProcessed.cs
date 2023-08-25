using MassTransit;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.RabbitMq;

public class ScheduledPlanProcessed
{
    public Guid ScheduledPlanId { get; set; }
    public ScheduledPlanType ScheduledPlanType { get; set; }
}

public class ScheduledPlanProcessedConsumer : IConsumer<ScheduledPlanProcessed>
{
    private readonly IApplicationDbContext _dbContext;

    public ScheduledPlanProcessedConsumer(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ScheduledPlanProcessed> context)
    {
        if (context.Message.ScheduledPlanType == ScheduledPlanType.OneOff)
        {
            await _dbContext.ScheduledPlans
                .Where(scheduledPlan => scheduledPlan.Id == context.Message.ScheduledPlanId)
                .ExecuteDeleteAsync(context.CancellationToken);
        }
    }
}