using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Modules.PdfBuilder.Infrastructure.Persistence.Configurations
{
    public class PdfImageUsageConfiguration : IEntityTypeConfiguration<PdfImageUsage>
    {
        public void Configure(EntityTypeBuilder<PdfImageUsage> builder)
        {
            builder.ToTable("RII_PDF_IMAGE_USAGES");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ElementId)
                .IsRequired()
                .HasMaxLength(120);

            builder.Property(x => x.PageNumber)
                .IsRequired();

            builder.Property(x => x.RuleType)
                .IsRequired();

            builder.Property(x => x.UsageType)
                .IsRequired();

            builder.HasOne(x => x.ReportTemplate)
                .WithMany()
                .HasForeignKey(x => x.ReportTemplateId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.ReportTemplateId, x.ElementId, x.PageNumber })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0")
                .HasDatabaseName("IX_RII_PDF_IMAGE_USAGES_Template_Element_Page");

            builder.HasIndex(x => x.PdfTemplateAssetId)
                .HasDatabaseName("IX_RII_PDF_IMAGE_USAGES_ImageId");
        }
    }
}
