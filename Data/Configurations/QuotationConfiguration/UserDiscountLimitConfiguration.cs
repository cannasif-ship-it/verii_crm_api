using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using cms_webapi.Models;

namespace cms_webapi.Data.Configurations
{
    public class UserDiscountLimitConfiguration : BaseEntityConfiguration<UserDiscountLimit>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<UserDiscountLimit> builder)
        {
            // Table name
            builder.ToTable("RII_USER_DISCOUNT_LIMIT");

            // UserDiscountLimit specific properties
            builder.Property(e => e.ErpProductGroupCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.SalespersonId)
                .IsRequired();

            builder.Property(e => e.MaxDiscount1)
                .HasColumnType("decimal(18,6)")
                .IsRequired();

            builder.Property(e => e.MaxDiscount2)
                .HasColumnType("decimal(18,6)")
                .IsRequired(false);

            builder.Property(e => e.MaxDiscount3)
                .HasColumnType("decimal(18,6)")
                .IsRequired(false);

            // Foreign key relationship
            builder.HasOne(e => e.Salespersons)
                .WithMany()
                .HasForeignKey(e => e.SalespersonId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes for performance
            builder.HasIndex(e => e.SalespersonId)
                .HasDatabaseName("IX_UserDiscountLimit_SalespersonId");

            builder.HasIndex(e => e.ErpProductGroupCode)
                .HasDatabaseName("IX_UserDiscountLimit_ErpProductGroupCode");

            builder.HasIndex(e => new { e.SalespersonId, e.ErpProductGroupCode })
                .HasDatabaseName("IX_UserDiscountLimit_SalespersonId_ErpProductGroupCode")
                .IsUnique();

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_UserDiscountLimit_IsDeleted");

            // Query filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
