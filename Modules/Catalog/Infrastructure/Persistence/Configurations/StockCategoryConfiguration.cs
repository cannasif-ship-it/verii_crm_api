using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockCategoryEntity = crm_api.Modules.Catalog.Domain.Entities.StockCategory;

namespace crm_api.Modules.Catalog.Infrastructure.Persistence.Configurations
{
    public class StockCategoryConfiguration : BaseEntityConfiguration<StockCategoryEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<StockCategoryEntity> builder)
        {
            builder.ToTable("RII_STOCK_CATEGORY");

            builder.Property(x => x.AssignmentType).IsRequired();
            builder.Property(x => x.IsPrimary).HasDefaultValue(true).IsRequired();
            builder.Property(x => x.SortOrder).HasDefaultValue(0).IsRequired();
            builder.Property(x => x.Note).HasMaxLength(500).IsRequired(false);

            builder.HasOne(x => x.Stock)
                .WithMany()
                .HasForeignKey(x => x.StockId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.StockCategories)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Rule)
                .WithMany(x => x.StockCategories)
                .HasForeignKey(x => x.RuleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.StockId, x.CategoryId }).IsUnique().HasDatabaseName("UX_StockCategory_StockId_CategoryId");
            builder.HasIndex(x => x.CategoryId).HasDatabaseName("IX_StockCategory_CategoryId");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_StockCategory_IsDeleted");
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
