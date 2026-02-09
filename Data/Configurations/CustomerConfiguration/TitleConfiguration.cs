using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models;

namespace crm_api.Data.Configurations
{
    public class TitleConfiguration : BaseEntityConfiguration<Title>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Title> builder)
        {
            // Table name
            builder.ToTable("RII_TITLE");

            // Title specific properties
            builder.Property(e => e.TitleName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Code)
                .HasMaxLength(10)
                .IsRequired(false);

            // Relationship configuration
            builder.HasMany(e => e.Contacts)
                .WithOne(c => c.Title)
                .HasForeignKey(c => c.TitleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(e => e.TitleName)
                .HasDatabaseName("IX_Title_TitleName");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_Title_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
