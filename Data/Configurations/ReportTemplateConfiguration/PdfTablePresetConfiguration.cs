using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models;

namespace crm_api.Data.Configurations
{
    public class PdfTablePresetConfiguration : IEntityTypeConfiguration<PdfTablePreset>
    {
        public void Configure(EntityTypeBuilder<PdfTablePreset> builder)
        {
            builder.ToTable("RII_PDF_TABLE_PRESETS");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.RuleType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Key)
                .IsRequired()
                .HasMaxLength(120);

            builder.Property(x => x.ColumnsJson)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(x => x.TableOptionsJson)
                .HasColumnType("nvarchar(max)");

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasIndex(x => x.Key)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0")
                .HasDatabaseName("IX_RII_PDF_TABLE_PRESETS_Key");

            builder.HasIndex(x => new { x.RuleType, x.IsActive })
                .HasDatabaseName("IX_RII_PDF_TABLE_PRESETS_RuleType_IsActive");
        }
    }
}
