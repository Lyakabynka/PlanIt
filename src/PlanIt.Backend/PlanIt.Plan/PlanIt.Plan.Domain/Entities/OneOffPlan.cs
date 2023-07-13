using System.ComponentModel.DataAnnotations;

namespace PlanIt.Plan.Domain.Entities;

public class OneOffPlan : BaseEntity
{
    public string HangfireId { get; set; }
    
    public DateTime? ExecuteUtc { get; set; }

    public Guid PlanId { get; set; }
    public Plan Plan { get; set; }
}