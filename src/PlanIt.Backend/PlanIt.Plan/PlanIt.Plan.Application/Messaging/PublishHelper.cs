using MassTransit;
using PlanIt.Plan.Application.Configurations;
using PlanIt.Plan.Application.Features.Interfaces;

namespace PlanIt.Messaging;

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
    }

    public async Task PublishScheduledPlanGroupTriggered(ScheduledPlanGroupTriggered message,
        CancellationToken cancellationToken)
    {
        await _bus.Send(message, cancellationToken);
    }
}