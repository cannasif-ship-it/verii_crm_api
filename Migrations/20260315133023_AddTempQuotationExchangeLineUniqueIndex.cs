using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddTempQuotationExchangeLineUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RII_TEMP_QUOTATTION_EXCHANGE_LINE_TempQuotattionId_Currency",
                table: "RII_TEMP_QUOTATTION_EXCHANGE_LINE",
                columns: new[] { "TempQuotattionId", "Currency" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RII_TEMP_QUOTATTION_EXCHANGE_LINE_TempQuotattionId_Currency",
                table: "RII_TEMP_QUOTATTION_EXCHANGE_LINE");
        }
    }
}
