using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cms_webapi.Migrations
{
    /// <inheritdoc />
    public partial class AddStockFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchCode",
                table: "RII_STOCK",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "GrupAdi",
                table: "RII_STOCK",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GrupKodu",
                table: "RII_STOCK",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Kod1",
                table: "RII_STOCK",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Kod1Adi",
                table: "RII_STOCK",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Kod2",
                table: "RII_STOCK",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Kod2Adi",
                table: "RII_STOCK",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Kod3",
                table: "RII_STOCK",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Kod3Adi",
                table: "RII_STOCK",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Kod4",
                table: "RII_STOCK",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Kod4Adi",
                table: "RII_STOCK",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Kod5",
                table: "RII_STOCK",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Kod5Adi",
                table: "RII_STOCK",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UreticiKodu",
                table: "RII_STOCK",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchCode",
                table: "RII_STOCK");

            migrationBuilder.DropColumn(
                name: "GrupAdi",
                table: "RII_STOCK");

            migrationBuilder.DropColumn(
                name: "GrupKodu",
                table: "RII_STOCK");

            migrationBuilder.DropColumn(
                name: "Kod1",
                table: "RII_STOCK");

            migrationBuilder.DropColumn(
                name: "Kod1Adi",
                table: "RII_STOCK");

            migrationBuilder.DropColumn(
                name: "Kod2",
                table: "RII_STOCK");

            migrationBuilder.DropColumn(
                name: "Kod2Adi",
                table: "RII_STOCK");

            migrationBuilder.DropColumn(
                name: "Kod3",
                table: "RII_STOCK");

            migrationBuilder.DropColumn(
                name: "Kod3Adi",
                table: "RII_STOCK");

            migrationBuilder.DropColumn(
                name: "Kod4",
                table: "RII_STOCK");

            migrationBuilder.DropColumn(
                name: "Kod4Adi",
                table: "RII_STOCK");

            migrationBuilder.DropColumn(
                name: "Kod5",
                table: "RII_STOCK");

            migrationBuilder.DropColumn(
                name: "Kod5Adi",
                table: "RII_STOCK");

            migrationBuilder.DropColumn(
                name: "UreticiKodu",
                table: "RII_STOCK");
        }
    }
}
