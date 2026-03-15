using crm_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Data.Configurations
{
    public class TempQuotattionConfiguration : BaseEntityConfiguration<TempQuotattion>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<TempQuotattion> builder)
        {
            builder.ToTable("RII_TEMP_QUOTATTION");

            builder.Property(e => e.CustomerId)
                .IsRequired();

            builder.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(e => e.RevisionId)
                .IsRequired(false);

            builder.HasOne(e => e.Revision)
                .WithMany(e => e.Revisions)
                .HasForeignKey(e => e.RevisionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(e => e.QuotationId)
                .IsRequired(false);

            builder.HasOne(e => e.Quotation)
                .WithMany()
                .HasForeignKey(e => e.QuotationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(e => e.QuotationNo)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(e => e.OfferDate)
                .IsRequired();

            builder.Property(e => e.CurrencyCode)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(e => e.ExchangeRate)
                .HasColumnType("decimal(18,6)")
                .IsRequired()
                .HasDefaultValue(1m);

            builder.Property(e => e.DiscountRate1)
                .HasColumnType("decimal(18,6)")
                .IsRequired()
                .HasDefaultValue(0m);

            builder.Property(e => e.DiscountRate2)
                .HasColumnType("decimal(18,6)")
                .IsRequired()
                .HasDefaultValue(0m);

            builder.Property(e => e.DiscountRate3)
                .HasColumnType("decimal(18,6)")
                .IsRequired()
                .HasDefaultValue(0m);

            builder.Property(e => e.IsApproved)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.ApprovedDate)
                .IsRequired(false);

            builder.Property(e => e.Description)
                .HasMaxLength(500)
                .IsRequired()
                .HasDefaultValue(string.Empty);

            builder.HasMany(e => e.Lines)
                .WithOne(l => l.TempQuotattion)
                .HasForeignKey(l => l.TempQuotattionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.ExchangeLines)
                .WithOne(x => x.TempQuotattion)
                .HasForeignKey(x => x.TempQuotattionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.CustomerId);
            builder.HasIndex(e => e.IsApproved);
            builder.HasIndex(e => e.CreatedDate);
            builder.HasIndex(e => e.RevisionId);
            builder.HasIndex(e => e.QuotationId);
        }
    }

    public class TempQuotattionLineConfiguration : BaseEntityConfiguration<TempQuotattionLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<TempQuotattionLine> builder)
        {
            builder.ToTable("RII_TEMP_QUOTATTION_LINE");

            builder.Property(e => e.TempQuotattionId)
                .IsRequired();

            builder.Property(e => e.ProductCode)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.ProductName)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(e => e.Description)
                .HasMaxLength(500)
                .IsRequired()
                .HasDefaultValue(string.Empty);

            builder.Property(e => e.Quantity).HasColumnType("decimal(18,6)");
            builder.Property(e => e.UnitPrice).HasColumnType("decimal(18,6)");
            builder.Property(e => e.DiscountRate1).HasColumnType("decimal(18,6)");
            builder.Property(e => e.DiscountAmount1).HasColumnType("decimal(18,6)");
            builder.Property(e => e.DiscountRate2).HasColumnType("decimal(18,6)");
            builder.Property(e => e.DiscountAmount2).HasColumnType("decimal(18,6)");
            builder.Property(e => e.DiscountRate3).HasColumnType("decimal(18,6)");
            builder.Property(e => e.DiscountAmount3).HasColumnType("decimal(18,6)");
            builder.Property(e => e.VatRate).HasColumnType("decimal(18,6)");
            builder.Property(e => e.VatAmount).HasColumnType("decimal(18,6)");
            builder.Property(e => e.LineTotal).HasColumnType("decimal(18,6)");
            builder.Property(e => e.LineGrandTotal).HasColumnType("decimal(18,6)");

            builder.HasIndex(e => e.TempQuotattionId);
            builder.HasIndex(e => e.ProductCode);
            builder.HasIndex(e => e.IsDeleted);
        }
    }

    public class TempQuotattionExchangeLineConfiguration : BaseEntityConfiguration<TempQuotattionExchangeLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<TempQuotattionExchangeLine> builder)
        {
            builder.ToTable("RII_TEMP_QUOTATTION_EXCHANGE_LINE");

            builder.Property(e => e.TempQuotattionId)
                .IsRequired();

            builder.Property(e => e.Currency)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(e => e.ExchangeRate)
                .HasColumnType("decimal(18,6)")
                .IsRequired();

            builder.Property(e => e.ExchangeRateDate)
                .IsRequired();

            builder.Property(e => e.IsManual)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasIndex(e => e.TempQuotattionId);
            builder.HasIndex(e => e.Currency);
            builder.HasIndex(e => e.IsDeleted);
            builder.HasIndex(e => new { e.TempQuotattionId, e.Currency })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");
        }
    }
}
