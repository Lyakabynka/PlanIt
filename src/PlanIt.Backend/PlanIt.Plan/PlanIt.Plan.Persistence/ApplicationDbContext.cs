using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Domain.Entities;
using PlanIt.Plan.Persistence.EntityTypeConfigurations;

namespace PlanIt.Plan.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Domain.Entities.Plan> Plans { get; set; }
        public DbSet<ScheduledPlan> ScheduledPlans { get; set; }
        public DbSet<PlanGroup> PlanGroups { get; set; }
        public DbSet<PlanPlanGroup> PlanPlanGroups { get; set; }
        public DbSet<ScheduledPlanGroup> ScheduledPlanGroups { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PlanConfiguration());
            modelBuilder.ApplyConfiguration(new ScheduledPlanConfiguration());
            modelBuilder.ApplyConfiguration(new PlanGroupConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}