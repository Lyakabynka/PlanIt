﻿using PlanIt.Worker.Domain.Enums;

namespace PlanIt.Messaging;

public class ScheduledPlanProcessed
{
    public Guid ScheduledPlanId { get; set; }
    public ScheduleType ScheduleType { get; set; }
}