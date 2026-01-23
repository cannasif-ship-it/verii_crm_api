using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models;

namespace crm_api.Data.Configurations
{
    public class ApprovalFlowStepConfiguration : BaseEntityConfiguration<ApprovalFlowStep>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ApprovalFlowStep> builder)
        {
            // Table name
            builder.ToTable("RII_APPROVAL_FLOW_STEP");

            // Properties
            builder.Property(e => e.ApprovalFlowId)
                .IsRequired();

            builder.Property(e => e.StepOrder)
                .IsRequired();

            builder.Property(e => e.ApprovalRoleGroupId)
                .IsRequired();

            // Navigation Properties
            builder.HasOne(e => e.ApprovalFlow)
                .WithMany()
                .HasForeignKey(e => e.ApprovalFlowId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.ApprovalRoleGroup)
                .WithMany()
                .HasForeignKey(e => e.ApprovalRoleGroupId)
                .OnDelete(DeleteBehavior.NoAction);

            // Indexes
            builder.HasIndex(e => e.ApprovalFlowId)
                .HasDatabaseName("IX_ApprovalFlowStep_ApprovalFlowId");

            builder.HasIndex(e => e.ApprovalRoleGroupId)
                .HasDatabaseName("IX_ApprovalFlowStep_ApprovalRoleGroupId");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_ApprovalFlowStep_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
