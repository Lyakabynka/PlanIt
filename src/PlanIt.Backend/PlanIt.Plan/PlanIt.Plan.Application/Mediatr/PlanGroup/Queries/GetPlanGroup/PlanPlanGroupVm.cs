using PlanIt.Plan.Application.Mediatr.Plan.Queries.GetPlans;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Queries.GetPlanGroup;

public class PlanPlanGroupVm
{
    public Guid Id { get; set; }
    
    public int Index { get; set; }
    
    public PlanVm Plan { get; set; } 
}