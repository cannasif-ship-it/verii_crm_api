using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models;

namespace crm_api.Data.Configurations
{
    public class CountryConfiguration : BaseEntityConfiguration<Country>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Country> builder)
        {
            // Table name
            builder.ToTable("RII_COUNTRY");

            // Properties
            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Code)
                .HasMaxLength(5)
                .IsRequired();

            builder.Property(e => e.ERPCode)
                .HasMaxLength(10)
                .IsRequired(false);

            // Navigation Properties
            builder.HasMany(e => e.Cities)
                .WithOne(c => c.Country)
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(e => e.Name)
                .IsUnique()
                .HasDatabaseName("IX_Country_Name");

            builder.HasIndex(e => e.Code)
                .IsUnique()
                .HasDatabaseName("IX_Country_Code");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_Country_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
