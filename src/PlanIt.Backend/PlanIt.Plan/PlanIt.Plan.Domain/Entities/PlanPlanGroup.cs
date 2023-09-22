using System.ComponentModel.DataAnnotations.Schema;

namespace PlanIt.Plan.Domain.Entities;

public class PlanPlanGroup : BaseEntity
{
    public int Index { get; set; }
    
    public Guid PlanId { get; set; }
    public Plan Plan { get; set; }
    
    public Guid PlanGroupId { get; set; }
    public PlanGroup PlanGroup { get; set; }
    
    public Guid UserId { get; set; }
}