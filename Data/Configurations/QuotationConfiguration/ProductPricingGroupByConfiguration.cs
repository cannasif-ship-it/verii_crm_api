using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using cms_webapi.Models;

namespace cms_webapi.Data.Configurations
{
    public class ProductPricingGroupByConfiguration : BaseEntityConfiguration<ProductPricingGroupBy>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ProductPricingGroupBy> builder)
        {
            builder.ToTable("RII_PRODUCT_PRICING_GROUP_BY");

            builder.Property(e => e.ErpGroupCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Currency)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.ListPrice)
                .HasColumnType("decimal(18,6)")
                .IsRequired();

            builder.Property(e => e.CostPrice)
                .HasColumnType("decimal(18,6)")
                .IsRequired();

            builder.Property(e => e.Discount1)
                .HasColumnType("decimal(18,6)")
                .IsRequired(false);

            builder.Property(e => e.Discount2)
                .HasColumnType("decimal(18,6)")
                .IsRequired(false);

            builder.Property(e => e.Discount3)
                .HasColumnType("decimal(18,6)")
                .IsRequired(false);

            builder.HasIndex(e => e.ErpGroupCode)
                .HasDatabaseName("IX_ProductPricingGroupBy_ErpGroupCode")
                .IsUnique();

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_ProductPricingGroupBy_IsDeleted");

            builder.HasIndex(e => e.CreatedBy)
                .HasDatabaseName("IX_RII_PRODUCT_PRICING_GROUP_BY_CreatedBy");

            builder.HasIndex(e => e.UpdatedBy)
                .HasDatabaseName("IX_RII_PRODUCT_PRICING_GROUP_BY_UpdatedBy");

            builder.HasIndex(e => e.DeletedBy)
                .HasDatabaseName("IX_RII_PRODUCT_PRICING_GROUP_BY_DeletedBy");

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
