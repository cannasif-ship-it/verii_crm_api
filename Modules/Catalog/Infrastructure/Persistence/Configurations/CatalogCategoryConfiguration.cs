using CatalogCategoryEntity = crm_api.Modules.Catalog.Domain.Entities.CatalogCategory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Modules.Catalog.Infrastructure.Persistence.Configurations
{
    public class CatalogCategoryConfiguration : BaseEntityConfiguration<CatalogCategoryEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<CatalogCategoryEntity> builder)
        {
            builder.ToTable("RII_CATALOG_CATEGORY");

            builder.Property(x => x.SortOrder).HasDefaultValue(0).IsRequired();
            builder.Property(x => x.IsRoot).HasDefaultValue(false).IsRequired();

            builder.HasOne(x => x.Catalog)
                .WithMany(x => x.CatalogCategories)
                .HasForeignKey(x => x.CatalogId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.CatalogCategories)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ParentCatalogCategory)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentCatalogCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.CatalogId, x.CategoryId }).IsUnique().HasDatabaseName("UX_CatalogCategory_CatalogId_CategoryId");
            builder.HasIndex(x => x.ParentCatalogCategoryId).HasDatabaseName("IX_CatalogCategory_ParentCatalogCategoryId");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_CatalogCategory_IsDeleted");
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
