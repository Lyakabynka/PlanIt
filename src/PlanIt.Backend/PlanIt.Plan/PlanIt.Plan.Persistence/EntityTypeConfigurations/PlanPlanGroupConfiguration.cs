using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanIt.Plan.Domain.Entities;

namespace PlanIt.Plan.Persistence.EntityTypeConfigurations;

public class PlanPlanGroupConfiguration : IEntityTypeConfiguration<PlanPlanGroup>
{
    public void Configure(EntityTypeBuilder<PlanPlanGroup> builder)
    {
        builder.HasKey(ppg => new { ppg.PlanId, ppg.PlanGroupId });

        builder.HasOne(ppg => ppg.Plan)
            .WithMany(p => p.PlanPlanGroups)
            .HasForeignKey(ppg => ppg.PlanId);

        builder.HasOne(ppg => ppg.PlanGroup)
            .WithMany(pg => pg.PlanPlanGroups)
            .HasForeignKey(ppg => ppg.PlanGroupId);
    }
}