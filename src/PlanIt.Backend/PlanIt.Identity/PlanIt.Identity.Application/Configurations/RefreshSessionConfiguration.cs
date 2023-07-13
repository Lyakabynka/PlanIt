namespace PlanIt.Identity.Application.Configurations;

public class RefreshSessionConfiguration
{
    public static readonly string RefreshSessionSection = "RefreshSession";
    
    public string RefreshCookieName { get; set; }
    public int HoursToExpiration { get; set; }
}