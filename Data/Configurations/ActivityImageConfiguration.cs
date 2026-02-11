using crm_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Data.Configurations
{
    public class ActivityImageConfiguration : BaseEntityConfiguration<ActivityImage>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ActivityImage> builder)
        {
            builder.ToTable("RII_ACTIVITY_IMAGE");

            builder.Property(e => e.ActivityId)
                .IsRequired();

            builder.Property(e => e.ResimAciklama)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(e => e.ResimUrl)
                .HasMaxLength(1000)
                .IsRequired();

            builder.HasOne(e => e.Activity)
                .WithMany(a => a.Images)
                .HasForeignKey(e => e.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.ActivityId)
                .HasDatabaseName("IX_ActivityImage_ActivityId");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_ActivityImage_IsDeleted");

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
