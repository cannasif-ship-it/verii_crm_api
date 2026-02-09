using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerSalutationType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RII_SMTP_SETTING",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "RII_USER_PERMISSION_GROUPS",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "RII_USERS",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "RII_SHIPPING_ADDRESS",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RII_SHIPPING_ADDRESS",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DefaultShippingAddressId",
                table: "RII_CUSTOMER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "TitleId",
                table: "RII_CONTACT",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "RII_CONTACT",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "RII_CONTACT",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "RII_CONTACT",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "RII_CONTACT",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Salutation",
                table: "RII_CONTACT",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_DefaultShippingAddressId",
                table: "RII_CUSTOMER",
                column: "DefaultShippingAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_CUSTOMER_RII_SHIPPING_ADDRESS_DefaultShippingAddressId",
                table: "RII_CUSTOMER",
                column: "DefaultShippingAddressId",
                principalTable: "RII_SHIPPING_ADDRESS",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RII_CUSTOMER_RII_SHIPPING_ADDRESS_DefaultShippingAddressId",
                table: "RII_CUSTOMER");

            migrationBuilder.DropIndex(
                name: "IX_Customer_DefaultShippingAddressId",
                table: "RII_CUSTOMER");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "RII_SHIPPING_ADDRESS");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "RII_SHIPPING_ADDRESS");

            migrationBuilder.DropColumn(
                name: "DefaultShippingAddressId",
                table: "RII_CUSTOMER");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "RII_CONTACT");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "RII_CONTACT");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "RII_CONTACT");

            migrationBuilder.DropColumn(
                name: "Salutation",
                table: "RII_CONTACT");

            migrationBuilder.AlterColumn<long>(
                name: "TitleId",
                table: "RII_CONTACT",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "RII_CONTACT",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.InsertData(
                table: "RII_SMTP_SETTING",
                columns: new[] { "Id", "CreatedBy", "CreatedByUserId", "CreatedDate", "DeletedBy", "DeletedByUserId", "DeletedDate", "EnableSsl", "FromEmail", "FromName", "Host", "IsDeleted", "PasswordEncrypted", "Port", "Timeout", "UpdatedBy", "UpdatedByUserId", "UpdatedDate", "Username" },
                values: new object[] { 1L, null, null, new DateTime(2026, 2, 7, 12, 44, 32, 621, DateTimeKind.Utc).AddTicks(3640), null, null, null, true, "", "V3RII CRM SYSTEM", "smtp.gmail.com", false, "", 587, 30, null, null, null, "" });

            migrationBuilder.InsertData(
                table: "RII_USERS",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "Email", "FirstName", "IsActive", "IsDeleted", "IsEmailConfirmed", "LastLoginDate", "LastName", "PasswordHash", "PhoneNumber", "RefreshToken", "RefreshTokenExpiryTime", "RoleId", "UpdatedBy", "UpdatedDate", "Username" },
                values: new object[] { 1L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "v3rii@v3rii.com", "Admin", true, false, true, null, "User", "$2a$11$abcdefghijklmnopqrstuuNIZsBQfUYLG05oQWoW6wLHKeQreQYs6", null, null, null, 3L, null, null, "admin@v3rii.com" });

            migrationBuilder.InsertData(
                table: "RII_USER_PERMISSION_GROUPS",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "IsDeleted", "PermissionGroupId", "UpdatedBy", "UpdatedDate", "UserId" },
                values: new object[] { 1L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1L, null, null, 1L });
        }
    }
}
