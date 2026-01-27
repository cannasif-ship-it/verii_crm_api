using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_DEMAND",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PotentialCustomerId = table.Column<long>(type: "bigint", nullable: true),
                    ErpCustomerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippingAddressId = table.Column<long>(type: "bigint", nullable: true),
                    RepresentativeId = table.Column<long>(type: "bigint", nullable: true),
                    ActivityId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PaymentTypeId = table.Column<long>(type: "bigint", nullable: true),
                    DocumentSerialTypeId = table.Column<long>(type: "bigint", nullable: false),
                    OfferType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OfferDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfferNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RevisionNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RevisionId = table.Column<long>(type: "bigint", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HasCustomerSpecificDiscount = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Total = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 0m),
                    GrandTotal = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 0m),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    Year = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPendingApproval = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ApprovalStatus = table.Column<bool>(type: "bit", nullable: true),
                    RejectedReason = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsERPIntegrated = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ERPIntegrationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastSyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CountTriedBy = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_DEMAND", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_RII_ACTIVITY_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "RII_ACTIVITY",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_RII_CONTACT_ContactId",
                        column: x => x.ContactId,
                        principalTable: "RII_CONTACT",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_RII_CUSTOMER_PotentialCustomerId",
                        column: x => x.PotentialCustomerId,
                        principalTable: "RII_CUSTOMER",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_RII_DOCUMENT_SERIAL_TYPE_DocumentSerialTypeId",
                        column: x => x.DocumentSerialTypeId,
                        principalTable: "RII_DOCUMENT_SERIAL_TYPE",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_RII_PAYMENT_TYPE_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "RII_PAYMENT_TYPE",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_RII_SHIPPING_ADDRESS_ShippingAddressId",
                        column: x => x.ShippingAddressId,
                        principalTable: "RII_SHIPPING_ADDRESS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_RII_USERS_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_RII_USERS_RepresentativeId",
                        column: x => x.RepresentativeId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_ORDER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PotentialCustomerId = table.Column<long>(type: "bigint", nullable: true),
                    ErpCustomerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippingAddressId = table.Column<long>(type: "bigint", nullable: true),
                    RepresentativeId = table.Column<long>(type: "bigint", nullable: true),
                    ActivityId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PaymentTypeId = table.Column<long>(type: "bigint", nullable: true),
                    DocumentSerialTypeId = table.Column<long>(type: "bigint", nullable: false),
                    OfferType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OfferDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfferNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RevisionNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RevisionId = table.Column<long>(type: "bigint", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HasCustomerSpecificDiscount = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Total = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 0m),
                    GrandTotal = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 0m),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    Year = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPendingApproval = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ApprovalStatus = table.Column<bool>(type: "bit", nullable: true),
                    RejectedReason = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsERPIntegrated = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ERPIntegrationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastSyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CountTriedBy = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_ORDER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_ORDER_RII_ACTIVITY_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "RII_ACTIVITY",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_ORDER_RII_CONTACT_ContactId",
                        column: x => x.ContactId,
                        principalTable: "RII_CONTACT",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_ORDER_RII_CUSTOMER_PotentialCustomerId",
                        column: x => x.PotentialCustomerId,
                        principalTable: "RII_CUSTOMER",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_ORDER_RII_DOCUMENT_SERIAL_TYPE_DocumentSerialTypeId",
                        column: x => x.DocumentSerialTypeId,
                        principalTable: "RII_DOCUMENT_SERIAL_TYPE",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_ORDER_RII_PAYMENT_TYPE_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "RII_PAYMENT_TYPE",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_ORDER_RII_SHIPPING_ADDRESS_ShippingAddressId",
                        column: x => x.ShippingAddressId,
                        principalTable: "RII_SHIPPING_ADDRESS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_ORDER_RII_USERS_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_ORDER_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_ORDER_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_ORDER_RII_USERS_RepresentativeId",
                        column: x => x.RepresentativeId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_ORDER_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_DEMAND_EXCHANGE_RATE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemandId = table.Column<long>(type: "bigint", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ExchangeRateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsOfficial = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_DEMAND_EXCHANGE_RATE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_EXCHANGE_RATE_RII_DEMAND_DemandId",
                        column: x => x.DemandId,
                        principalTable: "RII_DEMAND",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_EXCHANGE_RATE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_EXCHANGE_RATE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_EXCHANGE_RATE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_DEMAND_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemandId = table.Column<long>(type: "bigint", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DiscountRate1 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DiscountAmount1 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DiscountRate2 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DiscountAmount2 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DiscountRate3 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DiscountAmount3 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    VatRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    VatAmount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    LineGrandTotal = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    PricingRuleHeaderId = table.Column<long>(type: "bigint", nullable: true),
                    RelatedStockId = table.Column<long>(type: "bigint", nullable: true),
                    RelatedProductKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsMainRelatedProduct = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_DEMAND_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_LINE_RII_DEMAND_DemandId",
                        column: x => x.DemandId,
                        principalTable: "RII_DEMAND",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_LINE_RII_PRICING_RULE_HEADER_PricingRuleHeaderId",
                        column: x => x.PricingRuleHeaderId,
                        principalTable: "RII_PRICING_RULE_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_LINE_RII_STOCK_RelatedStockId",
                        column: x => x.RelatedStockId,
                        principalTable: "RII_STOCK",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_LINE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_LINE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_LINE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_ORDER_EXCHANGE_RATE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ExchangeRateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsOfficial = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_ORDER_EXCHANGE_RATE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_ORDER_EXCHANGE_RATE_RII_ORDER_OrderId",
                        column: x => x.OrderId,
                        principalTable: "RII_ORDER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RII_ORDER_EXCHANGE_RATE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_ORDER_EXCHANGE_RATE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_ORDER_EXCHANGE_RATE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_ORDER_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DiscountRate1 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DiscountAmount1 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DiscountRate2 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DiscountAmount2 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DiscountRate3 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DiscountAmount3 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    VatRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    VatAmount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    LineGrandTotal = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    PricingRuleHeaderId = table.Column<long>(type: "bigint", nullable: true),
                    RelatedStockId = table.Column<long>(type: "bigint", nullable: true),
                    RelatedProductKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsMainRelatedProduct = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_ORDER_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_ORDER_LINE_RII_ORDER_OrderId",
                        column: x => x.OrderId,
                        principalTable: "RII_ORDER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RII_ORDER_LINE_RII_PRICING_RULE_HEADER_PricingRuleHeaderId",
                        column: x => x.PricingRuleHeaderId,
                        principalTable: "RII_PRICING_RULE_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_RII_ORDER_LINE_RII_STOCK_RelatedStockId",
                        column: x => x.RelatedStockId,
                        principalTable: "RII_STOCK",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_RII_ORDER_LINE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_ORDER_LINE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_ORDER_LINE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Demand_ActivityId",
                table: "RII_DEMAND",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_ApprovalDate",
                table: "RII_DEMAND",
                column: "ApprovalDate");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_ApprovalStatus",
                table: "RII_DEMAND",
                column: "ApprovalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_ApprovedByUserId",
                table: "RII_DEMAND",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_ContactId",
                table: "RII_DEMAND",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_DeliveryDate",
                table: "RII_DEMAND",
                column: "DeliveryDate");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_DocumentSerialTypeId",
                table: "RII_DEMAND",
                column: "DocumentSerialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_IsCompleted",
                table: "RII_DEMAND",
                column: "IsCompleted");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_IsDeleted",
                table: "RII_DEMAND",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_OfferDate",
                table: "RII_DEMAND",
                column: "OfferDate");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_OfferNo",
                table: "RII_DEMAND",
                column: "OfferNo");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_PaymentTypeId",
                table: "RII_DEMAND",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_PotentialCustomerId",
                table: "RII_DEMAND",
                column: "PotentialCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_RepresentativeId",
                table: "RII_DEMAND",
                column: "RepresentativeId");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_ShippingAddressId",
                table: "RII_DEMAND",
                column: "ShippingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_Status",
                table: "RII_DEMAND",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_ValidUntil",
                table: "RII_DEMAND",
                column: "ValidUntil");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_Year",
                table: "RII_DEMAND",
                column: "Year");

            migrationBuilder.CreateIndex(
                name: "IX_RII_DEMAND_CreatedBy",
                table: "RII_DEMAND",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_DEMAND_DeletedBy",
                table: "RII_DEMAND",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_DEMAND_UpdatedBy",
                table: "RII_DEMAND",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DemandExchangeRate_Currency",
                table: "RII_DEMAND_EXCHANGE_RATE",
                column: "Currency");

            migrationBuilder.CreateIndex(
                name: "IX_DemandExchangeRate_DemandId",
                table: "RII_DEMAND_EXCHANGE_RATE",
                column: "DemandId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandExchangeRate_ExchangeRateDate",
                table: "RII_DEMAND_EXCHANGE_RATE",
                column: "ExchangeRateDate");

            migrationBuilder.CreateIndex(
                name: "IX_DemandExchangeRate_IsDeleted",
                table: "RII_DEMAND_EXCHANGE_RATE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_DemandExchangeRate_IsOfficial",
                table: "RII_DEMAND_EXCHANGE_RATE",
                column: "IsOfficial");

            migrationBuilder.CreateIndex(
                name: "IX_RII_DEMAND_EXCHANGE_RATE_CreatedBy",
                table: "RII_DEMAND_EXCHANGE_RATE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_DEMAND_EXCHANGE_RATE_DeletedBy",
                table: "RII_DEMAND_EXCHANGE_RATE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_DEMAND_EXCHANGE_RATE_UpdatedBy",
                table: "RII_DEMAND_EXCHANGE_RATE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DemandLine_DemandId",
                table: "RII_DEMAND_LINE",
                column: "DemandId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandLine_IsDeleted",
                table: "RII_DEMAND_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_DemandLine_PricingRuleHeaderId",
                table: "RII_DEMAND_LINE",
                column: "PricingRuleHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandLine_ProductCode",
                table: "RII_DEMAND_LINE",
                column: "ProductCode");

            migrationBuilder.CreateIndex(
                name: "IX_DemandLine_RelatedStockId",
                table: "RII_DEMAND_LINE",
                column: "RelatedStockId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_DEMAND_LINE_CreatedBy",
                table: "RII_DEMAND_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_DEMAND_LINE_DeletedBy",
                table: "RII_DEMAND_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_DEMAND_LINE_UpdatedBy",
                table: "RII_DEMAND_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ActivityId",
                table: "RII_ORDER",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ApprovalDate",
                table: "RII_ORDER",
                column: "ApprovalDate");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ApprovalStatus",
                table: "RII_ORDER",
                column: "ApprovalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ApprovedByUserId",
                table: "RII_ORDER",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ContactId",
                table: "RII_ORDER",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_DeliveryDate",
                table: "RII_ORDER",
                column: "DeliveryDate");

            migrationBuilder.CreateIndex(
                name: "IX_Order_DocumentSerialTypeId",
                table: "RII_ORDER",
                column: "DocumentSerialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_IsCompleted",
                table: "RII_ORDER",
                column: "IsCompleted");

            migrationBuilder.CreateIndex(
                name: "IX_Order_IsDeleted",
                table: "RII_ORDER",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Order_OfferDate",
                table: "RII_ORDER",
                column: "OfferDate");

            migrationBuilder.CreateIndex(
                name: "IX_Order_OfferNo",
                table: "RII_ORDER",
                column: "OfferNo");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PaymentTypeId",
                table: "RII_ORDER",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PotentialCustomerId",
                table: "RII_ORDER",
                column: "PotentialCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_RepresentativeId",
                table: "RII_ORDER",
                column: "RepresentativeId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ShippingAddressId",
                table: "RII_ORDER",
                column: "ShippingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Status",
                table: "RII_ORDER",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ValidUntil",
                table: "RII_ORDER",
                column: "ValidUntil");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Year",
                table: "RII_ORDER",
                column: "Year");

            migrationBuilder.CreateIndex(
                name: "IX_RII_ORDER_CreatedBy",
                table: "RII_ORDER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_ORDER_DeletedBy",
                table: "RII_ORDER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_ORDER_UpdatedBy",
                table: "RII_ORDER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_OrderExchangeRate_Currency",
                table: "RII_ORDER_EXCHANGE_RATE",
                column: "Currency");

            migrationBuilder.CreateIndex(
                name: "IX_OrderExchangeRate_ExchangeRateDate",
                table: "RII_ORDER_EXCHANGE_RATE",
                column: "ExchangeRateDate");

            migrationBuilder.CreateIndex(
                name: "IX_OrderExchangeRate_IsDeleted",
                table: "RII_ORDER_EXCHANGE_RATE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_OrderExchangeRate_IsOfficial",
                table: "RII_ORDER_EXCHANGE_RATE",
                column: "IsOfficial");

            migrationBuilder.CreateIndex(
                name: "IX_OrderExchangeRate_OrderId",
                table: "RII_ORDER_EXCHANGE_RATE",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_ORDER_EXCHANGE_RATE_CreatedBy",
                table: "RII_ORDER_EXCHANGE_RATE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_ORDER_EXCHANGE_RATE_DeletedBy",
                table: "RII_ORDER_EXCHANGE_RATE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_ORDER_EXCHANGE_RATE_UpdatedBy",
                table: "RII_ORDER_EXCHANGE_RATE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLine_IsDeleted",
                table: "RII_ORDER_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLine_OrderId",
                table: "RII_ORDER_LINE",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLine_PricingRuleHeaderId",
                table: "RII_ORDER_LINE",
                column: "PricingRuleHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLine_ProductCode",
                table: "RII_ORDER_LINE",
                column: "ProductCode");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLine_RelatedStockId",
                table: "RII_ORDER_LINE",
                column: "RelatedStockId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_ORDER_LINE_CreatedBy",
                table: "RII_ORDER_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_ORDER_LINE_DeletedBy",
                table: "RII_ORDER_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_ORDER_LINE_UpdatedBy",
                table: "RII_ORDER_LINE",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_DEMAND_EXCHANGE_RATE");

            migrationBuilder.DropTable(
                name: "RII_DEMAND_LINE");

            migrationBuilder.DropTable(
                name: "RII_ORDER_EXCHANGE_RATE");

            migrationBuilder.DropTable(
                name: "RII_ORDER_LINE");

            migrationBuilder.DropTable(
                name: "RII_DEMAND");

            migrationBuilder.DropTable(
                name: "RII_ORDER");
        }
    }
}
