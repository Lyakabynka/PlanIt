using PlanIt.Plan.Domain.Enums;

namespace PlanIt.RabbitMq;

public class ScheduledPlanTriggered
{
    public Guid ScheduledPlanId { get; set; }
    public ScheduledPlanType ScheduledPlanType { get; set; }
    
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    public string Information { get; set; }
    public string? ExecutionPath { get; set; }
    public PlanType PlanType { get; set; }
    
    public Guid UserId { get; set; }
}