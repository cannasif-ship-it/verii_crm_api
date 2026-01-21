using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cms_webapi.Migrations
{
    /// <inheritdoc />
    public partial class AddSerialFieldsToDocumentSerialType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SerialCurrent",
                table: "RII_DOCUMENT_SERIAL_TYPE",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SerialIncrement",
                table: "RII_DOCUMENT_SERIAL_TYPE",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SerialStart",
                table: "RII_DOCUMENT_SERIAL_TYPE",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerialCurrent",
                table: "RII_DOCUMENT_SERIAL_TYPE");

            migrationBuilder.DropColumn(
                name: "SerialIncrement",
                table: "RII_DOCUMENT_SERIAL_TYPE");

            migrationBuilder.DropColumn(
                name: "SerialStart",
                table: "RII_DOCUMENT_SERIAL_TYPE");
        }
    }
}
