using PlanIt.RabbitMq;

namespace PlanIt.Plan.Application.Features.Interfaces;

public interface IPublishHelper
{
    Task PublishScheduledPlanTriggered(ScheduledPlanTriggered message, CancellationToken cancellationToken);
}