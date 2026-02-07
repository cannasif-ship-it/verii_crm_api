using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddDynamicPermissionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_PERMISSION_DEFINITIONS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
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
                    table.PrimaryKey("PK_RII_PERMISSION_DEFINITIONS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PERMISSION_DEFINITIONS_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PERMISSION_DEFINITIONS_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PERMISSION_DEFINITIONS_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_PERMISSION_GROUPS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsSystemAdmin = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
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
                    table.PrimaryKey("PK_RII_PERMISSION_GROUPS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PERMISSION_GROUPS_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PERMISSION_GROUPS_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PERMISSION_GROUPS_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_PERMISSION_GROUP_PERMISSIONS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionGroupId = table.Column<long>(type: "bigint", nullable: false),
                    PermissionDefinitionId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_RII_PERMISSION_GROUP_PERMISSIONS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PERMISSION_GROUP_PERMISSIONS_RII_PERMISSION_DEFINITIONS_PermissionDefinitionId",
                        column: x => x.PermissionDefinitionId,
                        principalTable: "RII_PERMISSION_DEFINITIONS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PERMISSION_GROUP_PERMISSIONS_RII_PERMISSION_GROUPS_PermissionGroupId",
                        column: x => x.PermissionGroupId,
                        principalTable: "RII_PERMISSION_GROUPS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PERMISSION_GROUP_PERMISSIONS_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PERMISSION_GROUP_PERMISSIONS_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PERMISSION_GROUP_PERMISSIONS_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_USER_PERMISSION_GROUPS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    PermissionGroupId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_RII_USER_PERMISSION_GROUPS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_USER_PERMISSION_GROUPS_RII_PERMISSION_GROUPS_PermissionGroupId",
                        column: x => x.PermissionGroupId,
                        principalTable: "RII_PERMISSION_GROUPS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_PERMISSION_GROUPS_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_PERMISSION_GROUPS_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_PERMISSION_GROUPS_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_PERMISSION_GROUPS_RII_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "RII_PERMISSION_DEFINITIONS",
                columns: new[] { "Id", "Code", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "Description", "IsActive", "IsDeleted", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1L, "dashboard.view", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, true, false, "Dashboard View", null, null },
                    { 2L, "customers.view", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, true, false, "Customers View", null, null },
                    { 3L, "salesmen360.view", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, true, false, "Salesmen 360 View", null, null },
                    { 4L, "customer360.view", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, true, false, "Customer 360 View", null, null },
                    { 5L, "powerbi.view", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, true, false, "Power BI View", null, null }
                });

            migrationBuilder.InsertData(
                table: "RII_PERMISSION_GROUPS",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "Description", "IsActive", "IsDeleted", "IsSystemAdmin", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[] { 1L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Full system access", true, false, true, "System Admin", null, null });

            migrationBuilder.InsertData(
                table: "RII_USER_PERMISSION_GROUPS",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "IsDeleted", "PermissionGroupId", "UpdatedBy", "UpdatedDate", "UserId" },
                values: new object[] { 1L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1L, null, null, 1L });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionDefinitions_Code",
                table: "RII_PERMISSION_DEFINITIONS",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionDefinitions_IsDeleted",
                table: "RII_PERMISSION_DEFINITIONS",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PERMISSION_DEFINITIONS_CreatedBy",
                table: "RII_PERMISSION_DEFINITIONS",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PERMISSION_DEFINITIONS_DeletedBy",
                table: "RII_PERMISSION_DEFINITIONS",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PERMISSION_DEFINITIONS_UpdatedBy",
                table: "RII_PERMISSION_DEFINITIONS",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroupPermission_GroupId_DefinitionId",
                table: "RII_PERMISSION_GROUP_PERMISSIONS",
                columns: new[] { "PermissionGroupId", "PermissionDefinitionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroupPermission_IsDeleted",
                table: "RII_PERMISSION_GROUP_PERMISSIONS",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PERMISSION_GROUP_PERMISSIONS_CreatedBy",
                table: "RII_PERMISSION_GROUP_PERMISSIONS",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PERMISSION_GROUP_PERMISSIONS_DeletedBy",
                table: "RII_PERMISSION_GROUP_PERMISSIONS",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PERMISSION_GROUP_PERMISSIONS_PermissionDefinitionId",
                table: "RII_PERMISSION_GROUP_PERMISSIONS",
                column: "PermissionDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PERMISSION_GROUP_PERMISSIONS_UpdatedBy",
                table: "RII_PERMISSION_GROUP_PERMISSIONS",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroups_IsDeleted",
                table: "RII_PERMISSION_GROUPS",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroups_Name",
                table: "RII_PERMISSION_GROUPS",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RII_PERMISSION_GROUPS_CreatedBy",
                table: "RII_PERMISSION_GROUPS",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PERMISSION_GROUPS_DeletedBy",
                table: "RII_PERMISSION_GROUPS",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PERMISSION_GROUPS_UpdatedBy",
                table: "RII_PERMISSION_GROUPS",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_PERMISSION_GROUPS_CreatedBy",
                table: "RII_USER_PERMISSION_GROUPS",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_PERMISSION_GROUPS_DeletedBy",
                table: "RII_USER_PERMISSION_GROUPS",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_PERMISSION_GROUPS_PermissionGroupId",
                table: "RII_USER_PERMISSION_GROUPS",
                column: "PermissionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_PERMISSION_GROUPS_UpdatedBy",
                table: "RII_USER_PERMISSION_GROUPS",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissionGroup_IsDeleted",
                table: "RII_USER_PERMISSION_GROUPS",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissionGroup_UserId_GroupId",
                table: "RII_USER_PERMISSION_GROUPS",
                columns: new[] { "UserId", "PermissionGroupId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_PERMISSION_GROUP_PERMISSIONS");

            migrationBuilder.DropTable(
                name: "RII_USER_PERMISSION_GROUPS");

            migrationBuilder.DropTable(
                name: "RII_PERMISSION_DEFINITIONS");

            migrationBuilder.DropTable(
                name: "RII_PERMISSION_GROUPS");

        }
    }
}
