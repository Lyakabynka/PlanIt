using MediatR;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.Plan.Queries.GetPlans;

public class GetPlansQuery : IValidatableRequest<Result>
{
    public Guid UserId { get; set; }
}