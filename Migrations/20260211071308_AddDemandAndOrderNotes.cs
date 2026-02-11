using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddDemandAndOrderNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_DEMAND_NOTES",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemandId = table.Column<long>(type: "bigint", nullable: false),
                    Note1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note4 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note5 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note6 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note7 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note8 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note9 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note10 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note11 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note12 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note13 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note14 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note15 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_RII_DEMAND_NOTES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_NOTES_RII_DEMAND_DemandId",
                        column: x => x.DemandId,
                        principalTable: "RII_DEMAND",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_NOTES_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_NOTES_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_DEMAND_NOTES_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_ORDER_NOTES",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    Note1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note4 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note5 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note6 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note7 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note8 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note9 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note10 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note11 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note12 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note13 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note14 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note15 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_RII_ORDER_NOTES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_ORDER_NOTES_RII_ORDER_OrderId",
                        column: x => x.OrderId,
                        principalTable: "RII_ORDER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RII_ORDER_NOTES_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_ORDER_NOTES_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_ORDER_NOTES_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemandNotes_DemandId",
                table: "RII_DEMAND_NOTES",
                column: "DemandId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DemandNotes_IsDeleted",
                table: "RII_DEMAND_NOTES",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_DEMAND_NOTES_CreatedBy",
                table: "RII_DEMAND_NOTES",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_DEMAND_NOTES_DeletedBy",
                table: "RII_DEMAND_NOTES",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_DEMAND_NOTES_UpdatedBy",
                table: "RII_DEMAND_NOTES",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_OrderNotes_IsDeleted",
                table: "RII_ORDER_NOTES",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_OrderNotes_OrderId",
                table: "RII_ORDER_NOTES",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RII_ORDER_NOTES_CreatedBy",
                table: "RII_ORDER_NOTES",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_ORDER_NOTES_DeletedBy",
                table: "RII_ORDER_NOTES",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_ORDER_NOTES_UpdatedBy",
                table: "RII_ORDER_NOTES",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_DEMAND_NOTES");

            migrationBuilder.DropTable(
                name: "RII_ORDER_NOTES");
        }
    }
}
