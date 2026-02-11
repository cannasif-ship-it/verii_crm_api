using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddQuotationNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_QUOTATION_NOTES",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuotationId = table.Column<long>(type: "bigint", nullable: false),
                    Note1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note4 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note5 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note6 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note7 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note8 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note9 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note10 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note11 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note12 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note13 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note14 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note15 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_QUOTATION_NOTES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_QUOTATION_NOTES_RII_QUOTATION_QuotationId",
                        column: x => x.QuotationId,
                        principalTable: "RII_QUOTATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RII_QUOTATION_NOTES_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_QUOTATION_NOTES_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_QUOTATION_NOTES_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuotationNotes_IsDeleted",
                table: "RII_QUOTATION_NOTES",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationNotes_QuotationId",
                table: "RII_QUOTATION_NOTES",
                column: "QuotationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RII_QUOTATION_NOTES_CreatedBy",
                table: "RII_QUOTATION_NOTES",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_QUOTATION_NOTES_DeletedBy",
                table: "RII_QUOTATION_NOTES",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_QUOTATION_NOTES_UpdatedBy",
                table: "RII_QUOTATION_NOTES",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_QUOTATION_NOTES");
        }
    }
}
