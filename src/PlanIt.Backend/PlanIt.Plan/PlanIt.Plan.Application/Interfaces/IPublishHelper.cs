using PlanIt.RabbitMq;

namespace PlanIt.Plan.Application.Interfaces;

public interface IPublishHelper
{
    Task PublishScheduledPlanTriggered(ScheduledPlanTriggered message, CancellationToken cancellationToken);
}