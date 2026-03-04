using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddGoogleIntegrationLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_GOOGLE_INTEGRATION_LOGS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    Operation = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ErrorCode = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ActivityId = table.Column<long>(type: "bigint", nullable: true),
                    GoogleCalendarEventId = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    MetadataJson = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
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
                    table.PrimaryKey("PK_RII_GOOGLE_INTEGRATION_LOGS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_GOOGLE_INTEGRATION_LOGS_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_GOOGLE_INTEGRATION_LOGS_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_GOOGLE_INTEGRATION_LOGS_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_GOOGLE_INTEGRATION_LOGS_RII_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoogleIntegrationLogs_CreatedDate",
                table: "RII_GOOGLE_INTEGRATION_LOGS",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_GoogleIntegrationLogs_TenantId",
                table: "RII_GOOGLE_INTEGRATION_LOGS",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GoogleIntegrationLogs_UserId",
                table: "RII_GOOGLE_INTEGRATION_LOGS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GOOGLE_INTEGRATION_LOGS_CreatedBy",
                table: "RII_GOOGLE_INTEGRATION_LOGS",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GOOGLE_INTEGRATION_LOGS_DeletedBy",
                table: "RII_GOOGLE_INTEGRATION_LOGS",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GOOGLE_INTEGRATION_LOGS_UpdatedBy",
                table: "RII_GOOGLE_INTEGRATION_LOGS",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_GOOGLE_INTEGRATION_LOGS");
        }
    }
}
