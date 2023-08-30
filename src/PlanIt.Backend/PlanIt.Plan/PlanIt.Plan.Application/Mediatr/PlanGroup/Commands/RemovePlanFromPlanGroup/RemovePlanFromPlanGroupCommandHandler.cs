using MediatR;
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
        
    }
}