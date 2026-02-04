using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddPowerBiTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_POWERBI_GROUPS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
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
                    table.PrimaryKey("PK_RII_POWERBI_GROUPS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_POWERBI_GROUPS_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_POWERBI_GROUPS_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_POWERBI_GROUPS_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_POWERBI_REPORT_DEFINITIONS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WorkspaceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReportId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmbedUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ContentType = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DefaultSettingsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_RII_POWERBI_REPORT_DEFINITIONS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_POWERBI_REPORT_DEFINITIONS_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_POWERBI_REPORT_DEFINITIONS_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_POWERBI_REPORT_DEFINITIONS_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_USER_POWERBI_GROUPS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    GroupId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_RII_USER_POWERBI_GROUPS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_USER_POWERBI_GROUPS_RII_POWERBI_GROUPS_GroupId",
                        column: x => x.GroupId,
                        principalTable: "RII_POWERBI_GROUPS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_USER_POWERBI_GROUPS_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_POWERBI_GROUPS_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_POWERBI_GROUPS_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_POWERBI_GROUPS_RII_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_POWERBI_GROUP_REPORT_DEFINITIONS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<long>(type: "bigint", nullable: false),
                    ReportDefinitionId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_RII_POWERBI_GROUP_REPORT_DEFINITIONS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_POWERBI_GROUP_REPORT_DEFINITIONS_RII_POWERBI_GROUPS_GroupId",
                        column: x => x.GroupId,
                        principalTable: "RII_POWERBI_GROUPS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_POWERBI_GROUP_REPORT_DEFINITIONS_RII_POWERBI_REPORT_DEFINITIONS_ReportDefinitionId",
                        column: x => x.ReportDefinitionId,
                        principalTable: "RII_POWERBI_REPORT_DEFINITIONS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_POWERBI_GROUP_REPORT_DEFINITIONS_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_POWERBI_GROUP_REPORT_DEFINITIONS_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_POWERBI_GROUP_REPORT_DEFINITIONS_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "RII_SMTP_SETTING",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "FromName" },
                values: new object[] { new DateTime(2026, 2, 4, 13, 50, 14, 243, DateTimeKind.Utc).AddTicks(4392), "V3RII CRM SYSTEM" });

            migrationBuilder.CreateIndex(
                name: "IX_PowerBIGroupReportDefinitions_IsDeleted",
                table: "RII_POWERBI_GROUP_REPORT_DEFINITIONS",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_POWERBI_GROUP_REPORT_DEFINITIONS_CreatedBy",
                table: "RII_POWERBI_GROUP_REPORT_DEFINITIONS",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_POWERBI_GROUP_REPORT_DEFINITIONS_DeletedBy",
                table: "RII_POWERBI_GROUP_REPORT_DEFINITIONS",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_POWERBI_GROUP_REPORT_DEFINITIONS_ReportDefinitionId",
                table: "RII_POWERBI_GROUP_REPORT_DEFINITIONS",
                column: "ReportDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_POWERBI_GROUP_REPORT_DEFINITIONS_UpdatedBy",
                table: "RII_POWERBI_GROUP_REPORT_DEFINITIONS",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UX_PowerBIGroupReportDefinitions_Group_Report",
                table: "RII_POWERBI_GROUP_REPORT_DEFINITIONS",
                columns: new[] { "GroupId", "ReportDefinitionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PowerBIGroups_IsDeleted",
                table: "RII_POWERBI_GROUPS",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_POWERBI_GROUPS_CreatedBy",
                table: "RII_POWERBI_GROUPS",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_POWERBI_GROUPS_DeletedBy",
                table: "RII_POWERBI_GROUPS",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_POWERBI_GROUPS_UpdatedBy",
                table: "RII_POWERBI_GROUPS",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UX_PowerBIGroups_Name",
                table: "RII_POWERBI_GROUPS",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PowerBIReportDefinitions_IsDeleted",
                table: "RII_POWERBI_REPORT_DEFINITIONS",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PowerBIReportDefinitions_Name",
                table: "RII_POWERBI_REPORT_DEFINITIONS",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_RII_POWERBI_REPORT_DEFINITIONS_CreatedBy",
                table: "RII_POWERBI_REPORT_DEFINITIONS",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_POWERBI_REPORT_DEFINITIONS_DeletedBy",
                table: "RII_POWERBI_REPORT_DEFINITIONS",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_POWERBI_REPORT_DEFINITIONS_UpdatedBy",
                table: "RII_POWERBI_REPORT_DEFINITIONS",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UX_PowerBIReportDefinitions_Workspace_Report",
                table: "RII_POWERBI_REPORT_DEFINITIONS",
                columns: new[] { "WorkspaceId", "ReportId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_POWERBI_GROUPS_CreatedBy",
                table: "RII_USER_POWERBI_GROUPS",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_POWERBI_GROUPS_DeletedBy",
                table: "RII_USER_POWERBI_GROUPS",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_POWERBI_GROUPS_GroupId",
                table: "RII_USER_POWERBI_GROUPS",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_POWERBI_GROUPS_UpdatedBy",
                table: "RII_USER_POWERBI_GROUPS",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserPowerBIGroups_IsDeleted",
                table: "RII_USER_POWERBI_GROUPS",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "UX_UserPowerBIGroups_User_Group",
                table: "RII_USER_POWERBI_GROUPS",
                columns: new[] { "UserId", "GroupId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_POWERBI_GROUP_REPORT_DEFINITIONS");

            migrationBuilder.DropTable(
                name: "RII_USER_POWERBI_GROUPS");

            migrationBuilder.DropTable(
                name: "RII_POWERBI_REPORT_DEFINITIONS");

            migrationBuilder.DropTable(
                name: "RII_POWERBI_GROUPS");

            migrationBuilder.UpdateData(
                table: "RII_SMTP_SETTING",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedDate", "FromName" },
                values: new object[] { new DateTime(2026, 2, 4, 11, 53, 50, 511, DateTimeKind.Utc).AddTicks(8839), "V3RII CRM System" });
        }
    }
}
