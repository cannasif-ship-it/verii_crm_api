using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using crm_api.Models;

namespace crm_api.Data.Configurations
{
    public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        private static readonly ValueConverter<DateTime, DateTime> UtcDateTimeConverter = new(
            value => value.Kind == DateTimeKind.Utc ? value : DateTime.SpecifyKind(value, DateTimeKind.Utc),
            value => value.Kind == DateTimeKind.Utc ? value : DateTime.SpecifyKind(value, DateTimeKind.Utc)
        );

        private static readonly ValueConverter<DateTime?, DateTime?> NullableUtcDateTimeConverter = new(
            value => value.HasValue
                ? (value.Value.Kind == DateTimeKind.Utc ? value.Value : DateTime.SpecifyKind(value.Value, DateTimeKind.Utc))
                : value,
            value => value.HasValue
                ? (value.Value.Kind == DateTimeKind.Utc ? value.Value : DateTime.SpecifyKind(value.Value, DateTimeKind.Utc))
                : value
        );

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            // Primary key configuration
            builder.HasKey(e => e.Id);

            // Primary key property configuration
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            // Base properties configuration
            builder.Property(e => e.CreatedDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()")
                .HasConversion(UtcDateTimeConverter);

            builder.Property(e => e.UpdatedDate)
                .IsRequired(false)
                .HasConversion(NullableUtcDateTimeConverter);

            builder.Property(e => e.DeletedDate)
                .IsRequired(false)
                .HasConversion(NullableUtcDateTimeConverter);

            builder.Property(e => e.IsDeleted)
                .IsRequired();

            // Audit fields are nullable long FKs; no MaxLength configuration

            // Foreign key relationships with NoAction to prevent cascade cycles
            builder.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.UpdatedByUser)
                .WithMany()
                .HasForeignKey(e => e.UpdatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.DeletedByUser)
                .WithMany()
                .HasForeignKey(e => e.DeletedBy)
                .OnDelete(DeleteBehavior.NoAction);

            // Global query filter for soft delete is applied on root entity types (e.g., BaseHeaderEntity)

            // Configure specific entity
            ConfigureEntity(builder);
        }

        protected abstract void ConfigureEntity(EntityTypeBuilder<T> builder);
    }
}
