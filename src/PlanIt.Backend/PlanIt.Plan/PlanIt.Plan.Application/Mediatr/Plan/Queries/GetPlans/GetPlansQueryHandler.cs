using MediatR;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Mediatr.Plan.Queries.GetPlans;

public class GetPlansQueryHandler : IRequestHandler<GetPlansQuery, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public GetPlansQueryHandler(IApplicationDbContext dbContext) =>
        (_dbContext) = (dbContext);

    public async Task<Result> Handle(GetPlansQuery request, CancellationToken cancellationToken)
    {
        var plans = await _dbContext.Plans
            .Where(plan => plan.UserId == request.UserId)
            .Select(plan => new PlanVm()
            {
                Id = plan.Id,
                Name = plan.Name,
                Information = plan.Information,
                ExecutionPath = plan.ExecutionPath,
                Type = plan.Type
            })
            .ToListAsync(cancellationToken);

        return Result.Create(plans);
    }
}