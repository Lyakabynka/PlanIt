using MediatR;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Commands.RemovePlanFromPlanGroup;

public class RemovePlanFromPlanGroupCommand : IValidatableRequest<Result>
{
    public Guid PlanId { get; set; }
    public Guid PlanGroupId { get; set; }
    
    public Guid UserId { get; set; }
}