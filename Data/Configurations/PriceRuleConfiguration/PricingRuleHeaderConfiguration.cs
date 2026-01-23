using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models;

namespace crm_api.Data.Configurations
{
    public class PricingRuleHeaderConfiguration : BaseEntityConfiguration<PricingRuleHeader>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PricingRuleHeader> builder)
        {
            // Table name
            builder.ToTable("RII_PRICING_RULE_HEADER");

            // Rule Type (enum)
            builder.Property(e => e.RuleType)
                .IsRequired()
                .HasConversion<int>();

            // Rule Code
            builder.Property(e => e.RuleCode)
                .HasMaxLength(50)
                .IsRequired();

            // Rule Name
            builder.Property(e => e.RuleName)
                .HasMaxLength(250)
                .IsRequired();

            // Valid From/To dates
            builder.Property(e => e.ValidFrom)
                .IsRequired();

            builder.Property(e => e.ValidTo)
                .IsRequired();

            // Customer relationship
            builder.Property(e => e.CustomerId)
                .IsRequired(false);

            builder.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            // ERP Customer Code
            builder.Property(e => e.ErpCustomerCode)
                .HasMaxLength(50)
                .IsRequired(false);

            // Branch Code
            builder.Property(e => e.BranchCode)
                .IsRequired(false);

            // Price Includes VAT
            builder.Property(e => e.PriceIncludesVat)
                .IsRequired()
                .HasDefaultValue(false);

            // Is Active
            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Navigation properties
            builder.HasMany(e => e.Lines)
                .WithOne(l => l.PricingRuleHeader)
                .HasForeignKey(l => l.PricingRuleHeaderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Salesmen)
                .WithOne(s => s.PricingRuleHeader)
                .HasForeignKey(s => s.PricingRuleHeaderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(e => e.RuleCode)
                .HasDatabaseName("IX_PricingRuleHeader_RuleCode");

            builder.HasIndex(e => e.CustomerId)
                .HasDatabaseName("IX_PricingRuleHeader_CustomerId");

            builder.HasIndex(e => e.RuleType)
                .HasDatabaseName("IX_PricingRuleHeader_RuleType");

            builder.HasIndex(e => e.IsActive)
                .HasDatabaseName("IX_PricingRuleHeader_IsActive");

            builder.HasIndex(e => e.ValidFrom)
                .HasDatabaseName("IX_PricingRuleHeader_ValidFrom");

            builder.HasIndex(e => e.ValidTo)
                .HasDatabaseName("IX_PricingRuleHeader_ValidTo");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_PricingRuleHeader_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
