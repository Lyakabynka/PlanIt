using MediatR;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Mediatr.PlanGroup.ViewModels;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Queries.GetPlanGroups;

public class GetPlanGroupVmsCommandHandler : IRequestHandler<GetPlanGroupVmsCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public GetPlanGroupVmsCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(GetPlanGroupVmsCommand request, CancellationToken cancellationToken)
    {
        var planGroupsVm = await _dbContext.PlanGroups
            .Where(pg => pg.UserId == request.UserId)
            .Include(pg => pg.PlanPlanGroups)
            .Select(pg => new PlanGroupVm()
            {
                Id = pg.Id,
                Name = pg.Name,
                PlanCount = pg.PlanPlanGroups.Count
            })
            .ToListAsync(cancellationToken);

        return Result.Create(planGroupsVm);
    }
}