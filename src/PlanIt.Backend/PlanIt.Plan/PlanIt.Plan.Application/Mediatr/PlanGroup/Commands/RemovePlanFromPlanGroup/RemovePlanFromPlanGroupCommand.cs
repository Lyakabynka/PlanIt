using MediatR;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Commands.RemovePlanFromPlanGroup;

public class RemovePlanFromPlanGroupCommand : IRequest<Result>
{
    public Guid PlanId { get; set; }
    public Guid PlanGroupId { get; set; }
}