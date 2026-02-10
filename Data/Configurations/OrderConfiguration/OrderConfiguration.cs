using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crm_api.Models;

namespace crm_api.Data.Configurations
{
    public class OrderConfiguration : BaseEntityConfiguration<Order>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Order> builder)
        {
            // Table name
            builder.ToTable("RII_ORDER");

            // Potential Customer relationship
            builder.Property(e => e.PotentialCustomerId)
                .IsRequired(false);

            builder.HasOne(e => e.PotentialCustomer)
                .WithMany()
                .HasForeignKey(e => e.PotentialCustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            // ERP Customer Code
            builder.Property(e => e.ErpCustomerCode)
                .HasMaxLength(50)
                .IsRequired(false);

            // Contact relationship
            builder.Property(e => e.ContactId)
                .IsRequired(false);

            builder.HasOne(e => e.Contact)
                .WithMany()
                .HasForeignKey(e => e.ContactId)
                .OnDelete(DeleteBehavior.NoAction);

            // Valid Until
            builder.Property(e => e.ValidUntil)
                .IsRequired(false);

            // Delivery Date
            builder.Property(e => e.DeliveryDate)
                .IsRequired(false);

            // Shipping Address relationship
            builder.Property(e => e.ShippingAddressId)
                .IsRequired(false);

            builder.HasOne(e => e.ShippingAddress)
                .WithMany()
                .HasForeignKey(e => e.ShippingAddressId)
                .OnDelete(DeleteBehavior.NoAction);

            // Representative relationship
            builder.Property(e => e.RepresentativeId)
                .IsRequired(false);

            builder.HasOne(e => e.Representative)
                .WithMany()
                .HasForeignKey(e => e.RepresentativeId)
                .OnDelete(DeleteBehavior.NoAction);

            // Activity relationship
            builder.Property(e => e.ActivityId)
                .IsRequired(false);

            builder.HasOne(e => e.Activity)
                .WithMany()
                .HasForeignKey(e => e.ActivityId)
                .OnDelete(DeleteBehavior.NoAction);

            // Status
            builder.Property(e => e.Status)
                .IsRequired(false);

            // Description
            builder.Property(e => e.Description)
                .HasMaxLength(500)
                .IsRequired(false);

            // Payment Type relationship
            builder.Property(e => e.PaymentTypeId)
                .IsRequired(false);

            builder.HasOne(e => e.PaymentType)
                .WithMany()
                .HasForeignKey(e => e.PaymentTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            // Document Serial Type relationship
            builder.Property(e => e.DocumentSerialTypeId)
                .IsRequired();

            builder.HasOne(e => e.DocumentSerialType)
                .WithMany()
                .HasForeignKey(e => e.DocumentSerialTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            // Offer Type
            builder.Property(e => e.OfferType)
                .HasMaxLength(50)
                .IsRequired();

            // Offer Date
            builder.Property(e => e.OfferDate)
                .IsRequired(false);

            // Offer No
            builder.Property(e => e.OfferNo)
                .HasMaxLength(50)
                .IsRequired(false);

            // Revision No
            builder.Property(e => e.RevisionNo)
                .HasMaxLength(50)
                .IsRequired(false);

            // Revision Id
            builder.Property(e => e.RevisionId)
                .IsRequired(false);

            // Currency
            builder.Property(e => e.Currency)
                .HasMaxLength(50)
                .IsRequired();

            // Has Customer Specific Discount
            builder.Property(e => e.HasCustomerSpecificDiscount)
                .IsRequired()
                .HasDefaultValue(false);

            // General Discount Rate
            builder.Property(e => e.GeneralDiscountRate)
                .HasColumnType("decimal(18,6)")
                .IsRequired(false);

            // General Discount Amount
            builder.Property(e => e.GeneralDiscountAmount)
                .HasColumnType("decimal(18,6)")
                .IsRequired(false);

            // Total (KDV hariç)
            builder.Property(e => e.Total)
                .HasColumnType("decimal(18,6)")
                .IsRequired()
                .HasDefaultValue(0m);

            // Grand Total (KDV dahil)
            builder.Property(e => e.GrandTotal)
                .HasColumnType("decimal(18,6)")
                .IsRequired()
                .HasDefaultValue(0m);

            // Quotation relationship
            builder.Property(e => e.QuotationId)
                .IsRequired(false);

            builder.HasOne(e => e.Quotation)
                .WithMany()
                .HasForeignKey(e => e.QuotationId)
                .OnDelete(DeleteBehavior.NoAction);

            // Navigation - Lines
            builder.HasMany(e => e.Lines)
                .WithOne(l => l.Order)
                .HasForeignKey(l => l.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // BaseHeaderEntity fields (approval & ERP integration)
            builder.Property(e => e.Year)
                .HasMaxLength(4)
                .IsRequired();

            builder.Property(e => e.CompletionDate)
                .IsRequired(false);

            builder.Property(e => e.IsCompleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.IsPendingApproval)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.ApprovalStatus)
                .IsRequired(false);

            builder.Property(e => e.RejectedReason)
                .HasMaxLength(250)
                .IsRequired(false);

            builder.Property(e => e.ApprovedByUserId)
                .IsRequired(false);

            builder.HasOne(e => e.ApprovedByUser)
                .WithMany()
                .HasForeignKey(e => e.ApprovedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(e => e.ApprovalDate)
                .IsRequired(false);

            builder.Property(e => e.IsERPIntegrated)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.ERPIntegrationNumber)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(e => e.LastSyncDate)
                .IsRequired(false);

            builder.Property(e => e.CountTriedBy)
                .IsRequired()
                .HasDefaultValue(0);

            // Indexes
            builder.HasIndex(e => e.Year)
                .HasDatabaseName("IX_Order_Year");

            builder.HasIndex(e => e.PotentialCustomerId)
                .HasDatabaseName("IX_Order_PotentialCustomerId");

            builder.HasIndex(e => e.ContactId)
                .HasDatabaseName("IX_Order_ContactId");

            builder.HasIndex(e => e.ShippingAddressId)
                .HasDatabaseName("IX_Order_ShippingAddressId");

            builder.HasIndex(e => e.RepresentativeId)
                .HasDatabaseName("IX_Order_RepresentativeId");

            builder.HasIndex(e => e.ActivityId)
                .HasDatabaseName("IX_Order_ActivityId");

            builder.HasIndex(e => e.PaymentTypeId)
                .HasDatabaseName("IX_Order_PaymentTypeId");

            builder.HasIndex(e => e.DocumentSerialTypeId)
                .HasDatabaseName("IX_Order_DocumentSerialTypeId");

            builder.HasIndex(e => e.QuotationId)
                .HasDatabaseName("IX_Order_QuotationId");

            builder.HasIndex(e => e.Status)
                .HasDatabaseName("IX_Order_Status");

            builder.HasIndex(e => e.OfferDate)
                .HasDatabaseName("IX_Order_OfferDate");

            builder.HasIndex(e => e.OfferNo)
                .HasDatabaseName("IX_Order_OfferNo");

            builder.HasIndex(e => e.ValidUntil)
                .HasDatabaseName("IX_Order_ValidUntil");

            builder.HasIndex(e => e.DeliveryDate)
                .HasDatabaseName("IX_Order_DeliveryDate");

            builder.HasIndex(e => e.ApprovedByUserId)
                .HasDatabaseName("IX_Order_ApprovedByUserId");

            builder.HasIndex(e => e.ApprovalStatus)
                .HasDatabaseName("IX_Order_ApprovalStatus");

            builder.HasIndex(e => e.ApprovalDate)
                .HasDatabaseName("IX_Order_ApprovalDate");

            builder.HasIndex(e => e.IsCompleted)
                .HasDatabaseName("IX_Order_IsCompleted");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_Order_IsDeleted");

            // Global Query Filter for soft delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
