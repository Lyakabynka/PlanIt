using System.Numerics;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Queries.GetPlanGroups;

public class PlanGroupVm
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    public int PlanCount { get; set; } 
}