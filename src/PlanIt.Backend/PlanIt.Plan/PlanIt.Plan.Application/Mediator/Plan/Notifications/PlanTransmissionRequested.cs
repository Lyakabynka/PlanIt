using MediatR;
using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Hubs.Helpers;
using PlanIt.Plan.Application.Interfaces;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.Application.Mediator.Plan.Notifications;

public class PlanTransmissionRequested : INotification
{
    public Domain.Entities.Plan Plan { get; set; }

    public Guid? OneOffPlanId { get; set; }
}

public class PlanTransmissionRequestedHandler : INotificationHandler<PlanTransmissionRequested>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly PlanHubHelper _planHubHelper;

    public PlanTransmissionRequestedHandler(IApplicationDbContext dbContext, PlanHubHelper planHubHelper)
    {
        _dbContext = dbContext;
        _planHubHelper = planHubHelper;
    }

    public async Task Handle(PlanTransmissionRequested notification, CancellationToken cancellationToken)
    {
        await _planHubHelper.TransmitPlanToClient(notification.Plan, cancellationToken);
        
        if (notification.OneOffPlanId is not null)
        {
            await _dbContext.OneOffPlans
                .Where(sp => sp.Id == notification.OneOffPlanId)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}