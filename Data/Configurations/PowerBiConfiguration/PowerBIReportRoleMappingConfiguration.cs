using crm_api.Data.Configurations;
using crm_api.Models.PowerBi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Data.Configurations.PowerBi
{
    public class PowerBIReportRoleMappingConfiguration : BaseEntityConfiguration<PowerBIReportRoleMapping>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PowerBIReportRoleMapping> builder)
        {
            builder.ToTable("RII_POWERBI_REPORT_ROLE_MAPPINGS");

            builder.Property(e => e.PowerBIReportDefinitionId)
                .IsRequired();

            builder.Property(e => e.RoleId)
                .IsRequired();

            builder.Property(e => e.RlsRoles)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            builder.HasOne(e => e.ReportDefinition)
                .WithMany()
                .HasForeignKey(e => e.PowerBIReportDefinitionId);

            builder.HasOne(e => e.Role)
                .WithMany()
                .HasForeignKey(e => e.RoleId);

            builder.HasIndex(e => new { e.PowerBIReportDefinitionId, e.RoleId })
                .IsUnique()
                .HasDatabaseName("UX_PowerBIReportRoleMappings_Report_Role");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_PowerBIReportRoleMappings_IsDeleted");

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
