using Microsoft.EntityFrameworkCore;
using PlanIt.Identity.Domain.Entities;

namespace PlanIt.Identity.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<UserData> UserDatas { get; set; }
    DbSet<RefreshSession> RefreshSessions { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
