using MassTransit;
using Microsoft.AspNetCore.SignalR;
using PlanIt.Worker.Application.Hubs;
using PlanIt.Worker.Domain.Enums;

namespace PlanIt.RabbitMq;

public class ScheduledPlanTriggered
{
    public Guid ScheduledPlanId { get; set; }
    
    
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    public string Information { get; set; }
    public string? ExecutionPath { get; set; }
    public PlanType Type { get; set; }
    
    public Guid UserId { get; set; }
}

public class ScheduledPlanTriggeredConsumer : IConsumer<ScheduledPlanTriggered>
{
    private readonly IHubContext<PlanHub> _hubContext;

    public ScheduledPlanTriggeredConsumer(IHubContext<PlanHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<ScheduledPlanTriggered> context)
    {
        await _hubContext.Clients.Group(context.Message.UserId.ToString())
            .SendAsync("ProcessPlan", context.Message, context.CancellationToken);
    }
}