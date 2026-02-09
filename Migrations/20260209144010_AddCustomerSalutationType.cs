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
            migrationBuilder.Sql(@"
                UPDATE RII_ACTIVITY SET UpdatedBy = NULL WHERE UpdatedBy = 1;
                UPDATE RII_ACTIVITY SET CreatedBy = NULL WHERE CreatedBy = 1;
                UPDATE RII_ACTIVITY SET DeletedBy = NULL WHERE DeletedBy = 1;
                UPDATE RII_ACTIVITY_TYPE SET UpdatedBy = NULL WHERE UpdatedBy = 1;
                UPDATE RII_ACTIVITY_TYPE SET CreatedBy = NULL WHERE CreatedBy = 1;
                UPDATE RII_ACTIVITY_TYPE SET DeletedBy = NULL WHERE DeletedBy = 1;
                UPDATE RII_REPORT_TEMPLATES SET CreatedByUserId = NULL WHERE CreatedByUserId = 1;
                UPDATE RII_REPORT_TEMPLATES SET UpdatedByUserId = NULL WHERE UpdatedByUserId = 1;
                UPDATE RII_REPORT_TEMPLATES SET DeletedByUserId = NULL WHERE DeletedByUserId = 1;
                UPDATE RII_USER_PERMISSION_GROUPS SET CreatedBy = NULL WHERE CreatedBy = 1;
                UPDATE RII_USER_PERMISSION_GROUPS SET UpdatedBy = NULL WHERE UpdatedBy = 1;
                UPDATE RII_USER_PERMISSION_GROUPS SET DeletedBy = NULL WHERE DeletedBy = 1;
                UPDATE RII_APPROVAL_ACTION SET CreatedBy = NULL WHERE CreatedBy = 1;
                UPDATE RII_APPROVAL_ACTION SET UpdatedBy = NULL WHERE UpdatedBy = 1;
                UPDATE RII_APPROVAL_ACTION SET DeletedBy = NULL WHERE DeletedBy = 1;
                DELETE FROM RII_APPROVAL_ACTION WHERE ApprovedByUserId = 1;
                UPDATE RII_APPROVAL_FLOW SET CreatedBy = NULL WHERE CreatedBy = 1;
                UPDATE RII_APPROVAL_FLOW SET UpdatedBy = NULL WHERE UpdatedBy = 1;
                UPDATE RII_APPROVAL_FLOW SET DeletedBy = NULL WHERE DeletedBy = 1;
                UPDATE RII_APPROVAL_FLOW_STEP SET CreatedBy = NULL WHERE CreatedBy = 1;
                UPDATE RII_APPROVAL_FLOW_STEP SET UpdatedBy = NULL WHERE UpdatedBy = 1;
                UPDATE RII_APPROVAL_FLOW_STEP SET DeletedBy = NULL WHERE DeletedBy = 1;
                UPDATE RII_APPROVAL_REQUEST SET CreatedBy = NULL WHERE CreatedBy = 1;
                UPDATE RII_APPROVAL_REQUEST SET UpdatedBy = NULL WHERE UpdatedBy = 1;
                UPDATE RII_APPROVAL_REQUEST SET DeletedBy = NULL WHERE DeletedBy = 1;
                UPDATE RII_APPROVAL_ROLE SET CreatedBy = NULL WHERE CreatedBy = 1;
                UPDATE RII_APPROVAL_ROLE SET UpdatedBy = NULL WHERE UpdatedBy = 1;
                UPDATE RII_APPROVAL_ROLE SET DeletedBy = NULL WHERE DeletedBy = 1;
            ");

            migrationBuilder.Sql(@"
                IF COL_LENGTH('RII_SHIPPING_ADDRESS', 'IsDefault') IS NULL
                    ALTER TABLE [RII_SHIPPING_ADDRESS] ADD [IsDefault] bit NOT NULL DEFAULT CAST(0 AS bit);
                IF COL_LENGTH('RII_SHIPPING_ADDRESS', 'Name') IS NULL
                    ALTER TABLE [RII_SHIPPING_ADDRESS] ADD [Name] nvarchar(150) NULL;
                IF COL_LENGTH('RII_CUSTOMER', 'DefaultShippingAddressId') IS NULL
                    ALTER TABLE [RII_CUSTOMER] ADD [DefaultShippingAddressId] bigint NULL;
                IF COL_LENGTH('RII_CONTACT', 'FirstName') IS NULL
                    ALTER TABLE [RII_CONTACT] ADD [FirstName] nvarchar(100) NOT NULL DEFAULT N'';
                IF COL_LENGTH('RII_CONTACT', 'LastName') IS NULL
                    ALTER TABLE [RII_CONTACT] ADD [LastName] nvarchar(100) NOT NULL DEFAULT N'';
                IF COL_LENGTH('RII_CONTACT', 'MiddleName') IS NULL
                    ALTER TABLE [RII_CONTACT] ADD [MiddleName] nvarchar(100) NULL;
                IF COL_LENGTH('RII_CONTACT', 'Salutation') IS NULL
                    ALTER TABLE [RII_CONTACT] ADD [Salutation] int NOT NULL DEFAULT 0;
            ");

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

            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'IX_Customer_DefaultShippingAddressId'
                      AND object_id = OBJECT_ID('RII_CUSTOMER')
                )
                    CREATE INDEX [IX_Customer_DefaultShippingAddressId]
                    ON [RII_CUSTOMER] ([DefaultShippingAddressId]);

                IF NOT EXISTS (
                    SELECT 1 FROM sys.foreign_keys
                    WHERE name = 'FK_RII_CUSTOMER_RII_SHIPPING_ADDRESS_DefaultShippingAddressId'
                )
                    ALTER TABLE [RII_CUSTOMER]
                    ADD CONSTRAINT [FK_RII_CUSTOMER_RII_SHIPPING_ADDRESS_DefaultShippingAddressId]
                    FOREIGN KEY ([DefaultShippingAddressId])
                    REFERENCES [RII_SHIPPING_ADDRESS] ([Id])
                    ON DELETE SET NULL;
            ");
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
        }
    }
}
