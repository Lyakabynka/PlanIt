using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Queries.GetPlanGroups;

public class GetPlanGroupVmsCommand : IValidatableRequest<Result>
{
    public Guid UserId { get; set; }
}