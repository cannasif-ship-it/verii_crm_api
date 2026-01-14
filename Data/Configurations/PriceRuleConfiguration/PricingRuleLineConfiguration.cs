using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using cms_webapi.Models;

namespace cms_webapi.Data.Configurations
{
    public class PricingRuleLineConfiguration : BaseEntityConfiguration<PricingRuleLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PricingRuleLine> builder)
        {
            // Table name
            builder.ToTable("RII_PRICING_RULE_LINE");

            // Pricing Rule Header relationship
            builder.Property(e => e.PricingRuleHeaderId)
                .IsRequired();

            builder.HasOne(e => e.PricingRuleHeader)
                .WithMany(h => h.Lines)
                .HasForeignKey(e => e.PricingRuleHeaderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Stok Code
            builder.Property(e => e.StokCode)
                .HasMaxLength(50)
                .IsRequired();

            // Min Quantity
            builder.Property(e => e.MinQuantity)
                .HasColumnType("decimal(18,6)")
                .IsRequired();

            // Max Quantity
            builder.Property(e => e.MaxQuantity)
                .HasColumnType("decimal(18,6)")
                .IsRequired(false);

            // Fixed Unit Price
            builder.Property(e => e.FixedUnitPrice)
                .HasColumnType("decimal(18,6)")
                .IsRequired(false);

            // Currency Code
            builder.Property(e => e.CurrencyCode)
                .HasMaxLength(10)
                .IsRequired()
                .HasDefaultValue("TRY");

            // Discount Rate 1
            builder.Property(e => e.DiscountRate1)
                .HasColumnType("decimal(18,6)")
                .IsRequired()
                .HasDefaultValue(0m);

            // Discount Amount 1
            builder.Property(e => e.DiscountAmount1)
                .HasColumnType("decimal(18,6)")
                .IsRequired()
                .HasDefaultValue(0m);

            // Discount Rate 2
            builder.Property(e => e.DiscountRate2)
                .HasColumnType("decimal(18,6)")
                .IsRequired()
                .HasDefaultValue(0m);

            // Discount Amount 2
            builder.Property(e => e.DiscountAmount2)
                .HasColumnType("decimal(18,6)")
                .IsRequired()
                .HasDefaultValue(0m);

            // Discount Rate 3
            builder.Property(e => e.DiscountRate3)
                .HasColumnType("decimal(18,6)")
                .IsRequired()
                .HasDefaultValue(0m);

            // Discount Amount 3
            builder.Property(e => e.DiscountAmount3)
                .HasColumnType("decimal(18,6)")
                .IsRequired()
                .HasDefaultValue(0m);

            // Indexes
            builder.HasIndex(e => e.PricingRuleHeaderId)
                .HasDatabaseName("IX_PricingRuleLine_PricingRuleHeaderId");

            builder.HasIndex(e => e.StokCode)
                .HasDatabaseName("IX_PricingRuleLine_StokCode");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_PricingRuleLine_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
