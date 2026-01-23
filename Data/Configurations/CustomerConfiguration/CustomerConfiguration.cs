using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models;

namespace crm_api.Data.Configurations
{
    public class CustomerConfiguration : BaseEntityConfiguration<Customer>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Customer> builder)
        {
            // Table name
            builder.ToTable("RII_CUSTOMER");

            // Basic Information
            builder.Property(e => e.CustomerCode)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(e => e.CustomerName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(e => e.CustomerTypeId)
                .IsRequired(false);

            // Tax Information
            builder.Property(e => e.TaxOffice)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(e => e.TaxNumber)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(e => e.TcknNumber)
                .HasMaxLength(11)
                .IsRequired(false);

            // Classification
            builder.Property(e => e.SalesRepCode)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(e => e.GroupCode)
                .HasMaxLength(50)
                .IsRequired(false);

            // Financial Information
            builder.Property(e => e.CreditLimit)
                .HasColumnType("decimal(18,6)")
                .IsRequired(false);

            // ERP Integration
            builder.Property(e => e.BranchCode)
                .IsRequired();

            builder.Property(e => e.BusinessUnitCode)
                .IsRequired();

            // Contact Information
            builder.Property(e => e.Notes)
                .HasMaxLength(250)
                .IsRequired(false);

            builder.Property(e => e.Email)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(e => e.Website)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(e => e.Phone1)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(e => e.Phone2)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(e => e.Address)
                .HasMaxLength(500)
                .IsRequired(false);

            // Location Information
            builder.Property(e => e.CountryId)
                .IsRequired(false);

            builder.Property(e => e.CityId)
                .IsRequired(false);

            builder.Property(e => e.DistrictId)
                .IsRequired(false);

            // Foreign Key Relationships
            builder.HasOne(e => e.Countries)
                .WithMany()
                .HasForeignKey(e => e.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Cities)
                .WithMany()
                .HasForeignKey(e => e.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Districts)
                .WithMany()
                .HasForeignKey(e => e.DistrictId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.CustomerTypes)
                .WithMany(ct => ct.Customers)
                .HasForeignKey(e => e.CustomerTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            // Indexes
            builder.HasIndex(e => e.CustomerCode)
                .IsUnique()
                .HasFilter("[CustomerCode] IS NOT NULL")
                .HasDatabaseName("IX_Customer_CustomerCode");

            builder.HasIndex(e => e.TaxNumber)
                .HasFilter("[TaxNumber] IS NOT NULL")
                .HasDatabaseName("IX_Customer_TaxNumber");

            builder.HasIndex(e => e.TcknNumber)
                .HasFilter("[TcknNumber] IS NOT NULL")
                .HasDatabaseName("IX_Customer_TcknNumber");

            builder.HasIndex(e => e.Email)
                .HasFilter("[Email] IS NOT NULL")
                .HasDatabaseName("IX_Customer_Email");

            builder.HasIndex(e => e.CountryId)
                .HasDatabaseName("IX_Customer_CountryId");

            builder.HasIndex(e => e.CityId)
                .HasDatabaseName("IX_Customer_CityId");

            builder.HasIndex(e => e.DistrictId)
                .HasDatabaseName("IX_Customer_DistrictId");

            builder.HasIndex(e => e.CustomerTypeId)
                .HasDatabaseName("IX_Customer_CustomerTypeId");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_Customer_IsDeleted");

            // BaseHeaderEntity fields (approval & ERP integration)
            builder.Property(e => e.CompletionDate)
                .HasColumnName("COMPLETION_DATE");

            builder.Property(e => e.IsCompleted)
                .HasColumnName("IS_COMPLETED")
                .HasDefaultValue(false);

            builder.Property(e => e.IsPendingApproval)
                .HasColumnName("IS_PENDING_APPROVAL")
                .HasDefaultValue(false);

            builder.Property(e => e.ApprovalStatus)
                .HasColumnName("APPROVAL_STATUS");

            builder.Property(e => e.RejectedReason)
                .HasColumnName("REJECTED_REASON")
                .HasMaxLength(250);

            builder.Property(e => e.ApprovedByUserId)
                .HasColumnName("APPROVED_BY_USER_ID");

            builder.HasOne(e => e.ApprovedByUser)
                .WithMany()
                .HasForeignKey(e => e.ApprovedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(e => e.ApprovalDate)
                .HasColumnName("APPROVAL_DATE");

            builder.Property(e => e.IsERPIntegrated)
                .HasColumnName("IS_ERP_INTEGRATED")
                .HasDefaultValue(false);

            builder.Property(e => e.ERPIntegrationNumber)
                .HasColumnName("ERP_INTEGRATION_NUMBER")
                .HasMaxLength(100);

            builder.Property(e => e.LastSyncDate)
                .HasColumnName("LAST_SYNC_DATE");

            builder.Property(e => e.CountTriedBy)
                .HasColumnName("COUNT_TRIED_BY")
                .HasDefaultValue(0);

            // Helpful indexes for workflow fields
            builder.HasIndex(e => e.ApprovedByUserId);
            builder.HasIndex(e => e.ApprovalStatus);
            builder.HasIndex(e => e.ApprovalDate);
            builder.HasIndex(e => e.IsCompleted);

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
