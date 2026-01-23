using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models;

namespace crm_api.Data.Configurations
{
    public class DistrictConfiguration : BaseEntityConfiguration<District>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<District> builder)
        {
            // Table name
            builder.ToTable("RII_DISTRICT");

            // Properties
            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.ERPCode)
                .HasMaxLength(10)
                .IsRequired(false);

            builder.Property(e => e.CityId)
                .IsRequired();

            // Foreign Key Relationships
            builder.HasOne(e => e.City)
                .WithMany(c => c.Districts)
                .HasForeignKey(e => e.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            // Indexes
            builder.HasIndex(e => e.Name)
                .HasDatabaseName("IX_District_Name");

            builder.HasIndex(e => e.ERPCode)
                .IsUnique()
                .HasFilter("[ERPCode] IS NOT NULL")
                .HasDatabaseName("IX_District_ERPCode");

            builder.HasIndex(e => e.CityId)
                .HasDatabaseName("IX_District_CityId");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_District_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
