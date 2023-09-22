using MassTransit;
using Microsoft.AspNetCore.SignalR;
using PlanIt.Worker.Application.Hubs;
using PlanIt.Worker.Domain.Enums;

namespace PlanIt.RabbitMq;

public class ScheduledPlanTriggered
{
    public Guid ScheduledPlanId { get; set; }
    public ScheduleType ScheduleType { get; set; }

    public Guid Id { get; set; }

    public string Name { get; set; }
    public string Information { get; set; }
    public string? ExecutionPath { get; set; }
    public PlanType PlanType { get; set; }

    public Guid UserId { get; set; }
}

public class ScheduledPlanTriggeredConsumer : IConsumer<ScheduledPlanTriggered>
{
    private readonly IHubContext<PlanHub> _hubContext;
    private readonly IBus _bus;

    public ScheduledPlanTriggeredConsumer(IHubContext<PlanHub> hubContext, IBus bus)
    {
        _hubContext = hubContext;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<ScheduledPlanTriggered> context)
    {
        await _hubContext.Clients.Group(context.Message.UserId.ToString())
            .SendAsync("ProcessPlan", context.Message, context.CancellationToken);

        await _bus.Send(new ScheduledPlanProcessed()
        {
            ScheduledPlanId = context.Message.ScheduledPlanId
        });
    }
}