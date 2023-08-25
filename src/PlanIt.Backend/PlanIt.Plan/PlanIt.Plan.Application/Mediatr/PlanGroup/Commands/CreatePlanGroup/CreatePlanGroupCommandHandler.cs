using MediatR;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Commands.CreatePlanGroup;

public class CreatePlanGroupCommandHandler : IRequestHandler<CreatePlanGroupCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public CreatePlanGroupCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(CreatePlanGroupCommand request, CancellationToken cancellationToken)
    {
        var planGroup = new Domain.Entities.PlanGroup()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            UserId = request.UserId
        };

        _dbContext.PlanGroups.Add(planGroup);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Create(planGroup);
    }
}