using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddSerialPrefixAndLengthToDocumentSerialType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_APPROVAL_TRANSACTION");

            migrationBuilder.DropTable(
                name: "RII_APPROVAL_WORKFLOW");

            migrationBuilder.AddColumn<int>(
                name: "SerialLength",
                table: "RII_DOCUMENT_SERIAL_TYPE",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialPrefix",
                table: "RII_DOCUMENT_SERIAL_TYPE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerialLength",
                table: "RII_DOCUMENT_SERIAL_TYPE");

            migrationBuilder.DropColumn(
                name: "SerialPrefix",
                table: "RII_DOCUMENT_SERIAL_TYPE");

            migrationBuilder.CreateTable(
                name: "RII_APPROVAL_TRANSACTION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    DocumentId = table.Column<long>(type: "bigint", nullable: false),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalLevel = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_APPROVAL_TRANSACTION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_APPROVAL_TRANSACTION_RII_QUOTATION_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "RII_QUOTATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RII_APPROVAL_TRANSACTION_RII_QUOTATION_LINE_LineId",
                        column: x => x.LineId,
                        principalTable: "RII_QUOTATION_LINE",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_APPROVAL_TRANSACTION_RII_USERS_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_APPROVAL_TRANSACTION_RII_USERS_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_APPROVAL_TRANSACTION_RII_USERS_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_APPROVAL_TRANSACTION_RII_USERS_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_APPROVAL_WORKFLOW",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerTypeId = table.Column<long>(type: "bigint", nullable: false),
                    DeletedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    MaxAmount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    MinAmount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_APPROVAL_WORKFLOW", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_APPROVAL_WORKFLOW_RII_CUSTOMER_TYPE_CustomerTypeId",
                        column: x => x.CustomerTypeId,
                        principalTable: "RII_CUSTOMER_TYPE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RII_APPROVAL_WORKFLOW_RII_USERS_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_APPROVAL_WORKFLOW_RII_USERS_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_APPROVAL_WORKFLOW_RII_USERS_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_APPROVAL_WORKFLOW_RII_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_TRANSACTION_ApprovedByUserId",
                table: "RII_APPROVAL_TRANSACTION",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_TRANSACTION_CreatedByUserId",
                table: "RII_APPROVAL_TRANSACTION",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_TRANSACTION_DeletedByUserId",
                table: "RII_APPROVAL_TRANSACTION",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_TRANSACTION_DocumentId",
                table: "RII_APPROVAL_TRANSACTION",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_TRANSACTION_LineId",
                table: "RII_APPROVAL_TRANSACTION",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_TRANSACTION_UpdatedByUserId",
                table: "RII_APPROVAL_TRANSACTION",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_WORKFLOW_CreatedByUserId",
                table: "RII_APPROVAL_WORKFLOW",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_WORKFLOW_CustomerTypeId",
                table: "RII_APPROVAL_WORKFLOW",
                column: "CustomerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_WORKFLOW_DeletedByUserId",
                table: "RII_APPROVAL_WORKFLOW",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_WORKFLOW_UpdatedByUserId",
                table: "RII_APPROVAL_WORKFLOW",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_APPROVAL_WORKFLOW_UserId",
                table: "RII_APPROVAL_WORKFLOW",
                column: "UserId");
        }
    }
}
