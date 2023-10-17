using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PlanIt.Plan.Domain.Entities;

namespace PlanIt.Plan.Application.Features.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Domain.Entities.Plan> Plans { get; set; }
    DbSet<ScheduledPlan> ScheduledPlans { get; set; }
    DbSet<PlanGroup> PlanGroups { get; set; }
    DbSet<PlanPlanGroup> PlanPlanGroups { get; set; }
    DbSet<ScheduledPlanGroup> ScheduledPlanGroups { get; set; }
    
    DatabaseFacade Database { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
