using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddPowerBIConfigurationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_POWERBI_CONFIGURATION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApiBaseUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Scope = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("PK_RII_POWERBI_CONFIGURATION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_POWERBI_CONFIGURATION_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_POWERBI_CONFIGURATION_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_POWERBI_CONFIGURATION_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "RII_SMTP_SETTING",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 5, 12, 33, 49, 809, DateTimeKind.Utc).AddTicks(3440));

            migrationBuilder.CreateIndex(
                name: "IX_PowerBIConfiguration_IsDeleted",
                table: "RII_POWERBI_CONFIGURATION",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_POWERBI_CONFIGURATION_CreatedBy",
                table: "RII_POWERBI_CONFIGURATION",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_POWERBI_CONFIGURATION_DeletedBy",
                table: "RII_POWERBI_CONFIGURATION",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_POWERBI_CONFIGURATION_UpdatedBy",
                table: "RII_POWERBI_CONFIGURATION",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_POWERBI_CONFIGURATION");

            migrationBuilder.UpdateData(
                table: "RII_SMTP_SETTING",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 5, 10, 33, 52, 412, DateTimeKind.Utc).AddTicks(290));
        }
    }
}
