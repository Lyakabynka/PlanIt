using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PlanIt.Identity.Application.Features.Interfaces;
using PlanIt.Identity.Application.Mediatr.Results.Shared;
using PlanIt.Identity.Application.Response;

namespace PlanIt.Identity.Application.Mediatr.User.Queries;

public class GetUserProfileQuery : IValidatableRequest<Result>
{
    public Guid UserId { get; set; }
}