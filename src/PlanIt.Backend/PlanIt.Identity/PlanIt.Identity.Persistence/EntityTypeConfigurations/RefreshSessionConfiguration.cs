using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanIt.Identity.Domain.Entities;

namespace PlanIt.Identity.Persistence.EntityTypeConfigurations;

public class RefreshSessionConfiguration : IEntityTypeConfiguration<RefreshSession>
{
    public void Configure(EntityTypeBuilder<RefreshSession> builder)
    {
        builder.HasKey(r => r.Id);
        builder.HasOne(r => r.User)
            .WithMany(u => u.RefreshSessions)
            .HasForeignKey(r => r.UserId);
    }
}
