using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models;

namespace crm_api.Data.Configurations
{
    public class DemandExchangeRateConfiguration : BaseEntityConfiguration<DemandExchangeRate>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<DemandExchangeRate> builder)
        {
            // Table name
            builder.ToTable("RII_DEMAND_EXCHANGE_RATE");

            // Demand relationship
            builder.Property(e => e.DemandId)
                .IsRequired();

            builder.HasOne(e => e.Demand)
                .WithMany()
                .HasForeignKey(e => e.DemandId)
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
            builder.HasIndex(e => e.DemandId)
                .HasDatabaseName("IX_DemandExchangeRate_DemandId");

            builder.HasIndex(e => e.Currency)
                .HasDatabaseName("IX_DemandExchangeRate_Currency");

            builder.HasIndex(e => e.ExchangeRateDate)
                .HasDatabaseName("IX_DemandExchangeRate_ExchangeRateDate");

            builder.HasIndex(e => e.IsOfficial)
                .HasDatabaseName("IX_DemandExchangeRate_IsOfficial");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_DemandExchangeRate_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
