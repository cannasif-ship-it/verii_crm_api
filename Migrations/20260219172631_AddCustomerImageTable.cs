using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_CUSTOMER_IMAGE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ImageDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_RII_CUSTOMER_IMAGE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_CUSTOMER_IMAGE_RII_CUSTOMER_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "RII_CUSTOMER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RII_CUSTOMER_IMAGE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_CUSTOMER_IMAGE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_CUSTOMER_IMAGE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerImage_CustomerId",
                table: "RII_CUSTOMER_IMAGE",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerImage_IsDeleted",
                table: "RII_CUSTOMER_IMAGE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CUSTOMER_IMAGE_CreatedBy",
                table: "RII_CUSTOMER_IMAGE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CUSTOMER_IMAGE_DeletedBy",
                table: "RII_CUSTOMER_IMAGE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CUSTOMER_IMAGE_UpdatedBy",
                table: "RII_CUSTOMER_IMAGE",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_CUSTOMER_IMAGE");
        }
    }
}
