using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models;

namespace crm_api.Data.Configurations
{
    public class DemandLineConfiguration : BaseEntityConfiguration<DemandLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<DemandLine> builder)
        {
            // Table
            builder.ToTable("RII_DEMAND_LINE");

            // Properties
            builder.Property(e => e.ProductCode)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(e => e.RelatedProductKey)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(e => e.ErpProjectCode)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(e => e.Description1)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(e => e.Description2)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(e => e.Description3)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(e => e.IsMainRelatedProduct)
                .IsRequired()
                .HasDefaultValue(false);

            // Relationships
            builder.Property(e => e.DemandId)
                .IsRequired();

            builder.HasOne(e => e.PricingRuleHeader)
                .WithMany()
                .HasForeignKey(e => e.PricingRuleHeaderId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.RelatedStock)
                .WithMany()
                .HasForeignKey(e => e.RelatedStockId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes for common queries
            builder.HasIndex(e => e.DemandId)
                .HasDatabaseName("IX_DemandLine_DemandId");

            builder.HasIndex(e => e.ProductCode)
                .HasDatabaseName("IX_DemandLine_ProductCode");

            builder.HasIndex(e => e.PricingRuleHeaderId)
                .HasDatabaseName("IX_DemandLine_PricingRuleHeaderId");

            builder.HasIndex(e => e.RelatedStockId)
                .HasDatabaseName("IX_DemandLine_RelatedStockId");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_DemandLine_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
