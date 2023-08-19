using PlanIt.Plan.Domain.Enums;

namespace PlanIt.RabbitMq;

public class ScheduledPlanProcessed
{
    public Guid ScheduledPlanId { get; set; }
    public ScheduledPlanType ScheduledPlanType { get; set; }
}