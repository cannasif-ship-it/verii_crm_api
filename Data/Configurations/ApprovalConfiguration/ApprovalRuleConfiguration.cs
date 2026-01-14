using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using cms_webapi.Models;

namespace cms_webapi.Data.Configurations
{
    public class ApprovalRuleConfiguration : BaseEntityConfiguration<ApprovalRule>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ApprovalRule> builder)
        {
            // Table name
            builder.ToTable("RII_APPROVAL_RULE");

            // ApprovalAuthority relationship
            builder.Property(e => e.ApprovalAuthorityId)
                .IsRequired();

            builder.HasOne(e => e.ApprovalAuthority)
                .WithMany()
                .HasForeignKey(e => e.ApprovalAuthorityId)
                .OnDelete(DeleteBehavior.NoAction);

            // ForwardToUpperManagement
            builder.Property(e => e.ForwardToUpperManagement)
                .IsRequired()
                .HasDefaultValue(false);

            // ForwardToLevel (enum, nullable)
            builder.Property(e => e.ForwardToLevel)
                .IsRequired(false)
                .HasConversion<int>();

            // RequireFinanceApproval
            builder.Property(e => e.RequireFinanceApproval)
                .IsRequired()
                .HasDefaultValue(false);

            // IsActive
            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(e => e.ApprovalAuthorityId)
                .HasDatabaseName("IX_ApprovalRule_ApprovalAuthorityId");

            builder.HasIndex(e => e.IsActive)
                .HasDatabaseName("IX_ApprovalRule_IsActive");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_ApprovalRule_IsDeleted");

            // Composite index for active rules
            builder.HasIndex(e => new { e.ApprovalAuthorityId, e.IsActive })
                .HasDatabaseName("IX_ApprovalRule_AuthorityId_IsActive");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
