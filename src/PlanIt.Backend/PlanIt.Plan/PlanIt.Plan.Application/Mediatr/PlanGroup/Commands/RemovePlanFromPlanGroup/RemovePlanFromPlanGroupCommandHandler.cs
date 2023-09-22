using MediatR;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Commands.RemovePlanFromPlanGroup;

public class RemovePlanFromPlanGroupCommandHandler : IRequestHandler<RemovePlanFromPlanGroupCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public RemovePlanFromPlanGroupCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(RemovePlanFromPlanGroupCommand request, CancellationToken cancellationToken)
    {
        var planGroupVm = await _dbContext.PlanGroups
            .Where(pg => pg.Id == request.PlanGroupId)
            .Select(p=>new
            {
                Id = p.Id,
                UserId = p.UserId
            })
            .FirstAsync(cancellationToken);

        if (planGroupVm.UserId != request.UserId)
            return Result.FormForbidden();
        
        var planVm = await _dbContext.Plans
            .Where(p => p.Id == request.PlanId)
            .Select(p=>new
            {
                Id = p.Id,
                UserId = p.UserId
            })
            .FirstAsync(cancellationToken);
        
        if (planVm.UserId != request.UserId)
            return Result.FormForbidden();

        await _dbContext.PlanPlanGroups
            .Where(ppg => ppg.PlanId == planVm.Id && ppg.PlanGroupId == planGroupVm.Id)
            .ExecuteDeleteAsync(cancellationToken);
        
        return Result.Create(new {});
    }
}