using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OneOf;
using PlanIt.Plan.Application.Interfaces;
using PlanIt.Plan.Application.Mediator.Results;

namespace PlanIt.Plan.Application.Mediator.Plan.Queries;

public class GetPlansQuery : IRequest<OneOf<Success<List<Domain.Entities.Plan>>, NotFound>>
{
    public Guid UserId { get; set; }
}

public class GetPlansQueryHandler :
    IRequestHandler<GetPlansQuery, OneOf<Success<List<Domain.Entities.Plan>>, NotFound>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetPlansQueryHandler(IApplicationDbContext dbContext) =>
        (_dbContext) = (dbContext);

    public async Task<
        OneOf<Success<List<Domain.Entities.Plan>>, NotFound>> Handle(GetPlansQuery request,
        CancellationToken cancellationToken)
    {
        var plans = await _dbContext.Plans
            .Where(plan => plan.UserId == request.UserId)
            .Include(plan => plan.ScheduledPlans)
            .Include(plan => plan.RecurringPlans)
            .ToListAsync(cancellationToken);

        return plans.Count == 0
            ? new NotFound()
            : new Success<List<Domain.Entities.Plan>>(plans);
    }
}