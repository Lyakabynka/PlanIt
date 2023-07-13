using Microsoft.EntityFrameworkCore;

namespace PlanIt.Plan.Persistence;

public class DatabaseInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();
        context.Database.Migrate();
    }
}
