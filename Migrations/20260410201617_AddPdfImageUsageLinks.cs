using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddPdfImageUsageLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_PDF_IMAGE_USAGES",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PdfTemplateAssetId = table.Column<long>(type: "bigint", nullable: false),
                    ReportTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    UsageType = table.Column<int>(type: "int", nullable: false),
                    ElementId = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    PageNumber = table.Column<int>(type: "int", nullable: false),
                    RuleType = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_RII_PDF_IMAGE_USAGES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PDF_IMAGE_USAGES_RII_PDF_IMAGES_PdfTemplateAssetId",
                        column: x => x.PdfTemplateAssetId,
                        principalTable: "RII_PDF_IMAGES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_PDF_IMAGE_USAGES_RII_REPORT_TEMPLATES_ReportTemplateId",
                        column: x => x.ReportTemplateId,
                        principalTable: "RII_REPORT_TEMPLATES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_PDF_IMAGE_USAGES_RII_USERS_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PDF_IMAGE_USAGES_RII_USERS_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PDF_IMAGE_USAGES_RII_USERS_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_QUICK_QUOTATION_IMAGE_USAGES",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuickQuotationImageId = table.Column<long>(type: "bigint", nullable: false),
                    TempQuotattionId = table.Column<long>(type: "bigint", nullable: false),
                    TempQuotattionLineId = table.Column<long>(type: "bigint", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
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
                    table.PrimaryKey("PK_RII_QUICK_QUOTATION_IMAGE_USAGES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_QUICK_QUOTATION_IMAGE_USAGES_RII_QUICK_QUOTATION_IMAGES_QuickQuotationImageId",
                        column: x => x.QuickQuotationImageId,
                        principalTable: "RII_QUICK_QUOTATION_IMAGES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_QUICK_QUOTATION_IMAGE_USAGES_RII_TEMP_QUOTATTION_LINE_TempQuotattionLineId",
                        column: x => x.TempQuotattionLineId,
                        principalTable: "RII_TEMP_QUOTATTION_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_QUICK_QUOTATION_IMAGE_USAGES_RII_TEMP_QUOTATTION_TempQuotattionId",
                        column: x => x.TempQuotattionId,
                        principalTable: "RII_TEMP_QUOTATTION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_QUICK_QUOTATION_IMAGE_USAGES_RII_USERS_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_QUICK_QUOTATION_IMAGE_USAGES_RII_USERS_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_QUICK_QUOTATION_IMAGE_USAGES_RII_USERS_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RII_PDF_IMAGE_USAGES_CreatedByUserId",
                table: "RII_PDF_IMAGE_USAGES",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PDF_IMAGE_USAGES_DeletedByUserId",
                table: "RII_PDF_IMAGE_USAGES",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PDF_IMAGE_USAGES_ImageId",
                table: "RII_PDF_IMAGE_USAGES",
                column: "PdfTemplateAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PDF_IMAGE_USAGES_Template_Element_Page",
                table: "RII_PDF_IMAGE_USAGES",
                columns: new[] { "ReportTemplateId", "ElementId", "PageNumber" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PDF_IMAGE_USAGES_UpdatedByUserId",
                table: "RII_PDF_IMAGE_USAGES",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_QUICK_QUOTATION_IMAGE_USAGES_CreatedByUserId",
                table: "RII_QUICK_QUOTATION_IMAGE_USAGES",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_QUICK_QUOTATION_IMAGE_USAGES_DeletedByUserId",
                table: "RII_QUICK_QUOTATION_IMAGE_USAGES",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_QUICK_QUOTATION_IMAGE_USAGES_ImageId",
                table: "RII_QUICK_QUOTATION_IMAGE_USAGES",
                column: "QuickQuotationImageId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_QUICK_QUOTATION_IMAGE_USAGES_LineId",
                table: "RII_QUICK_QUOTATION_IMAGE_USAGES",
                column: "TempQuotattionLineId",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_RII_QUICK_QUOTATION_IMAGE_USAGES_TempQuotattionId",
                table: "RII_QUICK_QUOTATION_IMAGE_USAGES",
                column: "TempQuotattionId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_QUICK_QUOTATION_IMAGE_USAGES_UpdatedByUserId",
                table: "RII_QUICK_QUOTATION_IMAGE_USAGES",
                column: "UpdatedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_PDF_IMAGE_USAGES");

            migrationBuilder.DropTable(
                name: "RII_QUICK_QUOTATION_IMAGE_USAGES");
        }
    }
}
