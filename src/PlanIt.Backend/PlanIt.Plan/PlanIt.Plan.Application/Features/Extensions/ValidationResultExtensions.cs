using FluentValidation.Results;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Features.Extensions;

public static class ValidationResultExtensions
{
    public static IEnumerable<ValidationError> ToValidationErrors(this ValidationResult result)
    {
        return result.Errors
            .Select(error => new ValidationError(error.PropertyName, error.ErrorMessage));
    }
}