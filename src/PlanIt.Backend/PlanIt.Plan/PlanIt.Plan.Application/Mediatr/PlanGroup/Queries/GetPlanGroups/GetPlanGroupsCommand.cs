using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Queries.GetPlanGroups;

public class GetPlanGroupsCommand : IValidatableRequest<Result>
{
    public Guid UserId { get; set; }
}