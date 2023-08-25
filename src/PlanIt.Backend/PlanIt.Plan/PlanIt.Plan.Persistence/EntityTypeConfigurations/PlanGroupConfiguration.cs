using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanIt.Plan.Domain.Entities;

namespace PlanIt.Plan.Persistence.EntityTypeConfigurations;

public class PlanGroupConfiguration : IEntityTypeConfiguration<PlanGroup>
{
    public void Configure(EntityTypeBuilder<PlanGroup> builder)
    {
        builder.HasIndex(pg => pg.Id);
        builder
            .HasMany(pg => pg.Plans)
            .WithMany(p => p.PlanGroups);
    }
}