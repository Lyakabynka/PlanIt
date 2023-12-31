﻿using MassTransit;
using Microsoft.AspNetCore.SignalR;
using PlanIt.Worker.Application.Hubs;
using PlanIt.Worker.Domain.Enums;

namespace PlanIt.Messaging;

public class ScheduledPlanTriggered
{
    public Guid ScheduledPlanId { get; set; }
    public ScheduleType ScheduleType { get; set; }

    public PlanVm Plan { get; set; }

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
        // Sending 'PlanVm' to clients with the same UserId connected to SignalR hub
        // in order to process the plan on the client side
        await _hubContext.Clients.Group(context.Message.UserId.ToString())
            .SendAsync("ProcessPlan", context.Message.Plan, context.CancellationToken);

        await _bus.Send(new ScheduledPlanProcessed()
        {
            ScheduledPlanId = context.Message.ScheduledPlanId,
            ScheduleType = context.Message.ScheduleType
        });
    }
}