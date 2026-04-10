using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Modules.PdfBuilder.Infrastructure.Persistence.Configurations
{
    public class QuickQuotationImageConfiguration : IEntityTypeConfiguration<QuickQuotationImage>
    {
        public void Configure(EntityTypeBuilder<QuickQuotationImage> builder)
        {
            builder.ToTable("RII_QUICK_QUOTATION_IMAGES");

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
                .HasDatabaseName("IX_RII_QUICK_QUOTATION_IMAGES_RelativeUrl");

            builder.HasIndex(x => x.CreatedBy)
                .HasDatabaseName("IX_RII_QUICK_QUOTATION_IMAGES_CreatedBy");
        }
    }
}
