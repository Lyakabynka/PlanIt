namespace PlanIt.Plan.Application.Configurations;

public class RabbitMQConfiguration
{
    public static readonly string RabbitMQSection = "RabbitMQ";
    public string PlanQueue { get; set; }
}