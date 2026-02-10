using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddGeneralDiscountFieldsToQuotation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "GeneralDiscountAmount",
                table: "RII_QUOTATION",
                type: "decimal(18,6)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GeneralDiscountRate",
                table: "RII_QUOTATION",
                type: "decimal(18,6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneralDiscountAmount",
                table: "RII_QUOTATION");

            migrationBuilder.DropColumn(
                name: "GeneralDiscountRate",
                table: "RII_QUOTATION");
        }
    }
}
