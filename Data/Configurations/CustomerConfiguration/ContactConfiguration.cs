using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models;

namespace crm_api.Data.Configurations
{
    public class ContactConfiguration : BaseEntityConfiguration<Contact>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Contact> builder)
        {
            // Table name
            builder.ToTable("RII_CONTACT");

            // Contact specific properties
            builder.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Email)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(e => e.Mobile)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(e => e.Notes)
                .HasMaxLength(250)
                .IsRequired(false);

            // Foreign key configuration
            builder.Property(e => e.CustomerId)
                .IsRequired();

            builder.Property(e => e.TitleId)
                .IsRequired();

            // Relationship configuration
            builder.HasOne(e => e.Customers)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Titles)
                .WithMany(t => t.Contacts)
                .HasForeignKey(e => e.TitleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(e => e.Email)
                .HasDatabaseName("IX_Contact_Email");

            builder.HasIndex(e => e.CustomerId)
                .HasDatabaseName("IX_Contact_CustomerId");

            builder.HasIndex(e => e.TitleId)
                .HasDatabaseName("IX_Contact_TitleId");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_Contact_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
