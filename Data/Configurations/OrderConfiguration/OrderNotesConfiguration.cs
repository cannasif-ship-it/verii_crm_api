using crm_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Data.Configurations
{
    public class OrderNotesConfiguration : BaseEntityConfiguration<OrderNotes>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<OrderNotes> builder)
        {
            builder.ToTable("RII_ORDER_NOTES");

            builder.Property(e => e.OrderId).IsRequired();

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

            builder.HasOne(e => e.Order)
                .WithOne(o => o.OrderNotes)
                .HasForeignKey<OrderNotes>(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.OrderId).IsUnique().HasDatabaseName("IX_OrderNotes_OrderId");
            builder.HasIndex(e => e.IsDeleted).HasDatabaseName("IX_OrderNotes_IsDeleted");
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
