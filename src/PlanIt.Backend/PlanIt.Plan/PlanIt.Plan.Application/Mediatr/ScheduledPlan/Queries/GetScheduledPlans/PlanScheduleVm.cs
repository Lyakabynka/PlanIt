namespace PlanIt.Plan.Application.Mediatr.ScheduledPlan.Queries.GetScheduledPlans;

public class PlanScheduleVm
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public List<ScheduledPlanVm> ScheduledPlans { get; set; }
}