using PlanIt.RabbitMq;

namespace PlanIt.Plan.Application.Interfaces;

public interface IPublishHelper
{
    Task PublishInstantPlanTriggered(InstantPlanTriggered message, CancellationToken cancellationToken);

    Task PublishOneOffPlanTriggered(OneOffPlanTriggered message, CancellationToken cancellationToken);

    Task PublishRecurringPlanTriggered(RecurringPlanTriggered message, CancellationToken cancellationToken);
}