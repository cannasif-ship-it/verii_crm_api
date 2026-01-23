using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models;

namespace crm_api.Data.Configurations
{
    public class UserConfiguration : BaseEntityConfiguration<User>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<User> builder)
        {
            // Table name
            builder.ToTable("RII_USERS");

            // Properties configuration
            builder.Property(u => u.FirstName)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.PhoneNumber)
                .IsRequired(false)
                .HasMaxLength(20);

            builder.Property(u => u.RoleId)
                .IsRequired();

            builder.Property(u => u.RefreshToken)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(u => u.RefreshTokenExpiryTime)
                .IsRequired(false);

            builder.Property(u => u.LastLoginDate)
                .IsRequired(false);

            // Ignore FullName computed property
            builder.Ignore(u => u.FullName);

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);

            builder.Property(u => u.IsEmailConfirmed)
                .HasDefaultValue(false);

            // Relationships
            builder.HasOne(u => u.RoleNavigation)
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            // Indexes
            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email");

            builder.HasIndex(u => u.Username)
                .IsUnique()
                .HasDatabaseName("IX_Users_Username");

            builder.HasIndex(u => u.IsDeleted)
                .HasDatabaseName("IX_Users_IsDeleted");

            // Deterministic admin seed for first system access
            // Note: CreatedDate will use DB default (GETUTCDATE()) to reflect current UTC time at insert.
            builder.HasData(new User
            {
                Id = 1,
                Username = "admin@v3rii.com",
                Email = "v3rii@v3rii.com",
                // Deterministic bcrypt hash using fixed salt so migrations remain stable
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(
                    "Veriipass123!",
                    "$2a$11$abcdefghijklmnopqrstuv"
                ),
                FirstName = "Admin",
                LastName = "User",
                RoleId = 3, // Admin role
                IsEmailConfirmed = true,
                IsActive = true,
                CreatedDate = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false
            });

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
