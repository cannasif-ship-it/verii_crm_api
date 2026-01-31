-- Remove migration from history (if exists)
DELETE FROM [__EFMigrationsHistory] 
WHERE [MigrationId] = N'20260131185316_AddDemandIdToQuotationAndQuotationIdToOrder';
GO

-- Add DemandId to Quotation table (if not exists)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[RII_QUOTATION]') AND name = 'DemandId')
BEGIN
    ALTER TABLE [RII_QUOTATION] ADD [DemandId] bigint NULL;
END
GO

-- Create index for DemandId (if not exists)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[RII_QUOTATION]') AND name = 'IX_Quotation_DemandId')
BEGIN
    CREATE INDEX [IX_Quotation_DemandId] ON [RII_QUOTATION] ([DemandId]);
END
GO

-- Add foreign key for DemandId (if not exists)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_RII_QUOTATION_RII_DEMAND_DemandId]') AND parent_object_id = OBJECT_ID(N'[RII_QUOTATION]'))
BEGIN
    ALTER TABLE [RII_QUOTATION] 
    ADD CONSTRAINT [FK_RII_QUOTATION_RII_DEMAND_DemandId] 
    FOREIGN KEY ([DemandId]) REFERENCES [RII_DEMAND] ([Id]);
END
GO

-- Add QuotationId to Order table (if not exists)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[RII_ORDER]') AND name = 'QuotationId')
BEGIN
    ALTER TABLE [RII_ORDER] ADD [QuotationId] bigint NULL;
END
GO

-- Create index for QuotationId (if not exists)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[RII_ORDER]') AND name = 'IX_Order_QuotationId')
BEGIN
    CREATE INDEX [IX_Order_QuotationId] ON [RII_ORDER] ([QuotationId]);
END
GO

-- Add foreign key for QuotationId (if not exists)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_RII_ORDER_RII_QUOTATION_QuotationId]') AND parent_object_id = OBJECT_ID(N'[RII_ORDER]'))
BEGIN
    ALTER TABLE [RII_ORDER] 
    ADD CONSTRAINT [FK_RII_ORDER_RII_QUOTATION_QuotationId] 
    FOREIGN KEY ([QuotationId]) REFERENCES [RII_QUOTATION] ([Id]);
END
GO

-- Add migration to history
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260131185316_AddDemandIdToQuotationAndQuotationIdToOrder', N'8.0.11');
GO
