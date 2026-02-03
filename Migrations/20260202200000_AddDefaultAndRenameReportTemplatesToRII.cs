using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultAndRenameReportTemplatesToRII : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Tablo adını ReportTemplates -> RII_REPORT_TEMPLATES yap
            migrationBuilder.RenameTable(
                name: "ReportTemplates",
                newName: "RII_REPORT_TEMPLATES");

            // 2. Eski index isimlerini yeni tablo adına uyumlu yap (drop + create)
            migrationBuilder.DropIndex(
                name: "IX_ReportTemplates_CreatedByUserId",
                table: "RII_REPORT_TEMPLATES");
            migrationBuilder.DropIndex(
                name: "IX_ReportTemplates_DeletedByUserId",
                table: "RII_REPORT_TEMPLATES");
            migrationBuilder.DropIndex(
                name: "IX_ReportTemplates_IsActive",
                table: "RII_REPORT_TEMPLATES");
            migrationBuilder.DropIndex(
                name: "IX_ReportTemplates_RuleType",
                table: "RII_REPORT_TEMPLATES");
            migrationBuilder.DropIndex(
                name: "IX_ReportTemplates_RuleType_IsActive",
                table: "RII_REPORT_TEMPLATES");
            migrationBuilder.DropIndex(
                name: "IX_ReportTemplates_UpdatedByUserId",
                table: "RII_REPORT_TEMPLATES");

            migrationBuilder.CreateIndex(
                name: "IX_RII_REPORT_TEMPLATES_CreatedByUserId",
                table: "RII_REPORT_TEMPLATES",
                column: "CreatedByUserId");
            migrationBuilder.CreateIndex(
                name: "IX_RII_REPORT_TEMPLATES_DeletedByUserId",
                table: "RII_REPORT_TEMPLATES",
                column: "DeletedByUserId");
            migrationBuilder.CreateIndex(
                name: "IX_RII_REPORT_TEMPLATES_IsActive",
                table: "RII_REPORT_TEMPLATES",
                column: "IsActive");
            migrationBuilder.CreateIndex(
                name: "IX_RII_REPORT_TEMPLATES_RuleType",
                table: "RII_REPORT_TEMPLATES",
                column: "RuleType");
            migrationBuilder.CreateIndex(
                name: "IX_RII_REPORT_TEMPLATES_RuleType_IsActive",
                table: "RII_REPORT_TEMPLATES",
                columns: new[] { "RuleType", "IsActive" });
            migrationBuilder.CreateIndex(
                name: "IX_RII_REPORT_TEMPLATES_UpdatedByUserId",
                table: "RII_REPORT_TEMPLATES",
                column: "UpdatedByUserId");

            // 3. Default kolonunu ekle (bit, not null, default false)
            migrationBuilder.AddColumn<bool>(
                name: "Default",
                table: "RII_REPORT_TEMPLATES",
                type: "bit",
                nullable: false,
                defaultValue: false);

            // 4. Default index
            migrationBuilder.CreateIndex(
                name: "IX_RII_REPORT_TEMPLATES_RuleType_Default",
                table: "RII_REPORT_TEMPLATES",
                columns: new[] { "RuleType", "Default" });

            // 5. Her RuleType için tam 1 tane Default=true: o tipte Id'si en küçük olan kaydı default yap
            migrationBuilder.Sql(@"
                UPDATE RII_REPORT_TEMPLATES SET [Default] = 1
                WHERE Id IN (
                    SELECT MIN(Id) FROM RII_REPORT_TEMPLATES WHERE IsDeleted = 0 GROUP BY RuleType
                );
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RII_REPORT_TEMPLATES_RuleType_Default",
                table: "RII_REPORT_TEMPLATES");

            migrationBuilder.DropColumn(
                name: "Default",
                table: "RII_REPORT_TEMPLATES");

            migrationBuilder.DropIndex(
                name: "IX_RII_REPORT_TEMPLATES_CreatedByUserId",
                table: "RII_REPORT_TEMPLATES");
            migrationBuilder.DropIndex(
                name: "IX_RII_REPORT_TEMPLATES_DeletedByUserId",
                table: "RII_REPORT_TEMPLATES");
            migrationBuilder.DropIndex(
                name: "IX_RII_REPORT_TEMPLATES_IsActive",
                table: "RII_REPORT_TEMPLATES");
            migrationBuilder.DropIndex(
                name: "IX_RII_REPORT_TEMPLATES_RuleType",
                table: "RII_REPORT_TEMPLATES");
            migrationBuilder.DropIndex(
                name: "IX_RII_REPORT_TEMPLATES_RuleType_IsActive",
                table: "RII_REPORT_TEMPLATES");
            migrationBuilder.DropIndex(
                name: "IX_RII_REPORT_TEMPLATES_UpdatedByUserId",
                table: "RII_REPORT_TEMPLATES");

            migrationBuilder.RenameTable(
                name: "RII_REPORT_TEMPLATES",
                newName: "ReportTemplates");

            migrationBuilder.CreateIndex(
                name: "IX_ReportTemplates_CreatedByUserId",
                table: "ReportTemplates",
                column: "CreatedByUserId");
            migrationBuilder.CreateIndex(
                name: "IX_ReportTemplates_DeletedByUserId",
                table: "ReportTemplates",
                column: "DeletedByUserId");
            migrationBuilder.CreateIndex(
                name: "IX_ReportTemplates_IsActive",
                table: "ReportTemplates",
                column: "IsActive");
            migrationBuilder.CreateIndex(
                name: "IX_ReportTemplates_RuleType",
                table: "ReportTemplates",
                column: "RuleType");
            migrationBuilder.CreateIndex(
                name: "IX_ReportTemplates_RuleType_IsActive",
                table: "ReportTemplates",
                columns: new[] { "RuleType", "IsActive" });
            migrationBuilder.CreateIndex(
                name: "IX_ReportTemplates_UpdatedByUserId",
                table: "ReportTemplates",
                column: "UpdatedByUserId");
        }
    }
}
