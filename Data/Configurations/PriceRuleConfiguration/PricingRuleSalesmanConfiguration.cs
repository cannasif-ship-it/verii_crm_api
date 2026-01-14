using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using cms_webapi.Models;

namespace cms_webapi.Data.Configurations
{
    public class PricingRuleSalesmanConfiguration : BaseEntityConfiguration<PricingRuleSalesman>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PricingRuleSalesman> builder)
        {
            // Table name
            builder.ToTable("RII_PRICING_RULE_SALESMAN");

            // Pricing Rule Header relationship
            builder.Property(e => e.PricingRuleHeaderId)
                .IsRequired();

            builder.HasOne(e => e.PricingRuleHeader)
                .WithMany(h => h.Salesmen)
                .HasForeignKey(e => e.PricingRuleHeaderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Salesman relationship
            builder.Property(e => e.SalesmanId)
                .IsRequired();

            builder.HasOne(e => e.Salesman)
                .WithMany()
                .HasForeignKey(e => e.SalesmanId)
                .OnDelete(DeleteBehavior.NoAction);

            // Indexes
            builder.HasIndex(e => e.PricingRuleHeaderId)
                .HasDatabaseName("IX_PricingRuleSalesman_PricingRuleHeaderId");

            builder.HasIndex(e => e.SalesmanId)
                .HasDatabaseName("IX_PricingRuleSalesman_SalesmanId");

            // Unique constraint: A salesman can only be assigned once per pricing rule header
            builder.HasIndex(e => new { e.PricingRuleHeaderId, e.SalesmanId })
                .IsUnique()
                .HasDatabaseName("IX_PricingRuleSalesman_Header_Salesman_Unique");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_PricingRuleSalesman_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
