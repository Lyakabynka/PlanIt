using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.RestAPI.Models;

public class SchedulePlanRequestModel
{
    public ScheduledPlanType Type { get; set; }
    
    public string? CronExpressionUtc { get; set; }
    public DateTime? ExecuteUtc { get; set; }
}