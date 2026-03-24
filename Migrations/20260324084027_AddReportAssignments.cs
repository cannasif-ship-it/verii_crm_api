using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddReportAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_REPORT_ASSIGNMENTS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportDefinitionId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_REPORT_ASSIGNMENTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_REPORT_ASSIGNMENTS_RII_REPORT_DEFINITIONS_ReportDefinitionId",
                        column: x => x.ReportDefinitionId,
                        principalTable: "RII_REPORT_DEFINITIONS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RII_REPORT_ASSIGNMENTS_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_REPORT_ASSIGNMENTS_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_REPORT_ASSIGNMENTS_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_REPORT_ASSIGNMENTS_RII_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RII_REPORT_ASSIGNMENTS_CreatedBy",
                table: "RII_REPORT_ASSIGNMENTS",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_REPORT_ASSIGNMENTS_DeletedBy",
                table: "RII_REPORT_ASSIGNMENTS",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_REPORT_ASSIGNMENTS_ReportDefinitionId_UserId",
                table: "RII_REPORT_ASSIGNMENTS",
                columns: new[] { "ReportDefinitionId", "UserId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_RII_REPORT_ASSIGNMENTS_UpdatedBy",
                table: "RII_REPORT_ASSIGNMENTS",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_REPORT_ASSIGNMENTS_UserId",
                table: "RII_REPORT_ASSIGNMENTS",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_REPORT_ASSIGNMENTS");
        }
    }
}
