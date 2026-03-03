using crm_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Data.Configurations
{
    public class TenantGoogleOAuthSettingsConfiguration : IEntityTypeConfiguration<TenantGoogleOAuthSettings>
    {
        public void Configure(EntityTypeBuilder<TenantGoogleOAuthSettings> builder)
        {
            builder.ToTable("RII_TENANT_GOOGLE_OAUTH_SETTINGS");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.TenantId)
                .IsRequired();

            builder.Property(x => x.ClientId)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(x => x.ClientSecretEncrypted)
                .HasMaxLength(4000)
                .IsRequired();

            builder.Property(x => x.RedirectUri)
                .HasMaxLength(512);

            builder.Property(x => x.Scopes)
                .HasMaxLength(2000);

            builder.Property(x => x.IsEnabled)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.HasIndex(x => x.TenantId)
                .IsUnique()
                .HasDatabaseName("IX_TenantGoogleOAuthSettings_TenantId");
        }
    }
}
