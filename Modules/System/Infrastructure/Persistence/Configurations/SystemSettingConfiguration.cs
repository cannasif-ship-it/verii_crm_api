using crm_api.Modules.System.Domain.Entities;
using crm_api.Shared.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Modules.System.Infrastructure.Persistence.Configurations
{
    public class SystemSettingConfiguration : BaseEntityConfiguration<SystemSetting>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SystemSetting> builder)
        {
            builder.ToTable("SystemSettings");

            builder.Property(x => x.DefaultLanguage).HasMaxLength(10).IsRequired();
            builder.Property(x => x.DefaultCurrencyCode).HasMaxLength(10).IsRequired();
            builder.Property(x => x.DefaultTimeZone).HasMaxLength(100).IsRequired();
            builder.Property(x => x.DateFormat).HasMaxLength(20).IsRequired();
            builder.Property(x => x.TimeFormat).HasMaxLength(20).IsRequired();
            builder.Property(x => x.NumberFormat).HasMaxLength(20).IsRequired();
            builder.Property(x => x.DecimalPlaces).IsRequired();
        }
    }
}
