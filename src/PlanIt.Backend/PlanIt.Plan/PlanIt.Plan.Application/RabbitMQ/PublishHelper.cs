using MassTransit;

namespace PlanIt.Plan.Application.RabbitMQ;

public class PublishHelper
{
    private readonly IPublishEndpoint _endpoint;

    public PublishHelper(IPublishEndpoint endpoint)
    {
        _endpoint = endpoint;
    }

    public async Task Publish<T>(T message, CancellationToken cancellationToken)
        where T : class
    {
        await _endpoint.Publish(message, cancellationToken);
    }
}