using PlanIt.Plan.Domain.Contracts;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.Domain.Entities;

public class ScheduledPlanGroup : BaseEntity, IScheduled
{
    public ScheduleType Type { get; set; }
    public string? HangfireId { get; set; }
    public DateTime? ExecuteUtc { get; set; }
    public string? CronExpressionUtc { get; set; }
    
    public Guid PlanGroupId { get; set; }
    public PlanGroup PlanGroup { get; set; }
}