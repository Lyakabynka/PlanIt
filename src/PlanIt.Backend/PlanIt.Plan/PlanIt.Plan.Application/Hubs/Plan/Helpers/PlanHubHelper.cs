using Microsoft.AspNetCore.SignalR;

namespace PlanIt.Plan.Application.Hubs.Plan.Helpers;

public class PlanHubHelper
{
    private readonly IHubContext<PlanHub> _hubContext;
    public PlanHubHelper(IHubContext<PlanHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task TransmitPlanToClient(Domain.Entities.Plan plan, CancellationToken cancellationToken)
    {
        await _hubContext.Clients
            .Group(plan.UserId.ToString())
            .SendAsync("ProcessPlan", plan, cancellationToken);
    }
}