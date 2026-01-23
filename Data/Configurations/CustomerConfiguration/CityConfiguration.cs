using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models;

namespace crm_api.Data.Configurations
{
    public class CityConfiguration : BaseEntityConfiguration<City>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<City> builder)
        {
            // Table name
            builder.ToTable("RII_CITY");

            // Properties
            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.ERPCode)
                .HasMaxLength(10)
                .IsRequired(false);

            builder.Property(e => e.CountryId)
                .IsRequired();

            // Foreign Key Relationships
            builder.HasOne(e => e.Country)
                .WithMany()
                .HasForeignKey(e => e.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            // Indexes
            builder.HasIndex(e => e.Name)
                .HasDatabaseName("IX_City_Name");

            builder.HasIndex(e => e.CountryId)
                .HasDatabaseName("IX_City_CountryId");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_City_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
