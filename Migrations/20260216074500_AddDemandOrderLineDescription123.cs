using crm_api.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(CmsDbContext))]
    [Migration("20260216074500_AddDemandOrderLineDescription123")]
    public partial class AddDemandOrderLineDescription123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description1",
                table: "RII_DEMAND_LINE",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description2",
                table: "RII_DEMAND_LINE",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description3",
                table: "RII_DEMAND_LINE",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description1",
                table: "RII_ORDER_LINE",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description2",
                table: "RII_ORDER_LINE",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description3",
                table: "RII_ORDER_LINE",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description1",
                table: "RII_DEMAND_LINE");

            migrationBuilder.DropColumn(
                name: "Description2",
                table: "RII_DEMAND_LINE");

            migrationBuilder.DropColumn(
                name: "Description3",
                table: "RII_DEMAND_LINE");

            migrationBuilder.DropColumn(
                name: "Description1",
                table: "RII_ORDER_LINE");

            migrationBuilder.DropColumn(
                name: "Description2",
                table: "RII_ORDER_LINE");

            migrationBuilder.DropColumn(
                name: "Description3",
                table: "RII_ORDER_LINE");
        }
    }
}
