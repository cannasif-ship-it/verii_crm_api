using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using cms_webapi.Models;

namespace cms_webapi.Data.Configurations
{
    public class PaymentTypeConfiguration : BaseEntityConfiguration<PaymentType>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PaymentType> builder)
        {
            // Table name
            builder.ToTable("RII_PAYMENT_TYPE");

            // Properties
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            builder.Property(e => e.Description)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            // Indexes
            builder.HasIndex(e => e.Name)
                .HasDatabaseName("IX_PaymentType_Name");

            builder.HasIndex(e => e.CreatedDate)
                .HasDatabaseName("IX_PaymentType_CreatedDate");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_PaymentType_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
