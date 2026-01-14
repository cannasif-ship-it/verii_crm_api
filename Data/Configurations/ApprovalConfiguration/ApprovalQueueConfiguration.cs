using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using cms_webapi.Models;

namespace cms_webapi.Data.Configurations
{
    public class ApprovalQueueConfiguration : BaseEntityConfiguration<ApprovalQueue>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ApprovalQueue> builder)
        {
            // Table name
            builder.ToTable("RII_APPROVAL_QUEUE");

            // Quotation relationship
            builder.Property(e => e.QuotationId)
                .IsRequired();

            builder.HasOne(e => e.Quotation)
                .WithMany()
                .HasForeignKey(e => e.QuotationId)
                .OnDelete(DeleteBehavior.NoAction);

            // QuotationLine relationship (nullable)
            builder.Property(e => e.QuotationLineId)
                .IsRequired(false);

            builder.HasOne(e => e.QuotationLine)
                .WithMany()
                .HasForeignKey(e => e.QuotationLineId)
                .OnDelete(DeleteBehavior.NoAction);

            // AssignedToUser relationship
            builder.Property(e => e.AssignedToUserId)
                .IsRequired();

            builder.HasOne(e => e.AssignedToUser)
                .WithMany()
                .HasForeignKey(e => e.AssignedToUserId)
                .OnDelete(DeleteBehavior.NoAction);

            // ApprovalLevel (enum)
            builder.Property(e => e.ApprovalLevel)
                .IsRequired()
                .HasConversion<int>();

            // Status (enum)
            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(ApprovalStatus.Waiting);

            // AssignedAt
            builder.Property(e => e.AssignedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // CompletedAt
            builder.Property(e => e.CompletedAt)
                .IsRequired(false);

            // SequenceOrder
            builder.Property(e => e.SequenceOrder)
                .IsRequired()
                .HasDefaultValue(1);

            // IsCurrent
            builder.Property(e => e.IsCurrent)
                .IsRequired()
                .HasDefaultValue(true);

            // Note
            builder.Property(e => e.Note)
                .HasMaxLength(500)
                .IsRequired(false);

            // Indexes
            builder.HasIndex(e => e.QuotationId)
                .HasDatabaseName("IX_ApprovalQueue_QuotationId");

            builder.HasIndex(e => e.QuotationLineId)
                .HasDatabaseName("IX_ApprovalQueue_QuotationLineId");

            builder.HasIndex(e => e.AssignedToUserId)
                .HasDatabaseName("IX_ApprovalQueue_AssignedToUserId");

            builder.HasIndex(e => e.ApprovalLevel)
                .HasDatabaseName("IX_ApprovalQueue_ApprovalLevel");

            builder.HasIndex(e => e.Status)
                .HasDatabaseName("IX_ApprovalQueue_Status");

            builder.HasIndex(e => e.IsCurrent)
                .HasDatabaseName("IX_ApprovalQueue_IsCurrent");

            builder.HasIndex(e => e.SequenceOrder)
                .HasDatabaseName("IX_ApprovalQueue_SequenceOrder");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_ApprovalQueue_IsDeleted");

            // Composite indexes
            builder.HasIndex(e => new { e.QuotationId, e.IsCurrent, e.Status })
                .HasDatabaseName("IX_ApprovalQueue_QuotationId_IsCurrent_Status");

            builder.HasIndex(e => new { e.AssignedToUserId, e.Status, e.IsCurrent })
                .HasDatabaseName("IX_ApprovalQueue_AssignedToUserId_Status_IsCurrent");

            builder.HasIndex(e => new { e.QuotationId, e.SequenceOrder })
                .HasDatabaseName("IX_ApprovalQueue_QuotationId_SequenceOrder");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
