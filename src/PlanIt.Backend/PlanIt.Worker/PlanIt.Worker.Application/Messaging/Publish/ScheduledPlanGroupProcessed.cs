using PlanIt.Worker.Domain.Enums;

namespace PlanIt.Messaging;

public class ScheduledPlanGroupProcessed
{
    public Guid ScheduledPlanGroupId { get; set; }
    public ScheduleType ScheduleType { get; set; }
}