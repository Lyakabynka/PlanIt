using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.RestAPI.Models;

public class CreateScheduledPlanGroupRequestModel
{
    public Guid PlanGroupId { get; set; }
    
    public ScheduleType Type { get; set; }
    
    public string? CronExpressionUtc { get; set; }
    public DateTime? ExecuteUtc { get; set; }
    
    //Possible not null value only when executed using voice command
    public string? Arguments { get; set; }
}