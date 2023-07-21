using MassTransit;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Configurations;
using PlanIt.Plan.Application.Interfaces;

namespace PlanIt.RabbitMq;

public class PublishHelper : IPublishHelper
{
    private readonly IBus _bus;
    private readonly RabbitMqQueuesConfiguration _queues;
    private readonly IApplicationDbContext _dbContext;

    public PublishHelper(IBus bus, RabbitMqQueuesConfiguration queues, IApplicationDbContext dbContext)
    {
        _bus = bus;
        _queues = queues;
        _dbContext = dbContext;
    }

    public async Task PublishInstantPlanTriggered(InstantPlanTriggered message, CancellationToken cancellationToken)
    {
        await _bus.Send(message, cancellationToken);
    }

    public async Task PublishOneOffPlanTriggered(OneOffPlanTriggered message, CancellationToken cancellationToken)
    {
        await _bus.Send(message, cancellationToken);

        await _dbContext.OneOffPlans
            .Where(oneOffPlan => oneOffPlan.Id == message.OneOffPlanId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task PublishRecurringPlanTriggered(RecurringPlanTriggered message, CancellationToken cancellationToken)
    {
        await _bus.Send(message, cancellationToken);
    }
}