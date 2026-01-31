using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddDemandIdToQuotation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
