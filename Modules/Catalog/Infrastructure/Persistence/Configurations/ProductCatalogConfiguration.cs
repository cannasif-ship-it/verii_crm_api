using CatalogEntity = crm_api.Modules.Catalog.Domain.Entities.ProductCatalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Modules.Catalog.Infrastructure.Persistence.Configurations
{
    public class ProductCatalogConfiguration : BaseEntityConfiguration<CatalogEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<CatalogEntity> builder)
        {
            builder.ToTable("RII_PRODUCT_CATALOG");

            builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500).IsRequired(false);
            builder.Property(x => x.CatalogType).IsRequired();
            builder.Property(x => x.BranchCode).IsRequired(false);
            builder.Property(x => x.SortOrder).HasDefaultValue(0).IsRequired();

            builder.HasMany(x => x.CatalogCategories)
                .WithOne(x => x.Catalog)
                .HasForeignKey(x => x.CatalogId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Code).IsUnique().HasDatabaseName("UX_ProductCatalog_Code");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_ProductCatalog_IsDeleted");
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
