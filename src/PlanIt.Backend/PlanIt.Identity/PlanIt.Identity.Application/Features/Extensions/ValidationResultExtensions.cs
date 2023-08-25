using FluentValidation.Results;
using PlanIt.Identity.Application.Response;

namespace PlanIt.Identity.Application.Features.Extensions;

public static class ValidationResultExtensions
{
    public static IEnumerable<ValidationError> ToValidationErrors(this ValidationResult result)
    {
        return result.Errors
            .Select(error => new ValidationError(error.PropertyName, error.ErrorMessage));
    }
}