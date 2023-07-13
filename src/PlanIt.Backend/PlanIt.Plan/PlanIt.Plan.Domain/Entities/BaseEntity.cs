using System.ComponentModel.DataAnnotations;

namespace PlanIt.Plan.Domain.Entities;

public class BaseEntity
{
    [Key] public Guid Id { get; set; }
    
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;
}