using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models;

namespace crm_api.Data.Configurations
{
    public class ApprovalRoleGroupConfiguration : BaseEntityConfiguration<ApprovalRoleGroup>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ApprovalRoleGroup> builder)
        {
            // Table name
            builder.ToTable("RII_APPROVAL_ROLE_GROUP");

            // Properties
            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            // Indexes
            builder.HasIndex(e => e.Name)
                .HasDatabaseName("IX_ApprovalRoleGroup_Name");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_ApprovalRoleGroup_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
