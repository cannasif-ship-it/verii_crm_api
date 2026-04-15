using crm_api.Modules.Definitions.Domain.Entities;
using crm_api.Shared.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Modules.Definitions.Infrastructure.Persistence.Configurations
{
    public class SalesRepCodeConfiguration : BaseEntityConfiguration<SalesRepCode>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SalesRepCode> builder)
        {
            builder.ToTable("RII_SalepRep_Codes");

            builder.Property(e => e.BranchCode)
                .IsRequired();

            builder.Property(e => e.SalesRepCodeValue)
                .IsRequired()
                .HasMaxLength(8)
                .HasColumnName("SalesRepCode")
                .HasColumnType("nvarchar(8)");

            builder.Property(e => e.SalesRepDescription)
                .HasMaxLength(30)
                .HasColumnType("nvarchar(30)");

            builder.Property(e => e.Name)
                .HasMaxLength(35)
                .HasColumnType("nvarchar(35)");

            builder.HasIndex(e => new { e.BranchCode, e.SalesRepCodeValue })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0")
                .HasDatabaseName("IX_SalesRepCode_BranchCode_SalesRepCode");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_SalesRepCode_IsDeleted");

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
