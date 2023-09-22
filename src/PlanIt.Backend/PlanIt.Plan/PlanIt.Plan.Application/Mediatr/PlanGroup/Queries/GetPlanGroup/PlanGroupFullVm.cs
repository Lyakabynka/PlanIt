namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Queries.GetPlanGroup;

public class PlanGroupFullVm
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<PlanPlanGroupVm> PlanPlanGroups { get; set; }
}