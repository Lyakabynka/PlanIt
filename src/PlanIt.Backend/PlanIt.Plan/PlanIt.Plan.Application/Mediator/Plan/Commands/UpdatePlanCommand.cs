using System.Security.Claims;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OneOf;
using PlanIt.Plan.Application.Interfaces;
using PlanIt.Plan.Application.Mediator.Results;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.Application.Mediator.Plan.Commands;

public class UpdatePlanCommand : IRequest<OneOf<Success<Domain.Entities.Plan>, NotFound, Forbidden>>
{
    public Guid PlanId { get; set; }
    public string Name { get; set; }
    public string Information { get; set; }
    public string? ExecutionPath { get; set; }
    public PlanType Type { get; set; }

    public Guid UserId { get; set; }
}

public class UpdatePlanCommandHandler : IRequestHandler<UpdatePlanCommand, OneOf<Success<Domain.Entities.Plan>, NotFound, Forbidden>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IBackgroundJobClientV2 _backgroundJobClient;
    private readonly IRecurringJobManagerV2 _recurringJobClient;

    public UpdatePlanCommandHandler(IApplicationDbContext dbContext, IBackgroundJobClientV2 backgroundJobClient,
        IRecurringJobManagerV2 recurringJobClient)
    {
        _dbContext = dbContext;
        _backgroundJobClient = backgroundJobClient;
        _recurringJobClient = recurringJobClient;
    }

    public async Task<OneOf<Success<Domain.Entities.Plan>, NotFound, Forbidden>> Handle(UpdatePlanCommand request,
        CancellationToken cancellationToken)
    {
        var plan = await _dbContext.Plans
            .Where(plan => plan.Id == request.PlanId)
            .Include(plan=>plan.ScheduledPlans)
            .Include(plan=>plan.RecurringPlans)
            .AsTracking()
            .FirstOrDefaultAsync(cancellationToken);
        if (plan is null) return new NotFound();

        if (plan.UserId != request.UserId) return new Forbidden();

        plan.Name = request.Name;
        plan.Information = request.Information;
        plan.ExecutionPath = request.ExecutionPath;
        plan.Type = request.Type;

        // deleting scheduled/recurring jobs from hangfire
        plan.ScheduledPlans?.ForEach(sp=>
            _backgroundJobClient.Delete(sp.Id.ToString()));
        plan.RecurringPlans?.ForEach(rp => 
            _recurringJobClient.RemoveIfExists(rp.Id.ToString()));

        await _dbContext.SaveChangesAsync(cancellationToken);

        plan.ScheduledPlans = null!;
        plan.RecurringPlans = null!;
        
        return new Success<Domain.Entities.Plan>(plan);
    }
}