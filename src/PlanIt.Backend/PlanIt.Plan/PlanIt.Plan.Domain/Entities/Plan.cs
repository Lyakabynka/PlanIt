using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.Domain.Entities;

public class Plan : BaseEntity
{
    public string Name { get; set; }
    public string Information { get; set; }
    public string? ExecutionPath { get; set; }
    public PlanType Type { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<OneOffPlan> ScheduledPlans { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<RecurringPlan> RecurringPlans { get; set; }
    
    public Guid UserId { get; set; }
}