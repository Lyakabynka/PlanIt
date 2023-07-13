using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Interfaces;
using PlanIt.Plan.Domain.Entities;
using PlanIt.Plan.Persistence.EntityTypeConfigurations;

namespace PlanIt.Plan.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Domain.Entities.Plan> Plans { get; set; }
        public DbSet<OneOffPlan> OneOffPlans { get; set; }
        public DbSet<RecurringPlan> RecurringPlans { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PlanConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
