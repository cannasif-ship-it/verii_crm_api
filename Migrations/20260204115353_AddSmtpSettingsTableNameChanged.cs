using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddSmtpSettingsTableNameChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmtpSettings_RII_USERS_CreatedByUserId",
                table: "SmtpSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_SmtpSettings_RII_USERS_DeletedByUserId",
                table: "SmtpSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_SmtpSettings_RII_USERS_UpdatedByUserId",
                table: "SmtpSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SmtpSettings",
                table: "SmtpSettings");

            migrationBuilder.RenameTable(
                name: "SmtpSettings",
                newName: "RII_SMTP_SETTING");

            migrationBuilder.RenameIndex(
                name: "IX_SmtpSettings_UpdatedByUserId",
                table: "RII_SMTP_SETTING",
                newName: "IX_RII_SMTP_SETTING_UpdatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_SmtpSettings_DeletedByUserId",
                table: "RII_SMTP_SETTING",
                newName: "IX_RII_SMTP_SETTING_DeletedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_SmtpSettings_CreatedByUserId",
                table: "RII_SMTP_SETTING",
                newName: "IX_RII_SMTP_SETTING_CreatedByUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RII_SMTP_SETTING",
                table: "RII_SMTP_SETTING",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "RII_SMTP_SETTING",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 4, 11, 53, 50, 511, DateTimeKind.Utc).AddTicks(8839));

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SMTP_SETTING_RII_USERS_CreatedByUserId",
                table: "RII_SMTP_SETTING",
                column: "CreatedByUserId",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SMTP_SETTING_RII_USERS_DeletedByUserId",
                table: "RII_SMTP_SETTING",
                column: "DeletedByUserId",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SMTP_SETTING_RII_USERS_UpdatedByUserId",
                table: "RII_SMTP_SETTING",
                column: "UpdatedByUserId",
                principalTable: "RII_USERS",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RII_SMTP_SETTING_RII_USERS_CreatedByUserId",
                table: "RII_SMTP_SETTING");

            migrationBuilder.DropForeignKey(
                name: "FK_RII_SMTP_SETTING_RII_USERS_DeletedByUserId",
                table: "RII_SMTP_SETTING");

            migrationBuilder.DropForeignKey(
                name: "FK_RII_SMTP_SETTING_RII_USERS_UpdatedByUserId",
                table: "RII_SMTP_SETTING");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RII_SMTP_SETTING",
                table: "RII_SMTP_SETTING");

            migrationBuilder.RenameTable(
                name: "RII_SMTP_SETTING",
                newName: "SmtpSettings");

            migrationBuilder.RenameIndex(
                name: "IX_RII_SMTP_SETTING_UpdatedByUserId",
                table: "SmtpSettings",
                newName: "IX_SmtpSettings_UpdatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_RII_SMTP_SETTING_DeletedByUserId",
                table: "SmtpSettings",
                newName: "IX_SmtpSettings_DeletedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_RII_SMTP_SETTING_CreatedByUserId",
                table: "SmtpSettings",
                newName: "IX_SmtpSettings_CreatedByUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SmtpSettings",
                table: "SmtpSettings",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "SmtpSettings",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedDate",
                value: new DateTime(2026, 2, 4, 11, 48, 43, 598, DateTimeKind.Utc).AddTicks(3942));

            migrationBuilder.AddForeignKey(
                name: "FK_SmtpSettings_RII_USERS_CreatedByUserId",
                table: "SmtpSettings",
                column: "CreatedByUserId",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SmtpSettings_RII_USERS_DeletedByUserId",
                table: "SmtpSettings",
                column: "DeletedByUserId",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SmtpSettings_RII_USERS_UpdatedByUserId",
                table: "SmtpSettings",
                column: "UpdatedByUserId",
                principalTable: "RII_USERS",
                principalColumn: "Id");
        }
    }
}
