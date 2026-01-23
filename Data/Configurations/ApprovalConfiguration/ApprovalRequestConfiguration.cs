using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models;

namespace crm_api.Data.Configurations
{
    public class ApprovalRequestConfiguration : BaseEntityConfiguration<ApprovalRequest>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ApprovalRequest> builder)
        {
            // Table name
            builder.ToTable("RII_APPROVAL_REQUEST");

            // Properties
            builder.Property(e => e.EntityId)
                .IsRequired();

            builder.Property(e => e.DocumentType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(e => e.ApprovalFlowId)
                .IsRequired();

            builder.Property(e => e.CurrentStep)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(ApprovalStatus.Waiting);

            // Navigation Properties
            builder.HasOne(e => e.ApprovalFlow)
                .WithMany()
                .HasForeignKey(e => e.ApprovalFlowId)
                .OnDelete(DeleteBehavior.NoAction);

            // Indexes
            builder.HasIndex(e => e.EntityId)
                .HasDatabaseName("IX_ApprovalRequest_EntityId");

            builder.HasIndex(e => e.DocumentType)
                .HasDatabaseName("IX_ApprovalRequest_DocumentType");

            builder.HasIndex(e => e.ApprovalFlowId)
                .HasDatabaseName("IX_ApprovalRequest_ApprovalFlowId");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_ApprovalRequest_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
