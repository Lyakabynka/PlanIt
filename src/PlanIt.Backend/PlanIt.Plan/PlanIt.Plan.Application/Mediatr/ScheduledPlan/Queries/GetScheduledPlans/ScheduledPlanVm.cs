using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.Application.Mediatr.ScheduledPlan.Queries.GetScheduledPlans;

public class ScheduledPlanVm
{
    public Guid Id { get; set; }
    
    public ScheduleType Type { get; set; }
    
    public DateTime? ExecuteUtc { get; set; }
    public string? CronExpressionUtc { get; set; }
}