using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanIt.Plan.Domain.Entities;

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

        builder.HasMany(p => p.RecurringPlans)
            .WithOne(rp => rp.Plan)
            .HasForeignKey(rp => rp.PlanId)
            .OnDelete(DeleteBehavior.Cascade);
        
        
    }
}