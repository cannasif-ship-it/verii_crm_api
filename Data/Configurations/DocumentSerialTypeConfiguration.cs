using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using cms_webapi.Models;

namespace cms_webapi.Data.Configurations
{
    public class DocumentSerialTypeConfiguration : BaseEntityConfiguration<DocumentSerialType>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<DocumentSerialType> builder)
        {
            // Table name
            builder.ToTable("RII_DOCUMENT_SERIAL_TYPE");

            // Rule Type (enum)
            builder.Property(e => e.RuleType)
                .IsRequired()
                .HasConversion<int>();

            // Customer Type relationship
            builder.Property(e => e.CustomerTypeId)
                .IsRequired(false);

            builder.HasOne(e => e.CustomerType)
                .WithMany()
                .HasForeignKey(e => e.CustomerTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            // Sales Rep relationship
            builder.Property(e => e.SalesRepId)
                .IsRequired(false);

            builder.HasOne(e => e.SalesRep)
                .WithMany()
                .HasForeignKey(e => e.SalesRepId)
                .OnDelete(DeleteBehavior.NoAction);

            // Indexes
            builder.HasIndex(e => e.RuleType)
                .HasDatabaseName("IX_DocumentSerialType_RuleType");

            builder.HasIndex(e => e.CustomerTypeId)
                .HasDatabaseName("IX_DocumentSerialType_CustomerTypeId");

            builder.HasIndex(e => e.SalesRepId)
                .HasDatabaseName("IX_DocumentSerialType_SalesRepId");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_DocumentSerialType_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
