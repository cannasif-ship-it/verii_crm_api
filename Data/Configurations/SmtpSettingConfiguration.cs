using crm_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Persistence.Configurations
{
    public class SmtpSettingConfiguration : IEntityTypeConfiguration<SmtpSetting>
    {
        public void Configure(EntityTypeBuilder<SmtpSetting> builder)
        {
            builder.ToTable("RII_SMTP_SETTING");

            builder.Property(x => x.Host).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Username).HasMaxLength(200);
            builder.Property(x => x.FromEmail).HasMaxLength(200);
            builder.Property(x => x.FromName).HasMaxLength(200);
            builder.Property(x => x.PasswordEncrypted).HasMaxLength(2000);

            builder.Property(x => x.Port).IsRequired();
            builder.Property(x => x.Timeout).IsRequired();

            // Tek kayıt mantığına yardımcı: Id = 1 seed (opsiyonel ama pratik)
            builder.HasData(new SmtpSetting
            {
                Id = 1,
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Username = "",
                PasswordEncrypted = "",
                FromEmail = "",
                FromName = "V3RII CRM SYSTEM",
                Timeout = 30
                // EntityBase alanların seed’de gerekiyorsa eklenir (CreatedAt vs.)
            });
        }
    }
}
