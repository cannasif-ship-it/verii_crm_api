using CategoryEntity = crm_api.Modules.Catalog.Domain.Entities.ProductCategory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Modules.Catalog.Infrastructure.Persistence.Configurations
{
    public class ProductCategoryConfiguration : BaseEntityConfiguration<CategoryEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("RII_PRODUCT_CATEGORY");

            builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500).IsRequired(false);
            builder.Property(x => x.Level).HasDefaultValue(1).IsRequired();
            builder.Property(x => x.FullPath).HasMaxLength(1000).IsRequired(false);
            builder.Property(x => x.SortOrder).HasDefaultValue(0).IsRequired();
            builder.Property(x => x.VisualPreset).HasDefaultValue(Domain.Enums.CategoryVisualPresetType.General).IsRequired();
            builder.Property(x => x.ImageUrl).HasMaxLength(500).IsRequired(false);
            builder.Property(x => x.IconName).HasMaxLength(100).IsRequired(false);
            builder.Property(x => x.ColorHex).HasMaxLength(20).IsRequired(false);
            builder.Property(x => x.IsLeaf).HasDefaultValue(false).IsRequired();

            builder.HasOne(x => x.ParentCategory)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.CatalogCategories)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.StockCategories)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.CategoryRules)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Code).IsUnique().HasDatabaseName("UX_ProductCategory_Code");
            builder.HasIndex(x => x.ParentCategoryId).HasDatabaseName("IX_ProductCategory_ParentCategoryId");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_ProductCategory_IsDeleted");
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
