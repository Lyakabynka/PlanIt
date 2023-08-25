using MediatR;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.Application.Mediatr.Plan.Commands.UpdatePlan;

public class UpdatePlanCommand : IValidatableRequest<Result>
{
    public Guid PlanId { get; set; }
    public string Name { get; set; }
    public string Information { get; set; }
    public string? ExecutionPath { get; set; }
    public PlanType Type { get; set; }

    public Guid UserId { get; set; }
}

