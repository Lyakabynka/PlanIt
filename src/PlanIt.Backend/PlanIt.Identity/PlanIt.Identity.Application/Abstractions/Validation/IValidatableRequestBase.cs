using MediatR;

namespace PlanIt.Identity.Application.Abstractions.Validation;

public interface IValidatableRequest : IRequest, IValidatableRequestBase { }

public interface IValidatableRequest<TResponse> : IRequest<TResponse>, IValidatableRequestBase { }

public interface IValidatableRequestBase { }