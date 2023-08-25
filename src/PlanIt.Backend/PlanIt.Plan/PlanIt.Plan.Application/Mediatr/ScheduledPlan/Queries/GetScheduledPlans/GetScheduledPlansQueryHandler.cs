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