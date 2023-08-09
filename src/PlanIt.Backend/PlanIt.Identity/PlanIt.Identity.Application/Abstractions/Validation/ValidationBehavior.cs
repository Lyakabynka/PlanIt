using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using PlanIt.Identity.Application.Mediator.Results;

namespace PlanIt.Identity.Application.Abstractions.Validation;

// public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//     where TRequest : IValidatableRequestBase
// {
//     private readonly IEnumerable<IValidator<TRequest>> _validators;
//
//     public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
//     {
//         _validators = validators;
//     }
//
//     public async Task<TResponse> Handle(TRequest request,
//         RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
//     {
//         var context = new ValidationContext<TRequest>(request);
//
//         var validationResults = 
//             await Task.WhenAll(_validators
//             .Select(validator => validator.ValidateAsync(context, cancellationToken)));
//         
//         var errors = 
//             validationResults.Where(validatorResult => !validatorResult.IsValid)
//             .SelectMany(validationResult => validationResult.Errors)
//             .Select(validationFailure => new Error(validationFailure.PropertyName, validationFailure.ErrorMessage))
//             .ToList();
//
//         if (errors.Any())
//         {
//            var problemDetails = ProblemDetails.FormValidationProblemDetails()
//
//             return new BadRequest(ProblemDetails) as TResponse;
//         }
//
//         var result = await next();
//
//         return result;
//     }
// }