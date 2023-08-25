using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PlanIt.Identity.Application.Features.Interfaces;
using PlanIt.Identity.Application.Response;
using PlanIt.Identity.Application.Services;

namespace PlanIt.Identity.Application.Mediatr.Auth.Commands.Refresh;

public class RefreshCommand : IRequest<Result>
{
}