using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddDemandOrderSalesTypeAndErpProjectCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErpProjectCode",
                table: "RII_ORDER_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErpProjectCode",
                table: "RII_ORDER",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SalesTypeDefinitionId",
                table: "RII_ORDER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErpProjectCode",
                table: "RII_DEMAND_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErpProjectCode",
                table: "RII_DEMAND",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SalesTypeDefinitionId",
                table: "RII_DEMAND",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_SalesTypeDefinitionId",
                table: "RII_ORDER",
                column: "SalesTypeDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Demand_SalesTypeDefinitionId",
                table: "RII_DEMAND",
                column: "SalesTypeDefinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_DEMAND_RII_SALES_TYPE_DEFINITION_SalesTypeDefinitionId",
                table: "RII_DEMAND",
                column: "SalesTypeDefinitionId",
                principalTable: "RII_SALES_TYPE_DEFINITION",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_ORDER_RII_SALES_TYPE_DEFINITION_SalesTypeDefinitionId",
                table: "RII_ORDER",
                column: "SalesTypeDefinitionId",
                principalTable: "RII_SALES_TYPE_DEFINITION",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RII_DEMAND_RII_SALES_TYPE_DEFINITION_SalesTypeDefinitionId",
                table: "RII_DEMAND");

            migrationBuilder.DropForeignKey(
                name: "FK_RII_ORDER_RII_SALES_TYPE_DEFINITION_SalesTypeDefinitionId",
                table: "RII_ORDER");

            migrationBuilder.DropIndex(
                name: "IX_Order_SalesTypeDefinitionId",
                table: "RII_ORDER");

            migrationBuilder.DropIndex(
                name: "IX_Demand_SalesTypeDefinitionId",
                table: "RII_DEMAND");

            migrationBuilder.DropColumn(
                name: "ErpProjectCode",
                table: "RII_ORDER_LINE");

            migrationBuilder.DropColumn(
                name: "ErpProjectCode",
                table: "RII_ORDER");

            migrationBuilder.DropColumn(
                name: "SalesTypeDefinitionId",
                table: "RII_ORDER");

            migrationBuilder.DropColumn(
                name: "ErpProjectCode",
                table: "RII_DEMAND_LINE");

            migrationBuilder.DropColumn(
                name: "ErpProjectCode",
                table: "RII_DEMAND");

            migrationBuilder.DropColumn(
                name: "SalesTypeDefinitionId",
                table: "RII_DEMAND");
        }
    }
}
