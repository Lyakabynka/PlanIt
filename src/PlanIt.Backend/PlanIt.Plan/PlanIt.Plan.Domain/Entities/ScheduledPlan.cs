using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.Domain.Entities;

public class ScheduledPlan : BaseEntity
{
    public ScheduledPlanType Type { get; set; }
    
    public string? HangfireId { get; set; }
    
    public DateTime? ExecuteUtc { get; set; }
    public string? CronExpressionUtc { get; set; }

    public Guid PlanId { get; set; }
    public Plan Plan { get; set; }
}