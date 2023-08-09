using MassTransit;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Configurations;
using PlanIt.Plan.Application.Interfaces;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.RabbitMq;

public class PublishHelper : IPublishHelper
{
    private readonly IBus _bus;
    private readonly IApplicationDbContext _dbContext;

    public PublishHelper(IBus bus, RabbitMqQueuesConfiguration queues, IApplicationDbContext dbContext)
    {
        _bus = bus;
        _dbContext = dbContext;
    }

    public async Task PublishScheduledPlanTriggered(ScheduledPlanTriggered message, CancellationToken cancellationToken)
    {
        await _bus.Send(message, cancellationToken);

        //TODO: make another consumer to delete OneOff-Type Scheduled Plans when it executes on Worker Side
        if (message.ScheduledPlanType == ScheduledPlanType.OneOff)
            await _dbContext.ScheduledPlans
                .Where(sp => sp.Id == message.ScheduledPlanId)
                .ExecuteDeleteAsync(cancellationToken);
    }
}