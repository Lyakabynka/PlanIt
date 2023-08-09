using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlanIt.Identity.Application.Abstractions.Validation;

namespace PlanIt.Identity.Application.Abstractions.Extensions;

// public static class ProblemDetailsExtensions
// {
//     public static ProblemDetails FormValidationError(this ProblemDetails pD, List<Error> errors)
//     {
//         var problemDetails = new ProblemDetails()
//         {
//             Status = StatusCodes.Status400BadRequest,
//             Type = "ValidationFailure",
//             Title = "Validation error",
//             Detail = "One or more validation errors has occurred"
//         };
//         
//         problemDetails.Extensions["errors"] = errors;
//
//         return problemDetails;
//     }
// }