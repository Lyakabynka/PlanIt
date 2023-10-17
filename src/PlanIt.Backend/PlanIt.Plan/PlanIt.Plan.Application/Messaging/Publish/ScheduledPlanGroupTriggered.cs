using PlanIt.Plan.Application.Mediatr.Plan.Queries.GetPlans;
using PlanIt.Plan.Application.Mediatr.PlanGroup.Queries.GetPlanGroup;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Messaging;

public class ScheduledPlanGroupTriggered
{
    public Guid ScheduledPlanGroupId { get; set; }
    public ScheduleType ScheduleType { get; set; }

    public Guid PlanGroupId { get; set; }
    
    public List<PlanPlanGroupVm> PlanPlanGroups { get; set; }
    
    public Guid UserId { get; set; }
}