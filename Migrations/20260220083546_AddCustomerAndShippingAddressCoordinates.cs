using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerAndShippingAddressCoordinates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "RII_SHIPPING_ADDRESS",
                type: "decimal(9,6)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "RII_SHIPPING_ADDRESS",
                type: "decimal(9,6)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "RII_CUSTOMER",
                type: "decimal(9,6)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "RII_CUSTOMER",
                type: "decimal(9,6)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShippingAddress_Latitude_Longitude",
                table: "RII_SHIPPING_ADDRESS",
                columns: new[] { "Latitude", "Longitude" });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Latitude_Longitude",
                table: "RII_CUSTOMER",
                columns: new[] { "Latitude", "Longitude" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShippingAddress_Latitude_Longitude",
                table: "RII_SHIPPING_ADDRESS");

            migrationBuilder.DropIndex(
                name: "IX_Customer_Latitude_Longitude",
                table: "RII_CUSTOMER");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "RII_SHIPPING_ADDRESS");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "RII_SHIPPING_ADDRESS");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "RII_CUSTOMER");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "RII_CUSTOMER");
        }
    }
}
