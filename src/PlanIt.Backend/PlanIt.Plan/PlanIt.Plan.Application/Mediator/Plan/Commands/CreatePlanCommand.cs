using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using OneOf;
using PlanIt.Plan.Application.Interfaces;
using PlanIt.Plan.Application.Mediator.Results;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.Application.Mediator.Plan.Commands;

public class CreatePlanCommand : IRequest<OneOf<Success<Domain.Entities.Plan>>>
{
    public string Name { get; set; }
    public string Information { get; set; }
    public string? ExecutionPath { get; set; }
    public PlanType Type { get; set; }

    public Guid UserId { get; set; }
}

public class CreatePlanCommandHandler : IRequestHandler<CreatePlanCommand, OneOf<Success<Domain.Entities.Plan>>>
{
    private readonly IApplicationDbContext _dbContext;

    public CreatePlanCommandHandler(IApplicationDbContext dbContext) =>
        (_dbContext) = (dbContext);

    public async Task<OneOf<Success<Domain.Entities.Plan>>> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
    {
        var plan = new Domain.Entities.Plan()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Information = request.Information,
            ExecutionPath = request.ExecutionPath,
            Type = request.Type,
            UserId = request.UserId
        };

        _dbContext.Plans.Add(plan);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Success<Domain.Entities.Plan>(plan);
    }
}