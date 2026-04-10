using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Modules.ReportBuilder.Infrastructure.Persistence.Configurations
{
    public class ReportAssignmentConfiguration : BaseEntityConfiguration<ReportAssignment>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ReportAssignment> builder)
        {
            builder.ToTable("RII_REPORT_ASSIGNMENTS");

            builder.Property(x => x.ReportDefinitionId).IsRequired();
            builder.Property(x => x.UserId).IsRequired();

            builder.HasOne(x => x.ReportDefinition)
                .WithMany(x => x.Assignments)
                .HasForeignKey(x => x.ReportDefinitionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.ReportDefinitionId, x.UserId })
                .HasDatabaseName("IX_RII_REPORT_ASSIGNMENTS_ReportDefinitionId_UserId")
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_RII_REPORT_ASSIGNMENTS_UserId");
        }
    }
}
