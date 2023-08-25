using Microsoft.EntityFrameworkCore;
using PlanIt.Identity.Domain.Entities;

namespace PlanIt.Identity.Application.Features.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }

    DbSet<RefreshSession> RefreshSessions { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
