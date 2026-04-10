using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Shared.Infrastructure.Persistence.Configurations;

namespace crm_api.Modules.Demand.Infrastructure.Persistence.Configurations
{
    public class DemandNotesConfiguration : BaseEntityConfiguration<DemandNotes>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<DemandNotes> builder)
        {
            builder.ToTable("RII_DEMAND_NOTES");

            builder.Property(e => e.DemandId).IsRequired();

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

            builder.HasOne(e => e.Demand)
                .WithOne(d => d.DemandNotes)
                .HasForeignKey<DemandNotes>(e => e.DemandId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.DemandId).IsUnique().HasDatabaseName("IX_DemandNotes_DemandId");
            builder.HasIndex(e => e.IsDeleted).HasDatabaseName("IX_DemandNotes_IsDeleted");
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
