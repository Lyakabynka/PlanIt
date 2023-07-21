using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using PlanIt.Plan.Application.Interfaces;
using PlanIt.Plan.Application.Mediator.Results;
using PlanIt.RabbitMq;

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
    private readonly IPublishHelper _publishHelper;

    public ScheduleInstantPlanCommandHandler(IApplicationDbContext dbContext, IPublishHelper publishHelper)
    {
        _dbContext = dbContext;
        _publishHelper = publishHelper;
    }

    public async Task<OneOf<Success, NotFound, Forbidden>> Handle(ScheduleInstantPlanCommand request,
        CancellationToken cancellationToken)
    {
        var plan = await _dbContext.Plans
            .Where(plan => plan.Id == request.PlanId)
            .FirstOrDefaultAsync(cancellationToken);

        if (plan is null) return new NotFound();

        if (plan.UserId != request.UserId) return new Forbidden();
        
        //method which sends message to determined queue ( recurring plan queue )
        await _publishHelper.PublishInstantPlanTriggered(new InstantPlanTriggered
        {
            Id = plan.Id,
            Name = plan.Name,
            Information = plan.Information,
            ExecutionPath = plan.ExecutionPath,
            Type = plan.Type,
            UserId = plan.UserId
        }, cancellationToken);

        return new Success();
    }
}