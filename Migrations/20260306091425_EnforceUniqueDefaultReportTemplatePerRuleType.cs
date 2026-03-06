using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class EnforceUniqueDefaultReportTemplatePerRuleType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RII_REPORT_TEMPLATES_RuleType_Default",
                table: "RII_REPORT_TEMPLATES");

            migrationBuilder.CreateIndex(
                name: "IX_RII_REPORT_TEMPLATES_RuleType_Default",
                table: "RII_REPORT_TEMPLATES",
                columns: new[] { "RuleType", "Default" },
                unique: true,
                filter: "[Default] = 1 AND [IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RII_REPORT_TEMPLATES_RuleType_Default",
                table: "RII_REPORT_TEMPLATES");

            migrationBuilder.CreateIndex(
                name: "IX_RII_REPORT_TEMPLATES_RuleType_Default",
                table: "RII_REPORT_TEMPLATES",
                columns: new[] { "RuleType", "Default" });
        }
    }
}
