using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using cms_webapi.Models;

namespace cms_webapi.Data.Configurations
{
    public class ActivityTypeConfiguration : BaseEntityConfiguration<ActivityType>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ActivityType> builder)
        {
            // Table name
            builder.ToTable("RII_ACTIVITY_TYPE");

            // Properties
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            builder.Property(e => e.Description)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            // Note: Activity model uses ActivityType as string, not foreign key
            // Navigation property is for reference only

            // Indexes
            builder.HasIndex(e => e.Name)
                .HasDatabaseName("IX_ActivityType_Name");

            builder.HasIndex(e => e.CreatedDate)
                .HasDatabaseName("IX_ActivityType_CreatedDate");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_ActivityType_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
