namespace PlanIt.Worker.Application.Configurations;

public class RabbitMqConfiguration
{
    public static readonly string RabbitMqSection = "RabbitMq";
    
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}