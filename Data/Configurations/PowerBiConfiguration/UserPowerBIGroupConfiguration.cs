using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models.PowerBi;
using crm_api.Data.Configurations;
using crm_api.Models; // User class burada diye varsaydım

namespace crm_api.Data.Configurations.PowerBi
{
    public class UserPowerBIGroupConfiguration : BaseEntityConfiguration<UserPowerBIGroup>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<UserPowerBIGroup> builder)
        {
            builder.ToTable("RII_USER_POWERBI_GROUPS");

            builder.Property(e => e.UserId)
                .IsRequired();

            builder.Property(e => e.GroupId)
                .IsRequired();

            // Relations
            builder.HasOne(e => e.User)
                .WithMany() // User tarafında collection yoksa böyle
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Group)
                .WithMany()
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique mapping: prevent duplicate
            builder.HasIndex(e => new { e.UserId, e.GroupId })
                .IsUnique()
                .HasDatabaseName("UX_UserPowerBIGroups_User_Group");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_UserPowerBIGroups_IsDeleted");

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
