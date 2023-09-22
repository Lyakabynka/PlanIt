using MediatR;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Mediatr.Plan.Queries.GetPlans;
using PlanIt.Plan.Application.Mediatr.PlanGroup.Queries.GetPlanGroups;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Queries.GetPlanGroup;

public class GetPlanGroupCommandHandler : IRequestHandler<GetPlanGroupCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public GetPlanGroupCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(GetPlanGroupCommand request, CancellationToken cancellationToken)
    {
        var planGroupUserId = await _dbContext.PlanGroups
            .Where(pg => pg.Id == request.PlanGroupId)
            .Select(pg=>pg.UserId)
            .FirstAsync(cancellationToken);

        if (planGroupUserId  != request.UserId)
            return Result.FormForbidden();
        
        var planGroup = await _dbContext.PlanGroups
            .Where(pg => pg.Id == request.PlanGroupId)
            .Include(pg => pg.PlanPlanGroups)
            .ThenInclude(ppg=>ppg.Plan)
            .Select(pg => new PlanGroupFullVm()
            {
                Id = pg.Id,
                Name = pg.Name,
                PlanPlanGroups = pg.PlanPlanGroups
                    .OrderBy(ppg => ppg.Index)
                    .Select(ppg => new PlanPlanGroupVm()
                    {
                        Id = ppg.Id,
                        Index = ppg.Index,
                        Plan = new PlanVm()
                        {
                            Id = ppg.Plan.Id,
                            Name = ppg.Plan.Name,
                            Information = ppg.Plan.Information,
                            Type = ppg.Plan.Type,
                            ExecutionPath = ppg.Plan.ExecutionPath
                        }
                    }).ToList()
            })
            .FirstAsync(cancellationToken);

        return Result.Create(planGroup);
    }
}