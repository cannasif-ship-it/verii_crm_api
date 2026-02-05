using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddPowerBIReportRoleMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_POWERBI_REPORT_ROLE_MAPPINGS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PowerBIReportDefinitionId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    RlsRoles = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_RII_POWERBI_REPORT_ROLE_MAPPINGS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_POWERBI_REPORT_ROLE_MAPPINGS_RII_POWERBI_REPORT_DEFINITIONS_PowerBIReportDefinitionId",
                        column: x => x.PowerBIReportDefinitionId,
                        principalTable: "RII_POWERBI_REPORT_DEFINITIONS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RII_POWERBI_REPORT_ROLE_MAPPINGS_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_POWERBI_REPORT_ROLE_MAPPINGS_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_POWERBI_REPORT_ROLE_MAPPINGS_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_POWERBI_REPORT_ROLE_MAPPINGS_RII_USER_AUTHORITY_RoleId",
                        column: x => x.RoleId,
                        principalTable: "RII_USER_AUTHORITY",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "RII_SMTP_SETTING",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 5, 15, 8, 23, 635, DateTimeKind.Utc).AddTicks(750));

            migrationBuilder.CreateIndex(
                name: "IX_PowerBIReportRoleMappings_IsDeleted",
                table: "RII_POWERBI_REPORT_ROLE_MAPPINGS",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_POWERBI_REPORT_ROLE_MAPPINGS_CreatedBy",
                table: "RII_POWERBI_REPORT_ROLE_MAPPINGS",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_POWERBI_REPORT_ROLE_MAPPINGS_DeletedBy",
                table: "RII_POWERBI_REPORT_ROLE_MAPPINGS",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_POWERBI_REPORT_ROLE_MAPPINGS_RoleId",
                table: "RII_POWERBI_REPORT_ROLE_MAPPINGS",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_POWERBI_REPORT_ROLE_MAPPINGS_UpdatedBy",
                table: "RII_POWERBI_REPORT_ROLE_MAPPINGS",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UX_PowerBIReportRoleMappings_Report_Role",
                table: "RII_POWERBI_REPORT_ROLE_MAPPINGS",
                columns: new[] { "PowerBIReportDefinitionId", "RoleId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_POWERBI_REPORT_ROLE_MAPPINGS");

            migrationBuilder.UpdateData(
                table: "RII_SMTP_SETTING",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 5, 12, 33, 49, 809, DateTimeKind.Utc).AddTicks(3440));
        }
    }
}
