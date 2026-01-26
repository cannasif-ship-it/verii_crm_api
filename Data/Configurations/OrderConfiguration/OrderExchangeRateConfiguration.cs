using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models;

namespace crm_api.Data.Configurations
{
    public class OrderExchangeRateConfiguration : BaseEntityConfiguration<OrderExchangeRate>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<OrderExchangeRate> builder)
        {
            // Table name
            builder.ToTable("RII_ORDER_EXCHANGE_RATE");

            // Order relationship
            builder.Property(e => e.OrderId)
                .IsRequired();

            builder.HasOne(e => e.Order)
                .WithMany()
                .HasForeignKey(e => e.OrderId)
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
            builder.HasIndex(e => e.OrderId)
                .HasDatabaseName("IX_OrderExchangeRate_OrderId");

            builder.HasIndex(e => e.Currency)
                .HasDatabaseName("IX_OrderExchangeRate_Currency");

            builder.HasIndex(e => e.ExchangeRateDate)
                .HasDatabaseName("IX_OrderExchangeRate_ExchangeRateDate");

            builder.HasIndex(e => e.IsOfficial)
                .HasDatabaseName("IX_OrderExchangeRate_IsOfficial");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_OrderExchangeRate_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
