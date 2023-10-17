using PlanIt.Plan.Application.Mediatr.Plan.Queries.GetPlans;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Messaging;

public class ScheduledPlanTriggered
{
    public Guid ScheduledPlanId { get; set; }
    public ScheduleType ScheduleType { get; set; }
    
    public PlanVm Plan { get; set; }
    
    public Guid UserId { get; set; }
}