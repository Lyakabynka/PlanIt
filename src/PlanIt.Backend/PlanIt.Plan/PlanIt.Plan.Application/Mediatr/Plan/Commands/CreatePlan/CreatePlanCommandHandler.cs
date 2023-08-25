using MediatR;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.Plan.Commands.CreatePlan;

public class CreatePlanCommandHandler : IRequestHandler<CreatePlanCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public CreatePlanCommandHandler(IApplicationDbContext dbContext) =>
        (_dbContext) = (dbContext);

    public async Task<Result> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
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

        return Result.Create(plan);
    }
}