using MediatR;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.ScheduledPlan.Commands.DeleteScheduledPlan;

public class DeleteScheduledPlanCommand : IValidatableRequest<Result>
{
    public Guid ScheduledPlanId { get; set; }

    public Guid UserId { get; set; }
}