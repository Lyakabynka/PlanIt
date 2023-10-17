using MassTransit;
using Microsoft.AspNetCore.SignalR;
using PlanIt.Worker.Application.Hubs;
using PlanIt.Worker.Domain.Enums;

namespace PlanIt.Messaging;

public class ScheduledPlanGroupTriggered
{
    public Guid ScheduledPlanGroupId { get; set; }
    public ScheduleType ScheduleType { get; set; }

    public Guid PlanGroupId { get; set; }
    
    public List<PlanPlanGroupVm> PlanPlanGroups { get; set; }
    
    public Guid UserId { get; set; }
}

public class ScheduledPlanGroupTriggeredConsumer : IConsumer<ScheduledPlanGroupTriggered>
{
    private readonly IHubContext<PlanHub> _hubContext;
    private readonly IBus _bus;

    public ScheduledPlanGroupTriggeredConsumer(IHubContext<PlanHub> hubContext, IBus bus)
    {
        _hubContext = hubContext;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<ScheduledPlanGroupTriggered> context)
    {
        // Sending 'PlanVm' to clients with the same UserId connected to SignalR hub
        // in order to process the plan on the client side
        await _hubContext.Clients.Group(context.Message.UserId.ToString())
            .SendAsync("ProcessPlanGroup", context.Message.PlanPlanGroups, context.CancellationToken);

        await _bus.Send(new ScheduledPlanGroupProcessed()
        {
            ScheduledPlanGroupId = context.Message.ScheduledPlanGroupId,
            ScheduleType = context.Message.ScheduleType
        });
    }
}