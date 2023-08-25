using MediatR;

namespace PlanIt.Plan.Application.Features.Interfaces;

public interface IValidatableRequest<out TResponse> : IRequest<TResponse>
{
}