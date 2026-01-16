using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using cms_webapi.Models;

namespace cms_webapi.Data.Configurations
{
    public class ActivityConfiguration : BaseEntityConfiguration<Activity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Activity> builder)
        {
            // Table name
            builder.ToTable("RII_ACTIVITY");

            // Activity specific properties
            builder.Property(e => e.Subject)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Description)
                .HasMaxLength(500)
                .IsRequired(false);

            // ActivityType relationship
            builder.Property(e => e.ActivityTypeId)
                .IsRequired(false);

            builder.HasOne(e => e.ActivityType)
                .WithMany(at => at.Activities)
                .HasForeignKey(e => e.ActivityTypeId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(e => e.ErpCustomerCode)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(e => e.Status)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.IsCompleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.Priority)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(e => e.ActivityDate)
                .IsRequired(false);

            // Relationship configurations
            builder.HasOne(e => e.PotentialCustomer)
                .WithMany()
                .HasForeignKey(e => e.PotentialCustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Contact)
                .WithMany()
                .HasForeignKey(e => e.ContactId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.AssignedUser)
                .WithMany()
                .HasForeignKey(e => e.AssignedUserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Indexes
            builder.HasIndex(e => e.Subject)
                .HasDatabaseName("IX_Activity_Subject");

            builder.HasIndex(e => e.ActivityTypeId)
                .HasDatabaseName("IX_Activity_ActivityTypeId");

            builder.HasIndex(e => e.Status)
                .HasDatabaseName("IX_Activity_Status");

            builder.HasIndex(e => e.IsCompleted)
                .HasDatabaseName("IX_Activity_IsCompleted");

            builder.HasIndex(e => e.PotentialCustomerId)
                .HasDatabaseName("IX_Activity_PotentialCustomerId");

            builder.HasIndex(e => e.ContactId)
                .HasDatabaseName("IX_Activity_ContactId");

            builder.HasIndex(e => e.AssignedUserId)
                .HasDatabaseName("IX_Activity_AssignedUserId");

            builder.HasIndex(e => e.ActivityDate)
                .HasDatabaseName("IX_Activity_ActivityDate");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_Activity_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
