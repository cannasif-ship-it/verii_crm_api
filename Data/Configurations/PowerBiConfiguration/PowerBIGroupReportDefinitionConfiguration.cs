using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models.PowerBi;
using crm_api.Data.Configurations;

namespace crm_api.Data.Configurations.PowerBi
{
    public class PowerBIGroupReportDefinitionConfiguration : BaseEntityConfiguration<PowerBIGroupReportDefinition>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PowerBIGroupReportDefinition> builder)
        {
            builder.ToTable("RII_POWERBI_GROUP_REPORT_DEFINITIONS");

            builder.Property(e => e.GroupId)
                .IsRequired();

            builder.Property(e => e.ReportDefinitionId)
                .IsRequired();

            // Relations
            builder.HasOne(e => e.Group)
                .WithMany()
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ReportDefinition)
                .WithMany()
                .HasForeignKey(e => e.ReportDefinitionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique mapping: prevent duplicate
            builder.HasIndex(e => new { e.GroupId, e.ReportDefinitionId })
                .IsUnique()
                .HasDatabaseName("UX_PowerBIGroupReportDefinitions_Group_Report");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_PowerBIGroupReportDefinitions_IsDeleted");

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
