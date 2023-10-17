using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.ScheduledPlan.Queries.GetScheduledPlans;

public class GetPlanScheduleVmQuery : IValidatableRequest<Result>
{
    public Guid PlanId { get; set; }
    
    public Guid UserId { get; set; }
}