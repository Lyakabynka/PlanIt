namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Commands.SetPlansToPlanGroup;

public class SetPlanToPlanGroupRequestModel
{
    public int Index { get; set; }
    
    public Guid PlanId { get; set; }
}