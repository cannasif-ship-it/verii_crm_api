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

            builder.Property(x => x.NumberFormat).HasMaxLength(20).IsRequired();
            builder.Property(x => x.DecimalPlaces).IsRequired();
            builder.Property(x => x.RestrictCustomersBySalesRepMatch).IsRequired();
        }
    }
}
