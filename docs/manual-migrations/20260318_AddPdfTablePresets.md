# AddPdfTablePresets

Bu migration'i ben uygulamadim. Sen `verii_crm_api/Migrations` altina yeni bir migration dosyasi olarak ekleyip kendi akışında çalıştırabilirsin.

Onerilen dosya adi:

`20260318150000_AddPdfTablePresets.cs`

```csharp
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    public partial class AddPdfTablePresets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_PDF_TABLE_PRESETS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RuleType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Key = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    ColumnsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TableOptionsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
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
                    table.PrimaryKey("PK_RII_PDF_TABLE_PRESETS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PDF_TABLE_PRESETS_RII_USERS_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PDF_TABLE_PRESETS_RII_USERS_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PDF_TABLE_PRESETS_RII_USERS_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RII_PDF_TABLE_PRESETS_CreatedByUserId",
                table: "RII_PDF_TABLE_PRESETS",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PDF_TABLE_PRESETS_DeletedByUserId",
                table: "RII_PDF_TABLE_PRESETS",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PDF_TABLE_PRESETS_Key",
                table: "RII_PDF_TABLE_PRESETS",
                column: "Key",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PDF_TABLE_PRESETS_RuleType_IsActive",
                table: "RII_PDF_TABLE_PRESETS",
                columns: new[] { "RuleType", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_RII_PDF_TABLE_PRESETS_UpdatedByUserId",
                table: "RII_PDF_TABLE_PRESETS",
                column: "UpdatedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_PDF_TABLE_PRESETS");
        }
    }
}
```

Notlar:

- Bunu ben `dotnet ef migrations add` ile generate etmedim; sen mevcut migration naming standard'ına gore timestamp'i degistirebilirsin.
- Ardindan kendi ortamina gore designer/snapshot dosyalarini olusturup migration'i uygulayabilirsin.
- Bu migration, server-side global table preset library icin gereken tabloyu ve indexleri ekler.
