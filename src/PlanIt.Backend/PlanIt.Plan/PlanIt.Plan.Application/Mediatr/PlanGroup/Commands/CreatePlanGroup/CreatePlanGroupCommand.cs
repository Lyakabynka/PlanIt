using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Commands.CreatePlanGroup;

public class CreatePlanGroupCommand : IValidatableRequest<Result>
{
    public string Name { get; set; }
    
    public Guid UserId { get; set; }
}