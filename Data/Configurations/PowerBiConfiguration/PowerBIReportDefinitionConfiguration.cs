using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models.PowerBi;
using crm_api.Data.Configurations;

namespace crm_api.Data.Configurations.PowerBi
{
    public class PowerBIReportDefinitionConfiguration : BaseEntityConfiguration<PowerBIReportDefinition>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PowerBIReportDefinition> builder)
        {
            builder.ToTable("RII_POWERBI_REPORT_DEFINITIONS");

            builder.Property(e => e.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(e => e.WorkspaceId)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.ReportId)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.EmbedUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(e => e.DefaultSettingsJson)
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            builder.Property(e => e.ContentType)
                .IsRequired();

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Unique: same report should not be defined twice
            builder.HasIndex(e => new { e.WorkspaceId, e.ReportId })
                .IsUnique()
                .HasDatabaseName("UX_PowerBIReportDefinitions_Workspace_Report");

            // Indexes
            builder.HasIndex(e => e.Name)
                .HasDatabaseName("IX_PowerBIReportDefinitions_Name");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_PowerBIReportDefinitions_IsDeleted");

            // Soft delete filter
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
