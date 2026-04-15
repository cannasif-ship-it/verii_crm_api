using crm_api.Modules.Definitions.Domain.Entities;
using crm_api.Shared.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Modules.Definitions.Infrastructure.Persistence.Configurations
{
    public class SalesRepCodeUserMatchConfiguration : BaseEntityConfiguration<SalesRepCodeUserMatch>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SalesRepCodeUserMatch> builder)
        {
            builder.ToTable("RII_SalepRep_Code_User_Matches");

            builder.Property(e => e.SalesRepCodeId)
                .IsRequired();

            builder.Property(e => e.UserId)
                .IsRequired();

            builder.HasOne(e => e.SalesRepCode)
                .WithMany(e => e.UserMatches)
                .HasForeignKey(e => e.SalesRepCodeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.SalesRepCodeId, e.UserId })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0")
                .HasDatabaseName("IX_SalesRepCodeUserMatch_SalesRepCodeId_UserId");

            builder.HasIndex(e => e.UserId)
                .HasDatabaseName("IX_SalesRepCodeUserMatch_UserId");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_SalesRepCodeUserMatch_IsDeleted");

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
