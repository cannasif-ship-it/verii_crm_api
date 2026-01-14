using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using cms_webapi.Models;

namespace cms_webapi.Data.Configurations
{
    public class ApprovalTransactionConfiguration : BaseEntityConfiguration<ApprovalTransaction>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ApprovalTransaction> builder)
        {
            // Table name
            builder.ToTable("RII_APPROVAL_TRANSACTION");

            // Document Id (QuotationId)
            builder.Property(e => e.DocumentId)
                .IsRequired();

            // Quotation relationship
            builder.HasOne(e => e.Quotation)
                .WithMany()
                .HasForeignKey(e => e.DocumentId)
                .OnDelete(DeleteBehavior.NoAction);

            // Line Id (nullable, for line-level approvals)
            builder.Property(e => e.LineId)
                .IsRequired(false);

            // QuotationLine relationship
            builder.HasOne(e => e.QuotationLine)
                .WithMany()
                .HasForeignKey(e => e.LineId)
                .OnDelete(DeleteBehavior.NoAction);

            // Approval Level (enum)
            builder.Property(e => e.ApprovalLevel)
                .IsRequired()
                .HasConversion<int>();

            // Status (enum)
            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(ApprovalStatus.Waiting);

            // Approved By User relationship
            builder.Property(e => e.ApprovedByUserId)
                .IsRequired(false);

            builder.HasOne(e => e.ApprovedByUser)
                .WithMany()
                .HasForeignKey(e => e.ApprovedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Requested At
            builder.Property(e => e.RequestedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // Action Date
            builder.Property(e => e.ActionDate)
                .IsRequired(false);

            // Note
            builder.Property(e => e.Note)
                .HasMaxLength(500)
                .IsRequired(false);

            // Indexes
            builder.HasIndex(e => e.DocumentId)
                .HasDatabaseName("IX_ApprovalTransaction_DocumentId");

            builder.HasIndex(e => e.LineId)
                .HasDatabaseName("IX_ApprovalTransaction_LineId");

            builder.HasIndex(e => e.ApprovalLevel)
                .HasDatabaseName("IX_ApprovalTransaction_ApprovalLevel");

            builder.HasIndex(e => e.Status)
                .HasDatabaseName("IX_ApprovalTransaction_Status");

            builder.HasIndex(e => e.ApprovedByUserId)
                .HasDatabaseName("IX_ApprovalTransaction_ApprovedByUserId");

            builder.HasIndex(e => e.RequestedAt)
                .HasDatabaseName("IX_ApprovalTransaction_RequestedAt");

            builder.HasIndex(e => e.ActionDate)
                .HasDatabaseName("IX_ApprovalTransaction_ActionDate");

            // Composite index for document and line
            builder.HasIndex(e => new { e.DocumentId, e.LineId })
                .HasDatabaseName("IX_ApprovalTransaction_DocumentId_LineId");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_ApprovalTransaction_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
