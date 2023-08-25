using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlanIt.Plan.Application.Features.Extensions;

namespace PlanIt.Plan.Application.Response;

public class Result : ActionResult
{
    private readonly int _statusCode;
    private readonly object _data;

    private Result(object data, int statusCode)
    {
        _data = data;
        _statusCode = statusCode;
    }

    public override async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;

        response.StatusCode = this._statusCode;

        await response.WriteAsJsonAsync(_data, JsonSerializerOptions);
    }

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };
    
    public static Result FormBadRequest(ValidationResult validationResult)
    {
        var problemDetails = new ProblemDetails()
        {
            Status = StatusCodes.Status406NotAcceptable,
            Type = "ValidationFailure",
            Title = "Validation error",
            Detail = "One or more validation errors has occurred"
        };

        if (validationResult.Errors is not null)
        {
            problemDetails.Extensions["errors"] = validationResult.ToValidationErrors();
        }

        return new Result(problemDetails, StatusCodes.Status406NotAcceptable);
    }
    
    public static Result Create(object data, int statusCode = StatusCodes.Status200OK)
    {
        return new Result(data, statusCode);
    }

    public static Result FormNotFound(string detail = "Entity was not found")
    {
        var problemDetails = new ProblemDetails()
        {
            Status = StatusCodes.Status404NotFound,
            Type = "NotFound",
            Title = "Not Found",
            Detail = detail
        };

        return new Result(problemDetails, StatusCodes.Status404NotFound);
    }
    
    public static Result FormUnauthorized()
    {
        var problemDetails = new ProblemDetails()
        {
            Status = StatusCodes.Status401Unauthorized,
            Type = "Unauthorized",
            Title = "Unauthorized",
            Detail = "User is not authorized"
        };

        return new Result(problemDetails, StatusCodes.Status401Unauthorized);
    }

    public static Result FormForbidden(string detail = "User does not have required permission")
    {
        var problemDetails = new ProblemDetails()
        {
            Status = StatusCodes.Status403Forbidden,
            Type = "Forbidden",
            Title = "Forbidden",
            Detail = detail,
        };

        return new Result(problemDetails, StatusCodes.Status403Forbidden);
    }

    public static Result Create(string type, string title, string detail, int statusCode, ValidationError? error = null)
    {
        var problemDetails = new ProblemDetails()
        {
            Status = statusCode,
            Type = type,
            Title = title,
            Detail = detail
        };

        if (error is not null)
        {
            problemDetails.Extensions["errors"] = new[] { error };
        }

        return new Result(problemDetails, statusCode);
    }
}