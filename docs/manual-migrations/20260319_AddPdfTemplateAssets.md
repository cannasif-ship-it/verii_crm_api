# 20260319 AddPdfTemplateAssets

Bu migration'i ben uygulamadim. Asagidaki migration sinifini `Migrations` altina kendi timestamp'in ile ekleyip sonra update edebilirsin.

## Migration Sinifi

```csharp
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    public partial class AddPdfTemplateAssets : Migration
    {
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
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PDF_TEMPLATE_ASSETS", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RII_PDF_TEMPLATE_ASSETS_CreatedBy",
                table: "RII_PDF_TEMPLATE_ASSETS",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PDF_TEMPLATE_ASSETS_RelativeUrl",
                table: "RII_PDF_TEMPLATE_ASSETS",
                column: "RelativeUrl",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_PDF_TEMPLATE_ASSETS");
        }
    }
}
```

## Update Komutu

```bash
dotnet ef database update --context CmsDbContext --project crm_api.csproj
```
