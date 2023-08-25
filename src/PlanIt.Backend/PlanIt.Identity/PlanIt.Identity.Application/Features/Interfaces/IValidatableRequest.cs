using MediatR;

namespace PlanIt.Identity.Application.Features.Interfaces;

public interface IValidatableRequest<out TResponse> : IRequest<TResponse>
{
}