using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models;

namespace crm_api.Data.Configurations
{
    public class ReportTemplateConfiguration : IEntityTypeConfiguration<ReportTemplate>
    {
        public void Configure(EntityTypeBuilder<ReportTemplate> builder)
        {
            builder.ToTable("RII_REPORT_TEMPLATES");

            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.RuleType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(rt => rt.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(rt => rt.TemplateJson)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(rt => rt.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(rt => rt.Default)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasIndex(rt => new { rt.RuleType, rt.Default })
                .HasDatabaseName("IX_RII_REPORT_TEMPLATES_RuleType_Default");

            builder.Property(rt => rt.CreatedByUserId)
                .IsRequired(false);

            builder.Property(rt => rt.UpdatedByUserId)
                .IsRequired(false);

            // Indexes
            builder.HasIndex(rt => rt.RuleType)
                .HasDatabaseName("IX_RII_REPORT_TEMPLATES_RuleType");

            builder.HasIndex(rt => rt.IsActive)
                .HasDatabaseName("IX_RII_REPORT_TEMPLATES_IsActive");

            builder.HasIndex(rt => new { rt.RuleType, rt.IsActive })
                .HasDatabaseName("IX_RII_REPORT_TEMPLATES_RuleType_IsActive");
        }
    }
}
