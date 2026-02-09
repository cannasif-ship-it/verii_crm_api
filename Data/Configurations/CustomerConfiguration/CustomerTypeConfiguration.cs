using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models;

namespace crm_api.Data.Configurations
{
    public class CustomerTypeConfiguration : BaseEntityConfiguration<CustomerType>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<CustomerType> builder)
        {
            // Table name
            builder.ToTable("RII_CUSTOMER_TYPE");

            // Properties
            builder.Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Description)
                .HasMaxLength(255)
                .IsRequired(false);

            // Navigation Properties
            builder.HasMany(e => e.Customers)
                .WithOne(c => c.CustomerType)
                .HasForeignKey(c => c.CustomerTypeId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(e => e.Name)
                .IsUnique()
                .HasDatabaseName("IX_CustomerType_Name");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_CustomerType_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
