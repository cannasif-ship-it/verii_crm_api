using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using cms_webapi.Models;

namespace cms_webapi.Data.Configurations
{
    public class UserHierarchyConfiguration : BaseEntityConfiguration<UserHierarchy>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<UserHierarchy> builder)
        {
            // Table name
            builder.ToTable("RII_USER_HIERARCHY");

            // Salesperson relationship
            builder.Property(e => e.SalespersonId)
                .IsRequired();

            builder.HasOne(e => e.Salesperson)
                .WithMany()
                .HasForeignKey(e => e.SalespersonId)
                .OnDelete(DeleteBehavior.NoAction);

            // Manager relationship (nullable)
            builder.Property(e => e.ManagerId)
                .IsRequired(false);

            builder.HasOne(e => e.Manager)
                .WithMany()
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.NoAction);

            // GeneralManager relationship (nullable)
            builder.Property(e => e.GeneralManagerId)
                .IsRequired(false);

            builder.HasOne(e => e.GeneralManager)
                .WithMany()
                .HasForeignKey(e => e.GeneralManagerId)
                .OnDelete(DeleteBehavior.NoAction);

            // HierarchyLevel
            builder.Property(e => e.HierarchyLevel)
                .IsRequired()
                .HasDefaultValue(1);

            // IsActive
            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(e => e.SalespersonId)
                .HasDatabaseName("IX_UserHierarchy_SalespersonId");

            builder.HasIndex(e => e.ManagerId)
                .HasDatabaseName("IX_UserHierarchy_ManagerId");

            builder.HasIndex(e => e.GeneralManagerId)
                .HasDatabaseName("IX_UserHierarchy_GeneralManagerId");

            builder.HasIndex(e => e.IsActive)
                .HasDatabaseName("IX_UserHierarchy_IsActive");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_UserHierarchy_IsDeleted");

            // Composite index for active hierarchies
            builder.HasIndex(e => new { e.SalespersonId, e.IsActive })
                .HasDatabaseName("IX_UserHierarchy_SalespersonId_IsActive");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
