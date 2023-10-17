using MediatR;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.ScheduledPlan.Queries.GetScheduledPlans;

public class GetPlanScheduleVmQueryHandler : IRequestHandler<GetPlanScheduleVmQuery,Result>
{
    private readonly IApplicationDbContext _dbContext;

    public GetPlanScheduleVmQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(GetPlanScheduleVmQuery request, CancellationToken cancellationToken)
    {
        var planVm = await _dbContext.Plans
            .Where(plan => plan.Id == request.PlanId)
            .Select(p=>new
            {
                Id = p.Id,
                UserId = p.UserId
            })
            .FirstAsync(cancellationToken);
        
        if (planVm.UserId != request.UserId)
            return Result.FormForbidden();

        var planScheduleVm = await _dbContext.Plans
            .Where(p => p.Id == request.PlanId)
            .Include(p => p.ScheduledPlans)
            .Select(p => new PlanScheduleVm()
            {
                Id = p.Id,
                Name = p.Name,
                ScheduledPlans = p.ScheduledPlans
                    .Select(sp => new ScheduledPlanVm()
                    {
                        Id = sp.Id,
                        Type = sp.Type,
                        ExecuteUtc = sp.ExecuteUtc,
                        CronExpressionUtc = sp.CronExpressionUtc,
                    }).ToList()
            })
            .FirstAsync(cancellationToken);

        return Result.Create(planScheduleVm);
    }
}