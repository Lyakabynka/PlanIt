using MediatR;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Commands.DeletePlanGroup;

public class DeletePlanGroupCommandHandler : IRequestHandler<DeletePlanGroupCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public DeletePlanGroupCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(DeletePlanGroupCommand request, CancellationToken cancellationToken)
    {
        var planGroup = await _dbContext.PlanGroups
            .Where(pg => pg.Id == request.PlanGroupId)
            .FirstAsync(cancellationToken);

        if (planGroup.UserId != request.UserId)
            return Result.FormForbidden();

        _dbContext.PlanGroups.Remove(planGroup);

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Create(new { });
    }
}