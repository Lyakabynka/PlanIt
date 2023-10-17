using MassTransit;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Messaging;

public class ScheduledPlanGroupProcessed
{
    public Guid ScheduledPlanGroupId { get; set; }
    public ScheduleType ScheduleType { get; set; }
}

public class ScheduledPlanGroupProcessedConsumer : IConsumer<ScheduledPlanGroupProcessed>
{
    private readonly IApplicationDbContext _dbContext;

    public ScheduledPlanGroupProcessedConsumer(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ScheduledPlanGroupProcessed> context)
    {
        if (context.Message.ScheduleType == ScheduleType.OneOff)
        {
            await _dbContext.ScheduledPlanGroups
                .Where(spg => spg.Id == context.Message.ScheduledPlanGroupId)
                .ExecuteDeleteAsync(context.CancellationToken);
        }
    }
}