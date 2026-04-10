using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCategoryRuleEntity = crm_api.Modules.Catalog.Domain.Entities.ProductCategoryRule;

namespace crm_api.Modules.Catalog.Infrastructure.Persistence.Configurations
{
    public class ProductCategoryRuleConfiguration : BaseEntityConfiguration<ProductCategoryRuleEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ProductCategoryRuleEntity> builder)
        {
            builder.ToTable("RII_PRODUCT_CATEGORY_RULE");

            builder.Property(x => x.RuleName).HasMaxLength(150).IsRequired();
            builder.Property(x => x.RuleCode).HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.StockAttributeType).IsRequired();
            builder.Property(x => x.OperatorType).IsRequired();
            builder.Property(x => x.Value).HasMaxLength(250).IsRequired();
            builder.Property(x => x.Priority).HasDefaultValue(0).IsRequired();

            builder.HasOne(x => x.Category)
                .WithMany(x => x.CategoryRules)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.CategoryId).HasDatabaseName("IX_ProductCategoryRule_CategoryId");
            builder.HasIndex(x => x.Priority).HasDatabaseName("IX_ProductCategoryRule_Priority");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_ProductCategoryRule_IsDeleted");
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
