using crm_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Data.Configurations
{
    public class ActivityReminderConfiguration : BaseEntityConfiguration<ActivityReminder>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ActivityReminder> builder)
        {
            builder.ToTable("RII_ACTIVITY_REMINDER");

            builder.Property(e => e.ActivityId).IsRequired();
            builder.Property(e => e.OffsetMinutes).IsRequired();
            builder.Property(e => e.Channel).IsRequired().HasConversion<int>().HasDefaultValue(ReminderChannel.InApp);
            builder.Property(e => e.Status).IsRequired().HasConversion<int>().HasDefaultValue(ReminderStatus.Pending);
            builder.Property(e => e.SentAt).IsRequired(false);

            builder.HasOne(e => e.Activity)
                .WithMany(a => a.Reminders)
                .HasForeignKey(e => e.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.ActivityId).HasDatabaseName("IX_ActivityReminder_ActivityId");
            builder.HasIndex(e => e.Status).HasDatabaseName("IX_ActivityReminder_Status");
            builder.HasIndex(e => e.Channel).HasDatabaseName("IX_ActivityReminder_Channel");
            builder.HasIndex(e => e.IsDeleted).HasDatabaseName("IX_ActivityReminder_IsDeleted");

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
