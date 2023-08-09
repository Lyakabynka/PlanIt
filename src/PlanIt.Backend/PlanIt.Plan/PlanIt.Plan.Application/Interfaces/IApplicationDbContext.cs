using Microsoft.EntityFrameworkCore;
using PlanIt.Plan.Domain.Entities;

namespace PlanIt.Plan.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Domain.Entities.Plan> Plans { get; set; }
    DbSet<ScheduledPlan> ScheduledPlans { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
