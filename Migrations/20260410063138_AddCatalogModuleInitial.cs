using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddCatalogModuleInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "RII_USER_DETAIL",
                type: "decimal(18,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Height",
                table: "RII_USER_DETAIL",
                type: "decimal(18,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "RII_SHIPPING_ADDRESS",
                type: "decimal(18,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "RII_SHIPPING_ADDRESS",
                type: "decimal(18,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "RII_CUSTOMER",
                type: "decimal(18,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "RII_CUSTOMER",
                type: "decimal(18,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RII_PRODUCT_CATALOG",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CatalogType = table.Column<int>(type: "int", nullable: false),
                    BranchCode = table.Column<int>(type: "int", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
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
                    table.PrimaryKey("PK_RII_PRODUCT_CATALOG", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PRODUCT_CATALOG_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PRODUCT_CATALOG_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PRODUCT_CATALOG_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_PRODUCT_CATEGORY",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    FullPath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IconName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ColorHex = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsLeaf = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
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
                    table.PrimaryKey("PK_RII_PRODUCT_CATEGORY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PRODUCT_CATEGORY_RII_PRODUCT_CATEGORY_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "RII_PRODUCT_CATEGORY",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_PRODUCT_CATEGORY_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PRODUCT_CATEGORY_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PRODUCT_CATEGORY_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_CATALOG_CATEGORY",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatalogId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    ParentCatalogCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsRoot = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
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
                    table.PrimaryKey("PK_RII_CATALOG_CATEGORY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_CATALOG_CATEGORY_RII_CATALOG_CATEGORY_ParentCatalogCategoryId",
                        column: x => x.ParentCatalogCategoryId,
                        principalTable: "RII_CATALOG_CATEGORY",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_CATALOG_CATEGORY_RII_PRODUCT_CATALOG_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "RII_PRODUCT_CATALOG",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_CATALOG_CATEGORY_RII_PRODUCT_CATEGORY_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "RII_PRODUCT_CATEGORY",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_CATALOG_CATEGORY_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_CATALOG_CATEGORY_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_CATALOG_CATEGORY_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_PRODUCT_CATEGORY_RULE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    RuleName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    RuleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StockAttributeType = table.Column<int>(type: "int", nullable: false),
                    OperatorType = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
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
                    table.PrimaryKey("PK_RII_PRODUCT_CATEGORY_RULE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PRODUCT_CATEGORY_RULE_RII_PRODUCT_CATEGORY_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "RII_PRODUCT_CATEGORY",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_PRODUCT_CATEGORY_RULE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PRODUCT_CATEGORY_RULE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PRODUCT_CATEGORY_RULE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_STOCK_CATEGORY",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    AssignmentType = table.Column<int>(type: "int", nullable: false),
                    RuleId = table.Column<long>(type: "bigint", nullable: true),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_RII_STOCK_CATEGORY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_STOCK_CATEGORY_RII_PRODUCT_CATEGORY_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "RII_PRODUCT_CATEGORY",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_STOCK_CATEGORY_RII_PRODUCT_CATEGORY_RULE_RuleId",
                        column: x => x.RuleId,
                        principalTable: "RII_PRODUCT_CATEGORY_RULE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_STOCK_CATEGORY_RII_STOCK_StockId",
                        column: x => x.StockId,
                        principalTable: "RII_STOCK",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_STOCK_CATEGORY_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_STOCK_CATEGORY_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_STOCK_CATEGORY_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogCategory_IsDeleted",
                table: "RII_CATALOG_CATEGORY",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogCategory_ParentCatalogCategoryId",
                table: "RII_CATALOG_CATEGORY",
                column: "ParentCatalogCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CATALOG_CATEGORY_CategoryId",
                table: "RII_CATALOG_CATEGORY",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CATALOG_CATEGORY_CreatedBy",
                table: "RII_CATALOG_CATEGORY",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CATALOG_CATEGORY_DeletedBy",
                table: "RII_CATALOG_CATEGORY",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CATALOG_CATEGORY_UpdatedBy",
                table: "RII_CATALOG_CATEGORY",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UX_CatalogCategory_CatalogId_CategoryId",
                table: "RII_CATALOG_CATEGORY",
                columns: new[] { "CatalogId", "CategoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCatalog_IsDeleted",
                table: "RII_PRODUCT_CATALOG",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRODUCT_CATALOG_CreatedBy",
                table: "RII_PRODUCT_CATALOG",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRODUCT_CATALOG_DeletedBy",
                table: "RII_PRODUCT_CATALOG",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRODUCT_CATALOG_UpdatedBy",
                table: "RII_PRODUCT_CATALOG",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UX_ProductCatalog_Code",
                table: "RII_PRODUCT_CATALOG",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_IsDeleted",
                table: "RII_PRODUCT_CATEGORY",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_ParentCategoryId",
                table: "RII_PRODUCT_CATEGORY",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRODUCT_CATEGORY_CreatedBy",
                table: "RII_PRODUCT_CATEGORY",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRODUCT_CATEGORY_DeletedBy",
                table: "RII_PRODUCT_CATEGORY",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRODUCT_CATEGORY_UpdatedBy",
                table: "RII_PRODUCT_CATEGORY",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UX_ProductCategory_Code",
                table: "RII_PRODUCT_CATEGORY",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryRule_CategoryId",
                table: "RII_PRODUCT_CATEGORY_RULE",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryRule_IsDeleted",
                table: "RII_PRODUCT_CATEGORY_RULE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryRule_Priority",
                table: "RII_PRODUCT_CATEGORY_RULE",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRODUCT_CATEGORY_RULE_CreatedBy",
                table: "RII_PRODUCT_CATEGORY_RULE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRODUCT_CATEGORY_RULE_DeletedBy",
                table: "RII_PRODUCT_CATEGORY_RULE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PRODUCT_CATEGORY_RULE_UpdatedBy",
                table: "RII_PRODUCT_CATEGORY_RULE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_CATEGORY_CreatedBy",
                table: "RII_STOCK_CATEGORY",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_CATEGORY_DeletedBy",
                table: "RII_STOCK_CATEGORY",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_CATEGORY_RuleId",
                table: "RII_STOCK_CATEGORY",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_CATEGORY_UpdatedBy",
                table: "RII_STOCK_CATEGORY",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StockCategory_CategoryId",
                table: "RII_STOCK_CATEGORY",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StockCategory_IsDeleted",
                table: "RII_STOCK_CATEGORY",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "UX_StockCategory_StockId_CategoryId",
                table: "RII_STOCK_CATEGORY",
                columns: new[] { "StockId", "CategoryId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_CATALOG_CATEGORY");

            migrationBuilder.DropTable(
                name: "RII_STOCK_CATEGORY");

            migrationBuilder.DropTable(
                name: "RII_PRODUCT_CATALOG");

            migrationBuilder.DropTable(
                name: "RII_PRODUCT_CATEGORY_RULE");

            migrationBuilder.DropTable(
                name: "RII_PRODUCT_CATEGORY");

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "RII_USER_DETAIL",
                type: "decimal(5,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Height",
                table: "RII_USER_DETAIL",
                type: "decimal(5,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "RII_SHIPPING_ADDRESS",
                type: "decimal(9,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "RII_SHIPPING_ADDRESS",
                type: "decimal(9,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "RII_CUSTOMER",
                type: "decimal(9,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "RII_CUSTOMER",
                type: "decimal(9,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldNullable: true);
        }
    }
}
