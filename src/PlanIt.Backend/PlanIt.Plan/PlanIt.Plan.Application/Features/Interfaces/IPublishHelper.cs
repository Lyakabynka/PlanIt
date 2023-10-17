using PlanIt.Messaging;

namespace PlanIt.Plan.Application.Features.Interfaces;

public interface IPublishHelper
{
    Task PublishScheduledPlanTriggered(ScheduledPlanTriggered message, CancellationToken cancellationToken);

    Task PublishScheduledPlanGroupTriggered(ScheduledPlanGroupTriggered message, CancellationToken cancellationToken);
}