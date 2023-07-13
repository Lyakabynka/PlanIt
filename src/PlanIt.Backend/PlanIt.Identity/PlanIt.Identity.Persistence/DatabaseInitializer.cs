using Microsoft.EntityFrameworkCore;

namespace PlanIt.Identity.Persistence;

public class DatabaseInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();
        context.Database.Migrate();
    }
}
