using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanIt.Identity.Domain.Entities;

namespace PlanIt.Identity.Persistence.EntityTypeConfigurations;

public class UserDataConfiguration : IEntityTypeConfiguration<UserData>
{
    public void Configure(EntityTypeBuilder<UserData> builder)
    {
        builder.HasKey(ud => ud.Id);
    }
}