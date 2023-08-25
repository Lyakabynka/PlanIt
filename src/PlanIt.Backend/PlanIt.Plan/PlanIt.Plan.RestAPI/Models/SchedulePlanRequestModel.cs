using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.RestAPI.Models;

public class SchedulePlanRequestModel
{
    public Guid PlanId { get; set; }
    
    public ScheduledPlanType Type { get; set; }
    
    public string? CronExpressionUtc { get; set; }
    public DateTime? ExecuteUtc { get; set; }
    
    public string? Arguments { get; set; }
}