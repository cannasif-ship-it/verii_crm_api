using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models.PowerBi;
using crm_api.Data.Configurations;

namespace crm_api.Data.Configurations.PowerBi
{
    public class PowerBIGroupConfiguration : BaseEntityConfiguration<PowerBIGroup>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PowerBIGroup> builder)
        {
            builder.ToTable("RII_POWERBI_GROUPS");

            builder.Property(e => e.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(e => e.Description)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Unique group name (opsiyonel ama tavsiye)
            builder.HasIndex(e => e.Name)
                .IsUnique()
                .HasDatabaseName("UX_PowerBIGroups_Name");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_PowerBIGroups_IsDeleted");

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
