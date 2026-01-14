using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cms_webapi.Migrations
{
    /// <inheritdoc />
    public partial class AddPricingRuleAndStockToQuotationLine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RII_QUOTATION_LINE_RII_PRICING_RULE_HEADER_PricingRuleHeaderId",
                table: "RII_QUOTATION_LINE");

            migrationBuilder.RenameIndex(
                name: "IX_RII_QUOTATION_LINE_PricingRuleHeaderId",
                table: "RII_QUOTATION_LINE",
                newName: "IX_QuotationLine_PricingRuleHeaderId");

            migrationBuilder.AddColumn<long>(
                name: "RelatedStockId",
                table: "RII_QUOTATION_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuotationLine_RelatedStockId",
                table: "RII_QUOTATION_LINE",
                column: "RelatedStockId");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_QUOTATION_LINE_RII_PRICING_RULE_HEADER_PricingRuleHeaderId",
                table: "RII_QUOTATION_LINE",
                column: "PricingRuleHeaderId",
                principalTable: "RII_PRICING_RULE_HEADER",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_QUOTATION_LINE_RII_STOCK_RelatedStockId",
                table: "RII_QUOTATION_LINE",
                column: "RelatedStockId",
                principalTable: "RII_STOCK",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RII_QUOTATION_LINE_RII_PRICING_RULE_HEADER_PricingRuleHeaderId",
                table: "RII_QUOTATION_LINE");

            migrationBuilder.DropForeignKey(
                name: "FK_RII_QUOTATION_LINE_RII_STOCK_RelatedStockId",
                table: "RII_QUOTATION_LINE");

            migrationBuilder.DropIndex(
                name: "IX_QuotationLine_RelatedStockId",
                table: "RII_QUOTATION_LINE");

            migrationBuilder.DropColumn(
                name: "RelatedStockId",
                table: "RII_QUOTATION_LINE");

            migrationBuilder.RenameIndex(
                name: "IX_QuotationLine_PricingRuleHeaderId",
                table: "RII_QUOTATION_LINE",
                newName: "IX_RII_QUOTATION_LINE_PricingRuleHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_QUOTATION_LINE_RII_PRICING_RULE_HEADER_PricingRuleHeaderId",
                table: "RII_QUOTATION_LINE",
                column: "PricingRuleHeaderId",
                principalTable: "RII_PRICING_RULE_HEADER",
                principalColumn: "Id");
        }
    }
}
