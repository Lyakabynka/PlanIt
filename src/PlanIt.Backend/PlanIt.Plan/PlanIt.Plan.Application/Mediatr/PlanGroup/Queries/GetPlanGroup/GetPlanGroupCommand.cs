using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Queries.GetPlanGroup;

public class GetPlanGroupCommand : IValidatableRequest<Result>
{
    public Guid PlanGroupId { get; set; }
    public Guid UserId { get; set; }
}