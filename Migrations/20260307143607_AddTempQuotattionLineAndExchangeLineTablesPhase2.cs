using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddTempQuotattionLineAndExchangeLineTablesPhase2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_TEMP_QUOTATION");

            migrationBuilder.CreateTable(
                name: "RII_TEMP_QUOTATTION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    OfferDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 1m),
                    DiscountRate1 = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 0m),
                    DiscountRate2 = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 0m),
                    DiscountRate3 = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 0m),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, defaultValue: ""),
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
                    table.PrimaryKey("PK_RII_TEMP_QUOTATTION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_TEMP_QUOTATTION_RII_CUSTOMER_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "RII_CUSTOMER",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_TEMP_QUOTATTION_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_TEMP_QUOTATTION_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_TEMP_QUOTATTION_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_TEMP_QUOTATTION_EXCHANGE_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TempQuotattionId = table.Column<long>(type: "bigint", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ExchangeRateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsManual = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
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
                    table.PrimaryKey("PK_RII_TEMP_QUOTATTION_EXCHANGE_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_TEMP_QUOTATTION_EXCHANGE_LINE_RII_TEMP_QUOTATTION_TempQuotattionId",
                        column: x => x.TempQuotattionId,
                        principalTable: "RII_TEMP_QUOTATTION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RII_TEMP_QUOTATTION_EXCHANGE_LINE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_TEMP_QUOTATTION_EXCHANGE_LINE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_TEMP_QUOTATTION_EXCHANGE_LINE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_TEMP_QUOTATTION_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TempQuotattionId = table.Column<long>(type: "bigint", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
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
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, defaultValue: ""),
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
                    table.PrimaryKey("PK_RII_TEMP_QUOTATTION_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_TEMP_QUOTATTION_LINE_RII_TEMP_QUOTATTION_TempQuotattionId",
                        column: x => x.TempQuotattionId,
                        principalTable: "RII_TEMP_QUOTATTION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RII_TEMP_QUOTATTION_LINE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_TEMP_QUOTATTION_LINE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_TEMP_QUOTATTION_LINE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATTION_CreatedBy",
                table: "RII_TEMP_QUOTATTION",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATTION_CreatedDate",
                table: "RII_TEMP_QUOTATTION",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATTION_CustomerId",
                table: "RII_TEMP_QUOTATTION",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATTION_DeletedBy",
                table: "RII_TEMP_QUOTATTION",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATTION_IsApproved",
                table: "RII_TEMP_QUOTATTION",
                column: "IsApproved");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATTION_UpdatedBy",
                table: "RII_TEMP_QUOTATTION",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATTION_EXCHANGE_LINE_CreatedBy",
                table: "RII_TEMP_QUOTATTION_EXCHANGE_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATTION_EXCHANGE_LINE_Currency",
                table: "RII_TEMP_QUOTATTION_EXCHANGE_LINE",
                column: "Currency");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATTION_EXCHANGE_LINE_DeletedBy",
                table: "RII_TEMP_QUOTATTION_EXCHANGE_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATTION_EXCHANGE_LINE_IsDeleted",
                table: "RII_TEMP_QUOTATTION_EXCHANGE_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATTION_EXCHANGE_LINE_TempQuotattionId",
                table: "RII_TEMP_QUOTATTION_EXCHANGE_LINE",
                column: "TempQuotattionId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATTION_EXCHANGE_LINE_UpdatedBy",
                table: "RII_TEMP_QUOTATTION_EXCHANGE_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATTION_LINE_CreatedBy",
                table: "RII_TEMP_QUOTATTION_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATTION_LINE_DeletedBy",
                table: "RII_TEMP_QUOTATTION_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATTION_LINE_IsDeleted",
                table: "RII_TEMP_QUOTATTION_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATTION_LINE_ProductCode",
                table: "RII_TEMP_QUOTATTION_LINE",
                column: "ProductCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATTION_LINE_TempQuotattionId",
                table: "RII_TEMP_QUOTATTION_LINE",
                column: "TempQuotattionId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATTION_LINE_UpdatedBy",
                table: "RII_TEMP_QUOTATTION_LINE",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_TEMP_QUOTATTION_EXCHANGE_LINE");

            migrationBuilder.DropTable(
                name: "RII_TEMP_QUOTATTION_LINE");

            migrationBuilder.DropTable(
                name: "RII_TEMP_QUOTATTION");

            migrationBuilder.CreateTable(
                name: "RII_TEMP_QUOTATION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CurrencyCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, defaultValue: ""),
                    DiscountRate1 = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 0m),
                    DiscountRate2 = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 0m),
                    DiscountRate3 = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 0m),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 1m),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OfferDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_TEMP_QUOTATION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_TEMP_QUOTATION_RII_CUSTOMER_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "RII_CUSTOMER",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_TEMP_QUOTATION_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_TEMP_QUOTATION_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_TEMP_QUOTATION_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATION_CreatedBy",
                table: "RII_TEMP_QUOTATION",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATION_CreatedDate",
                table: "RII_TEMP_QUOTATION",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATION_CustomerId",
                table: "RII_TEMP_QUOTATION",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATION_DeletedBy",
                table: "RII_TEMP_QUOTATION",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATION_IsApproved",
                table: "RII_TEMP_QUOTATION",
                column: "IsApproved");

            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATION_UpdatedBy",
                table: "RII_TEMP_QUOTATION",
                column: "UpdatedBy");
        }
    }
}
