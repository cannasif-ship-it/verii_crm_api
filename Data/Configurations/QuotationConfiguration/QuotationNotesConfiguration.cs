using crm_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Data.Configurations
{
    public class QuotationNotesConfiguration : BaseEntityConfiguration<QuotationNotes>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<QuotationNotes> builder)
        {
            builder.ToTable("RII_QUOTATION_NOTES");

            builder.Property(e => e.QuotationId)
                .IsRequired();

            builder.Property(e => e.Note1).HasMaxLength(100).IsRequired(false);
            builder.Property(e => e.Note2).HasMaxLength(100).IsRequired(false);
            builder.Property(e => e.Note3).HasMaxLength(100).IsRequired(false);
            builder.Property(e => e.Note4).HasMaxLength(100).IsRequired(false);
            builder.Property(e => e.Note5).HasMaxLength(100).IsRequired(false);
            builder.Property(e => e.Note6).HasMaxLength(100).IsRequired(false);
            builder.Property(e => e.Note7).HasMaxLength(100).IsRequired(false);
            builder.Property(e => e.Note8).HasMaxLength(100).IsRequired(false);
            builder.Property(e => e.Note9).HasMaxLength(100).IsRequired(false);
            builder.Property(e => e.Note10).HasMaxLength(100).IsRequired(false);
            builder.Property(e => e.Note11).HasMaxLength(100).IsRequired(false);
            builder.Property(e => e.Note12).HasMaxLength(100).IsRequired(false);
            builder.Property(e => e.Note13).HasMaxLength(100).IsRequired(false);
            builder.Property(e => e.Note14).HasMaxLength(100).IsRequired(false);
            builder.Property(e => e.Note15).HasMaxLength(100).IsRequired(false);

            builder.HasOne(e => e.Quotation)
                .WithOne(q => q.QuotationNotes)
                .HasForeignKey<QuotationNotes>(e => e.QuotationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.QuotationId)
                .IsUnique()
                .HasDatabaseName("IX_QuotationNotes_QuotationId");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_QuotationNotes_IsDeleted");

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
