namespace PlanIt.Plan.Application.Configurations;

public class RabbitMqSetupConfiguration
{
    public static readonly string RabbitMqSetupSection = "RabbitMq_Setup";
    
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}