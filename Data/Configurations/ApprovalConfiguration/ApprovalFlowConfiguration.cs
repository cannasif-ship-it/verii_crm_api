using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using cms_webapi.Models;

namespace cms_webapi.Data.Configurations
{
    public class ApprovalFlowConfiguration : BaseEntityConfiguration<ApprovalFlow>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ApprovalFlow> builder)
        {
            // Table name
            builder.ToTable("RII_APPROVAL_FLOW");

            // Properties
            builder.Property(e => e.DocumentType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(e => e.Description)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(e => e.DocumentType)
                .HasDatabaseName("IX_ApprovalFlow_DocumentType");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_ApprovalFlow_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
