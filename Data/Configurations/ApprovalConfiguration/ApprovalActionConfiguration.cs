using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models;

namespace crm_api.Data.Configurations
{
    public class ApprovalActionConfiguration : BaseEntityConfiguration<ApprovalAction>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ApprovalAction> builder)
        {
            // Table name
            builder.ToTable("RII_APPROVAL_ACTION");

            // Properties
            builder.Property(e => e.ApprovalRequestId)
                .IsRequired();

            builder.Property(e => e.StepOrder)
                .IsRequired();

            builder.Property(e => e.ApprovedByUserId)
                .IsRequired();

            builder.Property(e => e.ActionDate)
                .IsRequired();

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>();

            // Navigation Properties
            builder.HasOne(e => e.ApprovalRequest)
                .WithMany()
                .HasForeignKey(e => e.ApprovalRequestId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.ApprovedByUser)
                .WithMany()
                .HasForeignKey(e => e.ApprovedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Indexes
            builder.HasIndex(e => e.ApprovalRequestId)
                .HasDatabaseName("IX_ApprovalAction_ApprovalRequestId");

            builder.HasIndex(e => e.ApprovedByUserId)
                .HasDatabaseName("IX_ApprovalAction_ApprovedByUserId");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_ApprovalAction_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
