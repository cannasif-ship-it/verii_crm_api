using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cms_webapi.Migrations
{
    /// <inheritdoc />
    public partial class AddMaxAmountToApprovalRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MaxAmount",
                table: "RII_APPROVAL_ROLE",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxAmount",
                table: "RII_APPROVAL_ROLE");
        }
    }
}
