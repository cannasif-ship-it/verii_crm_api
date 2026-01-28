using crm_api.Models.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Data.Configurations.NotificationConfiguration
{
    public class NotificationConfiguration : BaseEntityConfiguration<Notification>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            builder.Property(x => x.TitleKey).IsRequired().HasMaxLength(200);
            builder.Property(x => x.TitleArgs).HasMaxLength(1000); // Arguments might be long JSON
            
            builder.Property(x => x.MessageKey).IsRequired().HasMaxLength(200);
            builder.Property(x => x.MessageArgs).HasMaxLength(2000); // Arguments might be long JSON
            
            builder.Property(x => x.RelatedEntityName).HasMaxLength(100);
            
            builder.Property(x => x.NotificationType)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.IsRead);
        }
    }
}
