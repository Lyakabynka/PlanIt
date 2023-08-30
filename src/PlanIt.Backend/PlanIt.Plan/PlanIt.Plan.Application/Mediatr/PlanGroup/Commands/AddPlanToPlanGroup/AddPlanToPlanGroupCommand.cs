using MediatR;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Commands.AddPlanToPlanGroup;

public class AddPlanToPlanGroupCommand : IRequest<Result>
{
    public Guid PlanId { get; set; }
    public Guid PlanGroupId { get; set; }
    
    public Guid UserId { get; set; }
}