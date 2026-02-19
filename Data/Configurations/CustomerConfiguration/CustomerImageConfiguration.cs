using crm_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Data.Configurations
{
    public class CustomerImageConfiguration : BaseEntityConfiguration<CustomerImage>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<CustomerImage> builder)
        {
            builder.ToTable("RII_CUSTOMER_IMAGE");

            builder.Property(e => e.CustomerId)
                .IsRequired();

            builder.Property(e => e.ImageUrl)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(e => e.ImageDescription)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.HasOne(e => e.Customer)
                .WithMany(c => c.CustomerImages)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.CustomerId)
                .HasDatabaseName("IX_CustomerImage_CustomerId");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_CustomerImage_IsDeleted");

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
