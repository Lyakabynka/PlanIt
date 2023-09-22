using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.Domain.Contracts;

public interface IScheduled
{
    ScheduleType Type { get; set; }
    
    string? HangfireId { get; set; }
    
    DateTime? ExecuteUtc { get; set; }
    string? CronExpressionUtc { get; set; }
}