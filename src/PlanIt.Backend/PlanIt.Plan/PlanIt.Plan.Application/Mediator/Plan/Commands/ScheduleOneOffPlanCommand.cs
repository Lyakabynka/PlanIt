﻿using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using PlanIt.Plan.Application.Interfaces;
using PlanIt.Plan.Application.Mediator.Plan.Notifications;
using PlanIt.Plan.Application.Mediator.Results;
using PlanIt.Plan.Domain.Entities;


namespace PlanIt.Plan.Application.Mediator.Plan.Commands;

public class ScheduleOneOffPlanCommand : IRequest<OneOf<Success, NotFound, Forbidden, BadRequest>>
{
    public Guid PlanId { get; set; }

    public DateTime ExecuteUtc { get; set; }

    public Guid UserId { get; set; }
}

public class
    SchedulePlanCommandHandler : IRequestHandler<ScheduleOneOffPlanCommand,
        OneOf<Success, NotFound, Forbidden, BadRequest>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IBackgroundJobClientV2 _backgroundJobClient;
    private readonly IPublisher _publisher;

    public SchedulePlanCommandHandler(
        IApplicationDbContext dbContext, IBackgroundJobClientV2 backgroundJobClient, IPublisher publisher)
    {
        _dbContext = dbContext;
        _backgroundJobClient = backgroundJobClient;
        _publisher = publisher;
    }

    public async Task<OneOf<Success, NotFound, Forbidden, BadRequest>> Handle(ScheduleOneOffPlanCommand request,
        CancellationToken cancellationToken)
    {
        var plan = await _dbContext.Plans
            .Where(plan => plan.Id == request.PlanId)
            .AsTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (plan is null) return new NotFound();

        if (plan.UserId != request.UserId) return new Forbidden();

        //service call
        var oneOffPlanId = Guid.NewGuid();
        var hangfireId = _backgroundJobClient.Schedule(
            () => _publisher.Publish(
                new PlanTransmissionRequested
                {
                    OneOffPlanId = oneOffPlanId,
                    Plan = plan
                }, cancellationToken),
            request.ExecuteUtc.ToLocalTime());

        //needs to be deleted once executed in hangfire
        _dbContext.OneOffPlans.Add(new OneOffPlan
        {
            Id = oneOffPlanId,
            HangfireId = hangfireId,
            ExecuteUtc = request.ExecuteUtc,
            PlanId = plan.Id,
        });

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}