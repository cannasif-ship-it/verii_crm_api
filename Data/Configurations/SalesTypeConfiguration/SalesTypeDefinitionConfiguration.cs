using crm_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Data.Configurations
{
    public class SalesTypeDefinitionConfiguration : BaseEntityConfiguration<SalesTypeDefinition>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SalesTypeDefinition> builder)
        {
            builder.ToTable("RII_SALES_TYPE_DEFINITION");

            builder.Property(e => e.SalesType)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnType("nvarchar(150)");

            builder.HasIndex(e => e.SalesType)
                .HasDatabaseName("IX_SalesTypeDefinition_SalesType");

            builder.HasIndex(e => e.Name)
                .HasDatabaseName("IX_SalesTypeDefinition_Name");

            builder.HasIndex(e => new { e.SalesType, e.Name })
                .HasDatabaseName("IX_SalesTypeDefinition_SalesType_Name")
                .IsUnique();

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_SalesTypeDefinition_IsDeleted");

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
