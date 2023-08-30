using MediatR;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.PlanGroup.Commands.AddPlanToPlanGroup;

public class AddPlanToPlanGroupCommandHandler : IRequestHandler<AddPlanToPlanGroupCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public AddPlanToPlanGroupCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(AddPlanToPlanGroupCommand request, CancellationToken cancellationToken)
    {
        var planGroup = await _dbContext.PlanGroups
            .AsTracking()
            .Where(pg => pg.Id == request.PlanGroupId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (planGroup is null)
            return Result.FormNotFound("PlanGroup does not exist");

        if (planGroup.UserId != request.UserId)
            return Result.FormForbidden();
        
        var plan = await _dbContext.Plans
            .Where(plan => plan.Id == request.PlanId)
            .FirstOrDefaultAsync(cancellationToken);

        if (plan is null)
            return Result.FormNotFound("Plan does not exist");

        if (plan.UserId != request.UserId)
            return Result.FormForbidden();
        
        planGroup.Plans.Add(plan);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Create(new {});
    }
}