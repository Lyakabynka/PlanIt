namespace PlanIt.Plan.Application.Configurations;

public class RabbitMqQueuesConfiguration
{
    public static readonly string RabbitMqQueuesSection = "RabbitMq_Queues";
    
    public string InstantPlanTriggered { get; set; }
    public string OneOffPlanTriggered { get; set; }
    public string RecurringPlanTriggered { get; set; }
}