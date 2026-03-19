using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class PdfImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_PDF_TEMPLATE_ASSETS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginalFileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    StoredFileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    RelativeUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedByUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PDF_TEMPLATE_ASSETS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PDF_TEMPLATE_ASSETS_RII_USERS_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PDF_TEMPLATE_ASSETS_RII_USERS_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PDF_TEMPLATE_ASSETS_RII_USERS_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RII_PDF_TEMPLATE_ASSETS_CreatedBy",
                table: "RII_PDF_TEMPLATE_ASSETS",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PDF_TEMPLATE_ASSETS_CreatedByUserId",
                table: "RII_PDF_TEMPLATE_ASSETS",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PDF_TEMPLATE_ASSETS_DeletedByUserId",
                table: "RII_PDF_TEMPLATE_ASSETS",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PDF_TEMPLATE_ASSETS_RelativeUrl",
                table: "RII_PDF_TEMPLATE_ASSETS",
                column: "RelativeUrl",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PDF_TEMPLATE_ASSETS_UpdatedByUserId",
                table: "RII_PDF_TEMPLATE_ASSETS",
                column: "UpdatedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_PDF_TEMPLATE_ASSETS");
        }
    }
}
