using MediatR;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;
using PlanIt.Plan.Domain.Entities;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Commands.SetPlansToPlanGroup;

public class SetPlansToPlanGroupCommandHandler : IRequestHandler<SetPlansToPlanGroupCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public SetPlansToPlanGroupCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(SetPlansToPlanGroupCommand request, CancellationToken cancellationToken)
    {
        var planGroup = await _dbContext.PlanGroups
            .Where(pg => pg.Id == request.PlanGroupId)
            .FirstAsync(cancellationToken);

        if (planGroup.UserId != request.UserId)
            return Result.FormForbidden();
        
        //Check for using someone else's plan
        var planIds = request.SetPlanToPlanGroupRequestModels
            .Select(rm => rm.PlanId).ToList();
        var atLeastOnePlanForbidden = await _dbContext.Plans
            .Where(plan => planIds.Contains(plan.Id) 
                           && plan.UserId != request.UserId)
            .AnyAsync(cancellationToken);
        
        if (atLeastOnePlanForbidden)
            return Result.FormForbidden();

        //Delete existing PlanPlanGroups
        await _dbContext.PlanPlanGroups
            .Where(ppg => ppg.PlanGroupId == request.PlanGroupId)
            .ExecuteDeleteAsync(cancellationToken);
        
        //Converting models into actual PlanPlanGroups
        var planPlanGroups = request.SetPlanToPlanGroupRequestModels
            .Select(m =>
                new PlanPlanGroup()
                {
                    Index = m.Index,
                    PlanId = m.PlanId,
                    PlanGroupId = planGroup.Id,
                    UserId = request.UserId
                })
            .ToList();
        //Inserting Generated PlanPlanGroups
        await _dbContext.PlanPlanGroups.AddRangeAsync(
            planPlanGroups,
            cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Create(new { });
    }
}