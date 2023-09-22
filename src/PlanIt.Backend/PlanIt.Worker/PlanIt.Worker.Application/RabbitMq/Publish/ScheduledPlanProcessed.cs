using PlanIt.Worker.Domain.Enums;

namespace PlanIt.RabbitMq;

public class ScheduledPlanProcessed
{
    public Guid ScheduledPlanId { get; set; }
    public ScheduleType ScheduleType { get; set; }
}