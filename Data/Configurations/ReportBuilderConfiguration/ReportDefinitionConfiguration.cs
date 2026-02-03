using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models.ReportBuilder;

namespace crm_api.Data.Configurations.ReportBuilderConfiguration
{
    public class ReportDefinitionConfiguration : BaseEntityConfiguration<ReportDefinition>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ReportDefinition> builder)
        {
            builder.ToTable("RII_REPORT_DEFINITIONS");

            builder.Property(r => r.Name).IsRequired().HasMaxLength(200);
            builder.Property(r => r.Description).IsRequired(false).HasMaxLength(500);
            builder.Property(r => r.ConnectionKey).IsRequired().HasMaxLength(20);
            builder.Property(r => r.DataSourceType).IsRequired().HasMaxLength(20);
            builder.Property(r => r.DataSourceName).IsRequired().HasMaxLength(128);
            builder.Property(r => r.ConfigJson).IsRequired().HasColumnType("nvarchar(max)");

            builder.HasIndex(r => new { r.CreatedBy, r.Name })
                .HasDatabaseName("IX_RII_REPORT_DEFINITIONS_CreatedBy_Name")
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.HasIndex(r => new { r.ConnectionKey, r.DataSourceType, r.DataSourceName })
                .HasDatabaseName("IX_RII_REPORT_DEFINITIONS_ConnectionKey_DataSourceType_DataSourceName");
        }
    }
}
