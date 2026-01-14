using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using cms_webapi.Models;

namespace cms_webapi.Data.Configurations
{
    public class ShippingAddressConfiguration : BaseEntityConfiguration<ShippingAddress>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ShippingAddress> builder)
        {
            // Table name
            builder.ToTable("RII_SHIPPING_ADDRESS");

            // ShippingAddress specific properties
            builder.Property(e => e.Address)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(e => e.PostalCode)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(e => e.ContactPerson)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(e => e.Notes)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(e => e.CustomerId)
                .IsRequired();

            builder.Property(e => e.CountryId)
                .IsRequired(false);

            builder.Property(e => e.CityId)
                .IsRequired(false);

            builder.Property(e => e.DistrictId)
                .IsRequired(false);

            // Foreign key relationships
            builder.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Countries)
                .WithMany()
                .HasForeignKey(e => e.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Cities)
                .WithMany()
                .HasForeignKey(e => e.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Districts)
                .WithMany()
                .HasForeignKey(e => e.DistrictId)
                .OnDelete(DeleteBehavior.NoAction);

            // Indexes
            builder.HasIndex(e => e.CustomerId)
                .HasDatabaseName("IX_ShippingAddress_CustomerId");

            builder.HasIndex(e => e.CountryId)
                .HasDatabaseName("IX_ShippingAddress_CountryId");

            builder.HasIndex(e => e.CityId)
                .HasDatabaseName("IX_ShippingAddress_CityId");

            builder.HasIndex(e => e.DistrictId)
                .HasDatabaseName("IX_ShippingAddress_DistrictId");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_ShippingAddress_IsDeleted");

            // Query filter for soft deletion
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
