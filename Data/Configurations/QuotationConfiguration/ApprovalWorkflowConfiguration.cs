using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using cms_webapi.Models;

namespace cms_webapi.Data.Configurations
{
    public class ApprovalWorkflowConfiguration : BaseEntityConfiguration<ApprovalWorkflow>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ApprovalWorkflow> builder)
        {
            // Table name
            builder.ToTable("RII_APPROVAL_WORKFLOW");

            // Properties
            builder.Property(e => e.CustomerTypeId)
                .IsRequired();

            builder.Property(e => e.UserId)
                .IsRequired(false);

            builder.Property(e => e.MinAmount)
                .HasColumnType("decimal(18,6)");

            builder.Property(e => e.MaxAmount)
                .HasColumnType("decimal(18,6)");

            // Relationships
            builder.HasOne(e => e.CustomerType)
                .WithMany()
                .HasForeignKey(e => e.CustomerTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Indexes
            builder.HasIndex(e => e.CustomerTypeId)
                .HasDatabaseName("IX_ApprovalWorkflow_CustomerTypeId");

            builder.HasIndex(e => e.UserId)
                .HasDatabaseName("IX_ApprovalWorkflow_UserId");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_ApprovalWorkflow_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}