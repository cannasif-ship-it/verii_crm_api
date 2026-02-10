using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddGeneralDiscountFieldsToDemandAndOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "GeneralDiscountAmount",
                table: "RII_ORDER",
                type: "decimal(18,6)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GeneralDiscountRate",
                table: "RII_ORDER",
                type: "decimal(18,6)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GeneralDiscountAmount",
                table: "RII_DEMAND",
                type: "decimal(18,6)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GeneralDiscountRate",
                table: "RII_DEMAND",
                type: "decimal(18,6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneralDiscountAmount",
                table: "RII_ORDER");

            migrationBuilder.DropColumn(
                name: "GeneralDiscountRate",
                table: "RII_ORDER");

            migrationBuilder.DropColumn(
                name: "GeneralDiscountAmount",
                table: "RII_DEMAND");

            migrationBuilder.DropColumn(
                name: "GeneralDiscountRate",
                table: "RII_DEMAND");
        }
    }
}
