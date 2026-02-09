using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crm_api.Migrations
{
    /// <inheritdoc />
    public partial class AlignActivitySchemaForReminderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID('RII_ACTIVITY', 'U') IS NULL
    RETURN;

IF COL_LENGTH('RII_ACTIVITY', 'StartDateTime') IS NULL
BEGIN
    EXEC('ALTER TABLE [RII_ACTIVITY] ADD [StartDateTime] datetime2 NULL;');

    IF COL_LENGTH('RII_ACTIVITY', 'ActivityDate') IS NOT NULL
        EXEC('UPDATE [RII_ACTIVITY] SET [StartDateTime] = [ActivityDate] WHERE [StartDateTime] IS NULL;');

    EXEC('UPDATE [RII_ACTIVITY] SET [StartDateTime] = ISNULL([StartDateTime], [CreatedDate]) WHERE [StartDateTime] IS NULL;');
    EXEC('ALTER TABLE [RII_ACTIVITY] ALTER COLUMN [StartDateTime] datetime2 NOT NULL;');
END;

IF COL_LENGTH('RII_ACTIVITY', 'EndDateTime') IS NULL
    EXEC('ALTER TABLE [RII_ACTIVITY] ADD [EndDateTime] datetime2 NULL;');

IF COL_LENGTH('RII_ACTIVITY', 'IsAllDay') IS NULL
    EXEC('ALTER TABLE [RII_ACTIVITY] ADD [IsAllDay] bit NOT NULL CONSTRAINT [DF_RII_ACTIVITY_IsAllDay] DEFAULT (0);');

IF EXISTS (
    SELECT 1
    FROM sys.columns c
    INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
    WHERE c.object_id = OBJECT_ID('RII_ACTIVITY')
      AND c.name = 'Status'
      AND t.name IN ('nvarchar', 'varchar', 'nchar', 'char')
)
BEGIN
    IF COL_LENGTH('RII_ACTIVITY', 'Status_New') IS NULL
        EXEC('ALTER TABLE [RII_ACTIVITY] ADD [Status_New] int NOT NULL CONSTRAINT [DF_RII_ACTIVITY_Status_New] DEFAULT (0);');

    EXEC('UPDATE [RII_ACTIVITY]
          SET [Status_New] = CASE
              WHEN LOWER(LTRIM(RTRIM(CAST([Status] AS nvarchar(50))))) = ''completed'' THEN 1
              WHEN LOWER(LTRIM(RTRIM(CAST([Status] AS nvarchar(50))))) IN (''cancelled'', ''canceled'') THEN 2
              ELSE 0
          END;');

    IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('RII_ACTIVITY') AND name = 'IX_Activity_Status')
        EXEC('DROP INDEX [IX_Activity_Status] ON [RII_ACTIVITY];');

    DECLARE @statusDfName sysname;
    SELECT @statusDfName = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c
        ON c.default_object_id = dc.object_id
       AND c.object_id = dc.parent_object_id
    WHERE dc.parent_object_id = OBJECT_ID('RII_ACTIVITY')
      AND c.name = 'Status';

    IF @statusDfName IS NOT NULL
        EXEC('ALTER TABLE [RII_ACTIVITY] DROP CONSTRAINT [' + @statusDfName + ']');

    EXEC('ALTER TABLE [RII_ACTIVITY] DROP COLUMN [Status];');
    EXEC('EXEC sp_rename ''RII_ACTIVITY.Status_New'', ''Status'', ''COLUMN'';');
END;

IF EXISTS (
    SELECT 1
    FROM sys.columns c
    INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
    WHERE c.object_id = OBJECT_ID('RII_ACTIVITY')
      AND c.name = 'Priority'
      AND t.name IN ('nvarchar', 'varchar', 'nchar', 'char')
)
BEGIN
    IF COL_LENGTH('RII_ACTIVITY', 'Priority_New') IS NULL
        EXEC('ALTER TABLE [RII_ACTIVITY] ADD [Priority_New] int NOT NULL CONSTRAINT [DF_RII_ACTIVITY_Priority_New] DEFAULT (1);');

    EXEC('UPDATE [RII_ACTIVITY]
          SET [Priority_New] = CASE
              WHEN LOWER(LTRIM(RTRIM(CAST([Priority] AS nvarchar(50))))) = ''low'' THEN 0
              WHEN LOWER(LTRIM(RTRIM(CAST([Priority] AS nvarchar(50))))) = ''high'' THEN 2
              ELSE 1
          END;');

    DECLARE @priorityDfName sysname;
    SELECT @priorityDfName = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c
        ON c.default_object_id = dc.object_id
       AND c.object_id = dc.parent_object_id
    WHERE dc.parent_object_id = OBJECT_ID('RII_ACTIVITY')
      AND c.name = 'Priority';

    IF @priorityDfName IS NOT NULL
        EXEC('ALTER TABLE [RII_ACTIVITY] DROP CONSTRAINT [' + @priorityDfName + ']');

    EXEC('ALTER TABLE [RII_ACTIVITY] DROP COLUMN [Priority];');
    EXEC('EXEC sp_rename ''RII_ACTIVITY.Priority_New'', ''Priority'', ''COLUMN'';');
END;

IF OBJECT_ID('RII_ACTIVITY_REMINDER', 'U') IS NULL
BEGIN
    CREATE TABLE [RII_ACTIVITY_REMINDER](
        [Id] bigint IDENTITY(1,1) NOT NULL,
        [ActivityId] bigint NOT NULL,
        [OffsetMinutes] int NOT NULL,
        [Channel] int NOT NULL CONSTRAINT [DF_RII_ACTIVITY_REMINDER_Channel] DEFAULT (0),
        [SentAt] datetime2 NULL,
        [Status] int NOT NULL CONSTRAINT [DF_RII_ACTIVITY_REMINDER_Status] DEFAULT (0),
        [CreatedDate] datetime2 NOT NULL CONSTRAINT [DF_RII_ACTIVITY_REMINDER_CreatedDate] DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL CONSTRAINT [DF_RII_ACTIVITY_REMINDER_IsDeleted] DEFAULT (0),
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_ACTIVITY_REMINDER] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_RII_ACTIVITY_REMINDER_RII_ACTIVITY_ActivityId')
    ALTER TABLE [RII_ACTIVITY_REMINDER]
    ADD CONSTRAINT [FK_RII_ACTIVITY_REMINDER_RII_ACTIVITY_ActivityId]
    FOREIGN KEY ([ActivityId]) REFERENCES [RII_ACTIVITY]([Id]) ON DELETE CASCADE;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_RII_ACTIVITY_REMINDER_RII_USERS_CreatedBy')
    ALTER TABLE [RII_ACTIVITY_REMINDER]
    ADD CONSTRAINT [FK_RII_ACTIVITY_REMINDER_RII_USERS_CreatedBy]
    FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS]([Id]) ON DELETE NO ACTION;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_RII_ACTIVITY_REMINDER_RII_USERS_UpdatedBy')
    ALTER TABLE [RII_ACTIVITY_REMINDER]
    ADD CONSTRAINT [FK_RII_ACTIVITY_REMINDER_RII_USERS_UpdatedBy]
    FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS]([Id]) ON DELETE NO ACTION;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_RII_ACTIVITY_REMINDER_RII_USERS_DeletedBy')
    ALTER TABLE [RII_ACTIVITY_REMINDER]
    ADD CONSTRAINT [FK_RII_ACTIVITY_REMINDER_RII_USERS_DeletedBy]
    FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS]([Id]) ON DELETE NO ACTION;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('RII_ACTIVITY_REMINDER') AND name = 'IX_ActivityReminder_ActivityId')
    CREATE INDEX [IX_ActivityReminder_ActivityId] ON [RII_ACTIVITY_REMINDER]([ActivityId]);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('RII_ACTIVITY_REMINDER') AND name = 'IX_ActivityReminder_Status')
    CREATE INDEX [IX_ActivityReminder_Status] ON [RII_ACTIVITY_REMINDER]([Status]);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('RII_ACTIVITY_REMINDER') AND name = 'IX_ActivityReminder_Channel')
    CREATE INDEX [IX_ActivityReminder_Channel] ON [RII_ACTIVITY_REMINDER]([Channel]);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('RII_ACTIVITY_REMINDER') AND name = 'IX_ActivityReminder_IsDeleted')
    CREATE INDEX [IX_ActivityReminder_IsDeleted] ON [RII_ACTIVITY_REMINDER]([IsDeleted]);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID('RII_ACTIVITY_REMINDER', 'U') IS NOT NULL
    DROP TABLE [RII_ACTIVITY_REMINDER];
");
        }
    }
}
