using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddDemandIdToQuotationAndQuotationIdToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add DemandId to Quotation table
            migrationBuilder.AddColumn<long>(
                name: "DemandId",
                table: "RII_QUOTATION",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_DemandId",
                table: "RII_QUOTATION",
                column: "DemandId");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_QUOTATION_RII_DEMAND_DemandId",
                table: "RII_QUOTATION",
                column: "DemandId",
                principalTable: "RII_DEMAND",
                principalColumn: "Id");

            // Add QuotationId to Order table
            migrationBuilder.AddColumn<long>(
                name: "QuotationId",
                table: "RII_ORDER",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_QuotationId",
                table: "RII_ORDER",
                column: "QuotationId");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_ORDER_RII_QUOTATION_QuotationId",
                table: "RII_ORDER",
                column: "QuotationId",
                principalTable: "RII_QUOTATION",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RII_ORDER_RII_QUOTATION_QuotationId",
                table: "RII_ORDER");

            migrationBuilder.DropIndex(
                name: "IX_Order_QuotationId",
                table: "RII_ORDER");

            migrationBuilder.DropColumn(
                name: "QuotationId",
                table: "RII_ORDER");

            migrationBuilder.DropForeignKey(
                name: "FK_RII_QUOTATION_RII_DEMAND_DemandId",
                table: "RII_QUOTATION");

            migrationBuilder.DropIndex(
                name: "IX_Quotation_DemandId",
                table: "RII_QUOTATION");

            migrationBuilder.DropColumn(
                name: "DemandId",
                table: "RII_QUOTATION");
        }
    }
}
