using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Commands.SetPlansToPlanGroup;

public class SetPlansToPlanGroupCommand : IValidatableRequest<Result>
{
    public List<SetPlanToPlanGroupRequestModel> SetPlanToPlanGroupRequestModels { get; set; }
    public Guid PlanGroupId { get; set; }
    
    public Guid UserId { get; set; }
}
