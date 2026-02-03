using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AddReportBuilderReportDefinitions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RII_REPORT_DEFINITIONS')
BEGIN
    CREATE TABLE [RII_REPORT_DEFINITIONS] (
        [Id] bigint NOT NULL IDENTITY,
        [Name] nvarchar(200) NOT NULL,
        [Description] nvarchar(500) NULL,
        [ConnectionKey] nvarchar(20) NOT NULL,
        [DataSourceType] nvarchar(20) NOT NULL,
        [DataSourceName] nvarchar(128) NOT NULL,
        [ConfigJson] nvarchar(max) NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_REPORT_DEFINITIONS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_REPORT_DEFINITIONS_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_REPORT_DEFINITIONS_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_REPORT_DEFINITIONS_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
    CREATE INDEX [IX_RII_REPORT_DEFINITIONS_ConnectionKey_DataSourceType_DataSourceName] ON [RII_REPORT_DEFINITIONS] ([ConnectionKey], [DataSourceType], [DataSourceName]);
    CREATE UNIQUE INDEX [IX_RII_REPORT_DEFINITIONS_CreatedBy_Name] ON [RII_REPORT_DEFINITIONS] ([CreatedBy], [Name]) WHERE [IsDeleted] = 0;
    CREATE INDEX [IX_RII_REPORT_DEFINITIONS_DeletedBy] ON [RII_REPORT_DEFINITIONS] ([DeletedBy]);
    CREATE INDEX [IX_RII_REPORT_DEFINITIONS_UpdatedBy] ON [RII_REPORT_DEFINITIONS] ([UpdatedBy]);
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('RII_REPORT_DEFINITIONS') AND name = 'ConnectionKey')
    BEGIN
        ALTER TABLE [RII_REPORT_DEFINITIONS] ADD [ConnectionKey] nvarchar(20) NOT NULL DEFAULT 'CRM';
        IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_RII_REPORT_DEFINITIONS_ConnectionKey_DataSourceType_DataSourceName' AND object_id = OBJECT_ID('RII_REPORT_DEFINITIONS'))
            CREATE INDEX [IX_RII_REPORT_DEFINITIONS_ConnectionKey_DataSourceType_DataSourceName] ON [RII_REPORT_DEFINITIONS] ([ConnectionKey], [DataSourceType], [DataSourceName]);
    END
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_REPORT_DEFINITIONS");
        }
    }
}
