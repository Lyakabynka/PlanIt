namespace PlanIt.Worker.Application.Configurations;

public class RabbitMqQueuesConfiguration
{
    public static readonly string RabbitMqQueuesSection = "RabbitMq_Queues";
    
    public string ScheduledPlanTriggered { get; set; }
    
    public string ScheduledPlanProcessed { get; set; }
}