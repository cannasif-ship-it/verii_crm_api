using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using cms_webapi.Models;

namespace cms_webapi.Data.Configurations
{
    public class QuotationExchangeRateConfiguration : BaseEntityConfiguration<QuotationExchangeRate>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<QuotationExchangeRate> builder)
        {
            // Table name
            builder.ToTable("RII_QUOTATION_EXCHANGE_RATE");

            // Quotation relationship
            builder.Property(e => e.QuotationId)
                .IsRequired();

            builder.HasOne(e => e.Quotation)
                .WithMany()
                .HasForeignKey(e => e.QuotationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Currency
            builder.Property(e => e.Currency)
                .HasMaxLength(50)
                .IsRequired();

            // Exchange Rate
            builder.Property(e => e.ExchangeRate)
                .HasColumnType("decimal(18,6)")
                .IsRequired();

            // Exchange Rate Date
            builder.Property(e => e.ExchangeRateDate)
                .IsRequired();

            // Is Official
            builder.Property(e => e.IsOfficial)
                .IsRequired()
                .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(e => e.QuotationId)
                .HasDatabaseName("IX_QuotationExchangeRate_QuotationId");

            builder.HasIndex(e => e.Currency)
                .HasDatabaseName("IX_QuotationExchangeRate_Currency");

            builder.HasIndex(e => e.ExchangeRateDate)
                .HasDatabaseName("IX_QuotationExchangeRate_ExchangeRateDate");

            builder.HasIndex(e => e.IsOfficial)
                .HasDatabaseName("IX_QuotationExchangeRate_IsOfficial");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_QuotationExchangeRate_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
