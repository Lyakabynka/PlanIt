namespace PlanIt.Plan.RestAPI.Models;

public class AddPlanToPlanGroupRequestModel
{
    public Guid PlanId { get; set; }
    public Guid PlanGroupId { get; set; }
}