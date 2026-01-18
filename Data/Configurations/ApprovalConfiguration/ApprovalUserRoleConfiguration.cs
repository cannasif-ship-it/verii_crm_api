using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using cms_webapi.Models;

namespace cms_webapi.Data.Configurations
{
    public class ApprovalUserRoleConfiguration : BaseEntityConfiguration<ApprovalUserRole>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ApprovalUserRole> builder)
        {
            // Table name
            builder.ToTable("RII_APPROVAL_USER_ROLE");

            // Properties
            builder.Property(e => e.UserId)
                .IsRequired();

            builder.Property(e => e.ApprovalRoleId)
                .IsRequired();

            // Navigation Properties
            builder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.ApprovalRole)
                .WithMany()
                .HasForeignKey(e => e.ApprovalRoleId)
                .OnDelete(DeleteBehavior.NoAction);

            // Indexes
            builder.HasIndex(e => e.UserId)
                .HasDatabaseName("IX_ApprovalUserRole_UserId");

            builder.HasIndex(e => e.ApprovalRoleId)
                .HasDatabaseName("IX_ApprovalUserRole_ApprovalRoleId");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_ApprovalUserRole_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
