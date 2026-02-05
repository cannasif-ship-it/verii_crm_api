using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models.PowerBi;
using crm_api.Data.Configurations;

namespace crm_api.Data.Configurations.PowerBi
{
    public class PowerBIConfigurationConfiguration : BaseEntityConfiguration<PowerBIConfiguration>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PowerBIConfiguration> builder)
        {
            builder.ToTable("RII_POWERBI_CONFIGURATION");

            builder.Property(e => e.TenantId)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.ClientId)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.WorkspaceId)
                .IsRequired();

            builder.Property(e => e.ApiBaseUrl)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(e => e.Scope)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_PowerBIConfiguration_IsDeleted");

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
