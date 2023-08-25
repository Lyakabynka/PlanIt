using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanIt.Identity.Domain.Entities;
using PlanIt.Identity.Domain.Enums;

namespace PlanIt.Identity.Persistence.EntityTypeConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Role)
            .HasConversion(
                r => r.ToString(),
                r => Enum.Parse<UserRole>(r));
    }
}
