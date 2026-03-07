using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddTempQuotationHeaderTablePhase1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StackTrace",
                table: "RII_JOB_FAILURE_LOG",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 8000,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RII_TEMP_QUOTATION",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_TEMP_QUOTATION");

            migrationBuilder.AlterColumn<string>(
                name: "StackTrace",
                table: "RII_JOB_FAILURE_LOG",
                type: "nvarchar(max)",
                maxLength: 8000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);
        }
    }
}
