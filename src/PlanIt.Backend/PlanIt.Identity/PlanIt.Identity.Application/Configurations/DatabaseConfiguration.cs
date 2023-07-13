namespace PlanIt.Identity.Application.Configurations
{
    public class DatabaseConfiguration
    {
        public static readonly string DatabaseSection = "Database";
        public string ConnectionString { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
