using crm_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Data.Configurations
{
    public class PdfTemplateAssetConfiguration : IEntityTypeConfiguration<PdfTemplateAsset>
    {
        public void Configure(EntityTypeBuilder<PdfTemplateAsset> builder)
        {
            builder.ToTable("RII_PDF_TEMPLATE_ASSETS");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OriginalFileName)
                .IsRequired()
                .HasMaxLength(260);

            builder.Property(x => x.StoredFileName)
                .IsRequired()
                .HasMaxLength(260);

            builder.Property(x => x.RelativeUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.ContentType)
                .IsRequired()
                .HasMaxLength(120);

            builder.Property(x => x.SizeBytes)
                .IsRequired();

            builder.HasIndex(x => x.RelativeUrl)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0")
                .HasDatabaseName("IX_RII_PDF_TEMPLATE_ASSETS_RelativeUrl");

            builder.HasIndex(x => x.CreatedBy)
                .HasDatabaseName("IX_RII_PDF_TEMPLATE_ASSETS_CreatedBy");
        }
    }
}
