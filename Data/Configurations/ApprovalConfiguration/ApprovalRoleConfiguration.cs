using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using cms_webapi.Models;

namespace cms_webapi.Data.Configurations
{
    public class ApprovalRoleConfiguration : BaseEntityConfiguration<ApprovalRole>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ApprovalRole> builder)
        {
            // Table name
            builder.ToTable("RII_APPROVAL_ROLE");

            // Properties
            builder.Property(e => e.ApprovalRoleGroupId)
                .IsRequired();

            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            // Navigation Properties
            builder.HasOne(e => e.ApprovalRoleGroup)
                .WithMany()
                .HasForeignKey(e => e.ApprovalRoleGroupId)
                .OnDelete(DeleteBehavior.NoAction);

            // Indexes
            builder.HasIndex(e => e.ApprovalRoleGroupId)
                .HasDatabaseName("IX_ApprovalRole_ApprovalRoleGroupId");

            builder.HasIndex(e => e.Name)
                .HasDatabaseName("IX_ApprovalRole_Name");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_ApprovalRole_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
