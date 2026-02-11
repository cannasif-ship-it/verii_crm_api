using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddQuotationSalesTypeAndErpProjectCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErpProjectCode",
                table: "RII_QUOTATION_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErpProjectCode",
                table: "RII_QUOTATION",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SalesTypeDefinitionId",
                table: "RII_QUOTATION",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_SalesTypeDefinitionId",
                table: "RII_QUOTATION",
                column: "SalesTypeDefinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_QUOTATION_RII_SALES_TYPE_DEFINITION_SalesTypeDefinitionId",
                table: "RII_QUOTATION",
                column: "SalesTypeDefinitionId",
                principalTable: "RII_SALES_TYPE_DEFINITION",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RII_QUOTATION_RII_SALES_TYPE_DEFINITION_SalesTypeDefinitionId",
                table: "RII_QUOTATION");

            migrationBuilder.DropIndex(
                name: "IX_Quotation_SalesTypeDefinitionId",
                table: "RII_QUOTATION");

            migrationBuilder.DropColumn(
                name: "ErpProjectCode",
                table: "RII_QUOTATION_LINE");

            migrationBuilder.DropColumn(
                name: "ErpProjectCode",
                table: "RII_QUOTATION");

            migrationBuilder.DropColumn(
                name: "SalesTypeDefinitionId",
                table: "RII_QUOTATION");
        }
    }
}
