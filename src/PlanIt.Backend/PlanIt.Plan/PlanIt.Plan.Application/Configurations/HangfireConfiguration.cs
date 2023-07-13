namespace PlanIt.Plan.Application.Configurations;

public class HangfireConfiguration
{
    public static readonly string HangfireSection = "Hangfire";
    public string ConnectionString { get; set; }
}