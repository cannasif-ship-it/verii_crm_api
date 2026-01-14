using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using cms_webapi.Models;

namespace cms_webapi.Data.Configurations
{
    public class ApprovalAuthorityConfiguration : BaseEntityConfiguration<ApprovalAuthority>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ApprovalAuthority> builder)
        {
            // Table name
            builder.ToTable("RII_APPROVAL_AUTHORITY");

            // User relationship
            builder.Property(e => e.UserId)
                .IsRequired();

            builder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // MaxApprovalAmount
            builder.Property(e => e.MaxApprovalAmount)
                .HasColumnType("decimal(18,6)")
                .IsRequired();

            // ApprovalLevel (enum)
            builder.Property(e => e.ApprovalLevel)
                .IsRequired()
                .HasConversion<int>();

            // CanFinalize
            builder.Property(e => e.CanFinalize)
                .IsRequired()
                .HasDefaultValue(false);

            // RequireUpperManagement
            builder.Property(e => e.RequireUpperManagement)
                .IsRequired()
                .HasDefaultValue(true);

            // IsActive
            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(e => e.UserId)
                .HasDatabaseName("IX_ApprovalAuthority_UserId");

            builder.HasIndex(e => e.ApprovalLevel)
                .HasDatabaseName("IX_ApprovalAuthority_ApprovalLevel");

            builder.HasIndex(e => e.IsActive)
                .HasDatabaseName("IX_ApprovalAuthority_IsActive");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_ApprovalAuthority_IsDeleted");

            // Composite index for active authorities by level
            builder.HasIndex(e => new { e.ApprovalLevel, e.IsActive })
                .HasDatabaseName("IX_ApprovalAuthority_Level_IsActive");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
