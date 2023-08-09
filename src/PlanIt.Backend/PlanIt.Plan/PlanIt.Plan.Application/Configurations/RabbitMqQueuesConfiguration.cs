namespace PlanIt.Plan.Application.Configurations;

public class RabbitMqQueuesConfiguration
{
    public static readonly string RabbitMqQueuesSection = "RabbitMq_Queues";
    
    public string ScheduledPlanTriggered { get; set; }
}