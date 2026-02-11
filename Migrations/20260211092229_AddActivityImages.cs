using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddActivityImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_ACTIVITY_IMAGE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    ResimAciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResimUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
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
                    table.PrimaryKey("PK_RII_ACTIVITY_IMAGE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_ACTIVITY_IMAGE_RII_ACTIVITY_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "RII_ACTIVITY",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RII_ACTIVITY_IMAGE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_ACTIVITY_IMAGE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_ACTIVITY_IMAGE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityImage_ActivityId",
                table: "RII_ACTIVITY_IMAGE",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityImage_IsDeleted",
                table: "RII_ACTIVITY_IMAGE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_ACTIVITY_IMAGE_CreatedBy",
                table: "RII_ACTIVITY_IMAGE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_ACTIVITY_IMAGE_DeletedBy",
                table: "RII_ACTIVITY_IMAGE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_ACTIVITY_IMAGE_UpdatedBy",
                table: "RII_ACTIVITY_IMAGE",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_ACTIVITY_IMAGE");
        }
    }
}
