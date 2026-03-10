using crm_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Data.Configurations
{
    public class UserOutlookAccountConfiguration : IEntityTypeConfiguration<UserOutlookAccount>
    {
        public void Configure(EntityTypeBuilder<UserOutlookAccount> builder)
        {
            builder.ToTable("RII_USER_OUTLOOK_ACCOUNTS");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.TenantId).IsRequired();
            builder.Property(x => x.OutlookEmail).HasMaxLength(256);
            builder.Property(x => x.RefreshTokenEncrypted).HasMaxLength(4000);
            builder.Property(x => x.AccessTokenEncrypted).HasMaxLength(4000);
            builder.Property(x => x.Scopes).HasMaxLength(2000);
            builder.Property(x => x.IsConnected).HasDefaultValue(false).IsRequired();
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();
            builder.Property(x => x.UpdatedAt).HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => x.UserId).IsUnique().HasDatabaseName("IX_UserOutlookAccounts_UserId");
            builder.HasIndex(x => x.TenantId).HasDatabaseName("IX_UserOutlookAccounts_TenantId");
        }
    }
}
