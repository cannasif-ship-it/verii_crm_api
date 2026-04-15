using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddSalesRepTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_SalepRep_Codes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchCode = table.Column<short>(type: "smallint", nullable: false),
                    SalesRepCode = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    SalesRepDescription = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
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
                    table.PrimaryKey("PK_RII_SalepRep_Codes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SalepRep_Codes_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SalepRep_Codes_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SalepRep_Codes_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_SalepRep_Code_User_Matches",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesRepCodeId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_RII_SalepRep_Code_User_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SalepRep_Code_User_Matches_RII_SalepRep_Codes_SalesRepCodeId",
                        column: x => x.SalesRepCodeId,
                        principalTable: "RII_SalepRep_Codes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RII_SalepRep_Code_User_Matches_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SalepRep_Code_User_Matches_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SalepRep_Code_User_Matches_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SalepRep_Code_User_Matches_RII_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RII_SalepRep_Code_User_Matches_CreatedBy",
                table: "RII_SalepRep_Code_User_Matches",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SalepRep_Code_User_Matches_DeletedBy",
                table: "RII_SalepRep_Code_User_Matches",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SalepRep_Code_User_Matches_UpdatedBy",
                table: "RII_SalepRep_Code_User_Matches",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SalesRepCodeUserMatch_IsDeleted",
                table: "RII_SalepRep_Code_User_Matches",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SalesRepCodeUserMatch_SalesRepCodeId_UserId",
                table: "RII_SalepRep_Code_User_Matches",
                columns: new[] { "SalesRepCodeId", "UserId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_SalesRepCodeUserMatch_UserId",
                table: "RII_SalepRep_Code_User_Matches",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SalepRep_Codes_CreatedBy",
                table: "RII_SalepRep_Codes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SalepRep_Codes_DeletedBy",
                table: "RII_SalepRep_Codes",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SalepRep_Codes_UpdatedBy",
                table: "RII_SalepRep_Codes",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SalesRepCode_BranchCode_SalesRepCode",
                table: "RII_SalepRep_Codes",
                columns: new[] { "BranchCode", "SalesRepCode" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_SalesRepCode_IsDeleted",
                table: "RII_SalepRep_Codes",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_SalepRep_Code_User_Matches");

            migrationBuilder.DropTable(
                name: "RII_SalepRep_Codes");
        }
    }
}
