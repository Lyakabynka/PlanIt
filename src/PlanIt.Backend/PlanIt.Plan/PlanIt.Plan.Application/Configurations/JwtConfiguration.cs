namespace PlanIt.Plan.Application.Configurations;

public class JwtConfiguration
{
    public static readonly string JwtSection = "Jwt";
    
    public string JwtCookieName { get; set; }
    
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int MinutesToExpiration { get; set; }
}