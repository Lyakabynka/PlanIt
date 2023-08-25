using MediatR;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Queries.GetPlanGroups;

public class GetPlanGroupsCommandHandler : IRequestHandler<GetPlanGroupsCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public GetPlanGroupsCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(GetPlanGroupsCommand request, CancellationToken cancellationToken)
    {
        var planGroupsVm = await _dbContext.PlanGroups
            .Where(pg => pg.UserId == request.UserId)
            .Include(pg => pg.Plans)
            .Select(pg => new PlanGroupVm()
            {
                Id = pg.Id,
                Name = pg.Name,
                PlanCount = pg.Plans.Count
            })
            .ToListAsync(cancellationToken);

        return Result.Create(planGroupsVm);
    }
}