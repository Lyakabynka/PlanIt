namespace PlanIt.Plan.Application.Configurations;

public class RabbitMqQueuesConfiguration
{
    public static readonly string RabbitMqQueuesSection = "RabbitMq_Queues";
    
    public string ScheduledPlanTriggered { get; set; }
    public string ScheduledPlanProcessed { get; set; }
    
    public string ScheduledPlanGroupTriggered { get; set; }
    public string ScheduledPlanGroupProcessed { get; set; }
}