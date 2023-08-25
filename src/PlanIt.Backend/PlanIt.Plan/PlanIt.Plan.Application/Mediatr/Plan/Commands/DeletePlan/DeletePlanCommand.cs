using MediatR;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.Plan.Commands.DeletePlan;

public class DeletePlanCommand : IValidatableRequest<Result>
{
    public Guid PlanId { get; set; }

    public Guid UserId { get; set; }
}