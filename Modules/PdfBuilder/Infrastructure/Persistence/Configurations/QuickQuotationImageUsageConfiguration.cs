using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Modules.PdfBuilder.Infrastructure.Persistence.Configurations
{
    public class QuickQuotationImageUsageConfiguration : IEntityTypeConfiguration<QuickQuotationImageUsage>
    {
        public void Configure(EntityTypeBuilder<QuickQuotationImageUsage> builder)
        {
            builder.ToTable("RII_QUICK_QUOTATION_IMAGE_USAGES");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProductCode)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(x => x.TempQuotattion)
                .WithMany()
                .HasForeignKey(x => x.TempQuotattionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TempQuotattionLine)
                .WithMany()
                .HasForeignKey(x => x.TempQuotattionLineId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.QuickQuotationImageId)
                .HasDatabaseName("IX_RII_QUICK_QUOTATION_IMAGE_USAGES_ImageId");

            builder.HasIndex(x => x.TempQuotattionLineId)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0")
                .HasDatabaseName("IX_RII_QUICK_QUOTATION_IMAGE_USAGES_LineId");
        }
    }
}
