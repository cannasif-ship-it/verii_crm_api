using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cms_webapi.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentSerialTypeToQuotation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DocumentSerialTypeId",
                table: "RII_QUOTATION",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_DocumentSerialTypeId",
                table: "RII_QUOTATION",
                column: "DocumentSerialTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_QUOTATION_RII_DOCUMENT_SERIAL_TYPE_DocumentSerialTypeId",
                table: "RII_QUOTATION",
                column: "DocumentSerialTypeId",
                principalTable: "RII_DOCUMENT_SERIAL_TYPE",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RII_QUOTATION_RII_DOCUMENT_SERIAL_TYPE_DocumentSerialTypeId",
                table: "RII_QUOTATION");

            migrationBuilder.DropIndex(
                name: "IX_Quotation_DocumentSerialTypeId",
                table: "RII_QUOTATION");

            migrationBuilder.DropColumn(
                name: "DocumentSerialTypeId",
                table: "RII_QUOTATION");
        }
    }
}
