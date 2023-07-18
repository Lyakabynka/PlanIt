using MassTransit;
using Microsoft.AspNetCore.SignalR;
using PlanIt.Worker.Application.Hubs;
using PlanIt.Worker.Domain.Enums;

// namespace PlanIt.Worker.Application.RabbitMQ.Consume;
namespace Consume;

public class InstantPlanTriggered
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    public string Information { get; set; }
    public string? ExecutionPath { get; set; }
    public PlanType Type { get; set; }
    
    public Guid UserId { get; set; }
}

public class InstantPlanTriggeredConsumer : IConsumer<InstantPlanTriggered>
{
    private readonly IHubContext<PlanHub> _hubContext;

    public InstantPlanTriggeredConsumer(IHubContext<PlanHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<InstantPlanTriggered> context)
    {
        await _hubContext.Clients.Group(context.Message.UserId.ToString())
            .SendAsync("ProcessPlan");
    }
}