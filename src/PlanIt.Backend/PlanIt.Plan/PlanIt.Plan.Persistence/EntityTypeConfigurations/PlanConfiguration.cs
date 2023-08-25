using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanIt.Plan.Domain.Entities;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.Persistence.EntityTypeConfigurations;

public class PlanConfiguration : IEntityTypeConfiguration<Domain.Entities.Plan>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Plan> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasMany(p => p.ScheduledPlans)
            .WithOne(op => op.Plan)
            .HasForeignKey(op => op.PlanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.Type)
            .HasConversion(
                t => t.ToString(),
                t => Enum.Parse<PlanType>(t));
    }
}