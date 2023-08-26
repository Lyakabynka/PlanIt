using MediatR;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.ScheduledPlan.Queries.GetScheduledPlans;

public class GetScheduledPlansQueryHandler : IRequestHandler<GetScheduledPlansQuery,Result>
{
    private readonly IApplicationDbContext _dbContext;

    public GetScheduledPlansQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(GetScheduledPlansQuery request, CancellationToken cancellationToken)
    {
        var plan = await _dbContext.Plans
            .Where(plan => plan.Id == request.PlanId)
            .FirstOrDefaultAsync(cancellationToken);

        if (plan is null)
            return Result.FormNotFound("Plan does not exist");

        if (plan.UserId != request.UserId)
            return Result.FormForbidden();
        
        var scheduledPlans = await _dbContext.ScheduledPlans
            .Where(sp => sp.PlanId == request.PlanId)
            .Select(sp => new ScheduledPlanVm()
            {
                Id = sp.Id,
                Type = sp.Type,
                ExecuteUtc = sp.ExecuteUtc,
                CronExpressionUtc = sp.CronExpressionUtc
            })
            .ToListAsync(cancellationToken);

        return Result.Create(scheduledPlans);
    }
}