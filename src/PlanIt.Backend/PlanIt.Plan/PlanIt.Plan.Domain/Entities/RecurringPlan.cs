using System.ComponentModel.DataAnnotations;

namespace PlanIt.Plan.Domain.Entities;

public class RecurringPlan : BaseEntity
{
    public string CronExpressionUtc { get; set; }
    
    public Guid PlanId { get; set; }
    public Plan Plan { get; set; }
}