using crm_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crm_api.Data.Configurations
{
    public class GoogleCustomerMailLogConfiguration : BaseEntityConfiguration<GoogleCustomerMailLog>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<GoogleCustomerMailLog> builder)
        {
            builder.ToTable("RII_GOOGLE_CUSTOMER_MAIL_LOGS");

            builder.Property(x => x.TenantId)
                .IsRequired();

            builder.Property(x => x.Provider)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.SenderEmail)
                .HasMaxLength(320);

            builder.Property(x => x.ToEmails)
                .HasMaxLength(4000)
                .IsRequired();

            builder.Property(x => x.CcEmails)
                .HasMaxLength(4000);

            builder.Property(x => x.BccEmails)
                .HasMaxLength(4000);

            builder.Property(x => x.Subject)
                .HasMaxLength(512)
                .IsRequired();

            builder.Property(x => x.Body)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            builder.Property(x => x.TemplateKey)
                .HasMaxLength(128);

            builder.Property(x => x.TemplateName)
                .HasMaxLength(256);

            builder.Property(x => x.TemplateVersion)
                .HasMaxLength(64);

            builder.Property(x => x.ErrorCode)
                .HasMaxLength(128);

            builder.Property(x => x.ErrorMessage)
                .HasMaxLength(2000);

            builder.Property(x => x.GoogleMessageId)
                .HasMaxLength(512);

            builder.Property(x => x.GoogleThreadId)
                .HasMaxLength(512);

            builder.Property(x => x.MetadataJson)
                .HasMaxLength(4000);

            builder.HasOne(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Contact)
                .WithMany()
                .HasForeignKey(x => x.ContactId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.SentByUser)
                .WithMany()
                .HasForeignKey(x => x.SentByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => x.TenantId)
                .HasDatabaseName("IX_GoogleCustomerMailLogs_TenantId");

            builder.HasIndex(x => x.CustomerId)
                .HasDatabaseName("IX_GoogleCustomerMailLogs_CustomerId");

            builder.HasIndex(x => x.ContactId)
                .HasDatabaseName("IX_GoogleCustomerMailLogs_ContactId");

            builder.HasIndex(x => x.SentByUserId)
                .HasDatabaseName("IX_GoogleCustomerMailLogs_SentByUserId");

            builder.HasIndex(x => x.IsSuccess)
                .HasDatabaseName("IX_GoogleCustomerMailLogs_IsSuccess");

            builder.HasIndex(x => x.CreatedDate)
                .HasDatabaseName("IX_GoogleCustomerMailLogs_CreatedDate");
        }
    }
}

