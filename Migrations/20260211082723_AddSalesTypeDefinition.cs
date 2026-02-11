using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddSalesTypeDefinition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_SALES_TYPE_DEFINITION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
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
                    table.PrimaryKey("PK_RII_SALES_TYPE_DEFINITION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SALES_TYPE_DEFINITION_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SALES_TYPE_DEFINITION_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SALES_TYPE_DEFINITION_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RII_SALES_TYPE_DEFINITION_CreatedBy",
                table: "RII_SALES_TYPE_DEFINITION",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SALES_TYPE_DEFINITION_DeletedBy",
                table: "RII_SALES_TYPE_DEFINITION",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SALES_TYPE_DEFINITION_UpdatedBy",
                table: "RII_SALES_TYPE_DEFINITION",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SalesTypeDefinition_IsDeleted",
                table: "RII_SALES_TYPE_DEFINITION",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SalesTypeDefinition_Name",
                table: "RII_SALES_TYPE_DEFINITION",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_SalesTypeDefinition_SalesType",
                table: "RII_SALES_TYPE_DEFINITION",
                column: "SalesType");

            migrationBuilder.CreateIndex(
                name: "IX_SalesTypeDefinition_SalesType_Name",
                table: "RII_SALES_TYPE_DEFINITION",
                columns: new[] { "SalesType", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_SALES_TYPE_DEFINITION");
        }
    }
}
