using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using PlanIt.Plan.Application.Interfaces;
using PlanIt.Plan.Application.Mediator.Plan.Notifications;
using PlanIt.Plan.Application.Mediator.Results;

namespace PlanIt.Plan.Application.Mediator.Plan.Commands;

public class ScheduleInstantPlanCommand : IRequest<OneOf<Success, NotFound, Forbidden>>
{
    public Guid PlanId { get; set; }

    public Guid UserId { get; set; }
}

public class ScheduleInstantPlanCommandHandler :
    IRequestHandler<ScheduleInstantPlanCommand, OneOf<Success, NotFound, Forbidden>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IPublisher _publisher;

    public ScheduleInstantPlanCommandHandler(IApplicationDbContext dbContext, IPublisher publisher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
    }

    public async Task<OneOf<Success, NotFound, Forbidden>> Handle(ScheduleInstantPlanCommand request,
        CancellationToken cancellationToken)
    {
        var plan = await _dbContext.Plans
            .Where(plan => plan.Id == request.PlanId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (plan is null) return new NotFound();

        if (plan.UserId != request.UserId) return new Forbidden();

        await _publisher.Publish(new PlanTransmissionRequested
        {
            Plan = plan
        }, cancellationToken);

        return new Success();
    }
}