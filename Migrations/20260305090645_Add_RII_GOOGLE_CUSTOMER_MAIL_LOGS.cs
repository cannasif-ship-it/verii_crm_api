using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class Add_RII_GOOGLE_CUSTOMER_MAIL_LOGS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_GOOGLE_CUSTOMER_MAIL_LOGS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    SentByUserId = table.Column<long>(type: "bigint", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    SenderEmail = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true),
                    ToEmails = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CcEmails = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    BccEmails = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsHtml = table.Column<bool>(type: "bit", nullable: false),
                    TemplateKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    TemplateName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TemplateVersion = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    ErrorCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    GoogleMessageId = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    GoogleThreadId = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    SentAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    MetadataJson = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
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
                    table.PrimaryKey("PK_RII_GOOGLE_CUSTOMER_MAIL_LOGS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_GOOGLE_CUSTOMER_MAIL_LOGS_RII_CONTACT_ContactId",
                        column: x => x.ContactId,
                        principalTable: "RII_CONTACT",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_GOOGLE_CUSTOMER_MAIL_LOGS_RII_CUSTOMER_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "RII_CUSTOMER",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_GOOGLE_CUSTOMER_MAIL_LOGS_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_GOOGLE_CUSTOMER_MAIL_LOGS_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_GOOGLE_CUSTOMER_MAIL_LOGS_RII_USERS_SentByUserId",
                        column: x => x.SentByUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_GOOGLE_CUSTOMER_MAIL_LOGS_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoogleCustomerMailLogs_ContactId",
                table: "RII_GOOGLE_CUSTOMER_MAIL_LOGS",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_GoogleCustomerMailLogs_CreatedDate",
                table: "RII_GOOGLE_CUSTOMER_MAIL_LOGS",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_GoogleCustomerMailLogs_CustomerId",
                table: "RII_GOOGLE_CUSTOMER_MAIL_LOGS",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_GoogleCustomerMailLogs_IsSuccess",
                table: "RII_GOOGLE_CUSTOMER_MAIL_LOGS",
                column: "IsSuccess");

            migrationBuilder.CreateIndex(
                name: "IX_GoogleCustomerMailLogs_SentByUserId",
                table: "RII_GOOGLE_CUSTOMER_MAIL_LOGS",
                column: "SentByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GoogleCustomerMailLogs_TenantId",
                table: "RII_GOOGLE_CUSTOMER_MAIL_LOGS",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GOOGLE_CUSTOMER_MAIL_LOGS_CreatedBy",
                table: "RII_GOOGLE_CUSTOMER_MAIL_LOGS",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GOOGLE_CUSTOMER_MAIL_LOGS_DeletedBy",
                table: "RII_GOOGLE_CUSTOMER_MAIL_LOGS",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GOOGLE_CUSTOMER_MAIL_LOGS_UpdatedBy",
                table: "RII_GOOGLE_CUSTOMER_MAIL_LOGS",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_GOOGLE_CUSTOMER_MAIL_LOGS");
        }
    }
}
