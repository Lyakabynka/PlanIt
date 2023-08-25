using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanIt.Plan.Domain.Entities;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.Persistence.EntityTypeConfigurations;

public class ScheduledPlanConfiguration : IEntityTypeConfiguration<ScheduledPlan>
{
    public void Configure(EntityTypeBuilder<ScheduledPlan> builder)
    {
        builder.HasKey(sp => sp.Id);
        builder.Property(sp => sp.Type)
            .HasConversion(
                t => t.ToString(),
                t => Enum.Parse<ScheduledPlanType>(t));
    }
}