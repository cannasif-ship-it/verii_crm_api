IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_ACTIVITY] (
        [Id] bigint NOT NULL IDENTITY,
        [Subject] nvarchar(100) NOT NULL,
        [Description] nvarchar(500) NULL,
        [ActivityTypeId] bigint NULL,
        [PotentialCustomerId] bigint NULL,
        [ErpCustomerCode] nvarchar(50) NULL,
        [Status] nvarchar(50) NOT NULL,
        [IsCompleted] bit NOT NULL DEFAULT CAST(0 AS bit),
        [Priority] nvarchar(50) NULL,
        [ContactId] bigint NULL,
        [AssignedUserId] bigint NULL,
        [ActivityDate] datetime2 NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_ACTIVITY] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_ACTIVITY_TYPE] (
        [Id] bigint NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Description] nvarchar(500) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_ACTIVITY_TYPE] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_APPROVAL_ACTION] (
        [Id] bigint NOT NULL IDENTITY,
        [ApprovalRequestId] bigint NOT NULL,
        [StepOrder] int NOT NULL,
        [ApprovedByUserId] bigint NOT NULL,
        [ActionDate] datetime2 NOT NULL,
        [Status] int NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_APPROVAL_ACTION] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_APPROVAL_FLOW] (
        [Id] bigint NOT NULL IDENTITY,
        [DocumentType] int NOT NULL,
        [Description] nvarchar(200) NULL,
        [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_APPROVAL_FLOW] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_APPROVAL_FLOW_STEP] (
        [Id] bigint NOT NULL IDENTITY,
        [ApprovalFlowId] bigint NOT NULL,
        [StepOrder] int NOT NULL,
        [ApprovalRoleGroupId] bigint NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_APPROVAL_FLOW_STEP] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_APPROVAL_FLOW_STEP_RII_APPROVAL_FLOW_ApprovalFlowId] FOREIGN KEY ([ApprovalFlowId]) REFERENCES [RII_APPROVAL_FLOW] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_APPROVAL_REQUEST] (
        [Id] bigint NOT NULL IDENTITY,
        [EntityId] bigint NOT NULL,
        [DocumentType] int NOT NULL,
        [ApprovalFlowId] bigint NOT NULL,
        [CurrentStep] int NOT NULL DEFAULT 1,
        [Status] int NOT NULL DEFAULT 1,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_APPROVAL_REQUEST] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_APPROVAL_REQUEST_RII_APPROVAL_FLOW_ApprovalFlowId] FOREIGN KEY ([ApprovalFlowId]) REFERENCES [RII_APPROVAL_FLOW] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_APPROVAL_ROLE] (
        [Id] bigint NOT NULL IDENTITY,
        [ApprovalRoleGroupId] bigint NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_APPROVAL_ROLE] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_APPROVAL_ROLE_GROUP] (
        [Id] bigint NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_APPROVAL_ROLE_GROUP] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_APPROVAL_TRANSACTION] (
        [Id] bigint NOT NULL IDENTITY,
        [DocumentId] bigint NOT NULL,
        [LineId] bigint NULL,
        [ApprovalLevel] int NOT NULL,
        [Status] int NOT NULL,
        [ApprovedByUserId] bigint NULL,
        [RequestedAt] datetime2 NOT NULL,
        [ActionDate] datetime2 NULL,
        [Note] nvarchar(500) NULL,
        [CreatedDate] datetime2 NOT NULL,
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [CreatedByUserId] bigint NULL,
        [UpdatedBy] bigint NULL,
        [UpdatedByUserId] bigint NULL,
        [DeletedBy] bigint NULL,
        [DeletedByUserId] bigint NULL,
        CONSTRAINT [PK_RII_APPROVAL_TRANSACTION] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_APPROVAL_USER_ROLE] (
        [Id] bigint NOT NULL IDENTITY,
        [UserId] bigint NOT NULL,
        [ApprovalRoleId] bigint NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_APPROVAL_USER_ROLE] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_APPROVAL_USER_ROLE_RII_APPROVAL_ROLE_ApprovalRoleId] FOREIGN KEY ([ApprovalRoleId]) REFERENCES [RII_APPROVAL_ROLE] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_APPROVAL_WORKFLOW] (
        [Id] bigint NOT NULL IDENTITY,
        [CustomerTypeId] bigint NOT NULL,
        [UserId] bigint NULL,
        [MinAmount] decimal(18,6) NOT NULL,
        [MaxAmount] decimal(18,6) NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [CreatedByUserId] bigint NULL,
        [UpdatedBy] bigint NULL,
        [UpdatedByUserId] bigint NULL,
        [DeletedBy] bigint NULL,
        [DeletedByUserId] bigint NULL,
        CONSTRAINT [PK_RII_APPROVAL_WORKFLOW] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_CITY] (
        [Id] bigint NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [ERPCode] nvarchar(10) NULL,
        [CountryId] bigint NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_CITY] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_CONTACT] (
        [Id] bigint NOT NULL IDENTITY,
        [FullName] nvarchar(100) NOT NULL,
        [Email] nvarchar(100) NULL,
        [Phone] nvarchar(20) NULL,
        [Mobile] nvarchar(20) NULL,
        [Notes] nvarchar(250) NULL,
        [CustomerId] bigint NOT NULL,
        [TitleId] bigint NOT NULL,
        [CountryId] bigint NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_CONTACT] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_COUNTRY] (
        [Id] bigint NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Code] nvarchar(5) NOT NULL,
        [ERPCode] nvarchar(10) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_COUNTRY] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_CUSTOMER] (
        [Id] bigint NOT NULL IDENTITY,
        [CustomerCode] nvarchar(100) NULL,
        [CustomerName] nvarchar(250) NOT NULL,
        [CustomerTypeId] bigint NULL,
        [TaxOffice] nvarchar(100) NULL,
        [TaxNumber] nvarchar(50) NULL,
        [TcknNumber] nvarchar(11) NULL,
        [SalesRepCode] nvarchar(50) NULL,
        [GroupCode] nvarchar(50) NULL,
        [CreditLimit] decimal(18,6) NULL,
        [BranchCode] smallint NOT NULL,
        [BusinessUnitCode] smallint NOT NULL,
        [Notes] nvarchar(250) NULL,
        [Email] nvarchar(100) NULL,
        [Website] nvarchar(100) NULL,
        [Phone1] nvarchar(100) NULL,
        [Phone2] nvarchar(100) NULL,
        [Address] nvarchar(500) NULL,
        [CountryId] bigint NULL,
        [CityId] bigint NULL,
        [DistrictId] bigint NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        [Year] nvarchar(max) NOT NULL,
        [COMPLETION_DATE] datetime2 NULL,
        [IS_COMPLETED] bit NOT NULL DEFAULT CAST(0 AS bit),
        [IS_PENDING_APPROVAL] bit NOT NULL DEFAULT CAST(0 AS bit),
        [APPROVAL_STATUS] bit NULL,
        [REJECTED_REASON] nvarchar(250) NULL,
        [APPROVED_BY_USER_ID] bigint NULL,
        [APPROVAL_DATE] datetime2 NULL,
        [IS_ERP_INTEGRATED] bit NOT NULL DEFAULT CAST(0 AS bit),
        [ERP_INTEGRATION_NUMBER] nvarchar(100) NULL,
        [LAST_SYNC_DATE] datetime2 NULL,
        [COUNT_TRIED_BY] int NULL DEFAULT 0,
        CONSTRAINT [PK_RII_CUSTOMER] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_CUSTOMER_RII_CITY_CityId] FOREIGN KEY ([CityId]) REFERENCES [RII_CITY] ([Id]),
        CONSTRAINT [FK_RII_CUSTOMER_RII_COUNTRY_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [RII_COUNTRY] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_CUSTOMER_TYPE] (
        [Id] bigint NOT NULL IDENTITY,
        [Name] nvarchar(50) NOT NULL,
        [Description] nvarchar(255) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_CUSTOMER_TYPE] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_DISTRICT] (
        [Id] bigint NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [ERPCode] nvarchar(10) NULL,
        [CityId] bigint NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_DISTRICT] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_DISTRICT_RII_CITY_CityId] FOREIGN KEY ([CityId]) REFERENCES [RII_CITY] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_DOCUMENT_SERIAL_TYPE] (
        [Id] bigint NOT NULL IDENTITY,
        [RuleType] int NOT NULL,
        [CustomerTypeId] bigint NULL,
        [SalesRepId] bigint NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_DOCUMENT_SERIAL_TYPE] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_DOCUMENT_SERIAL_TYPE_RII_CUSTOMER_TYPE_CustomerTypeId] FOREIGN KEY ([CustomerTypeId]) REFERENCES [RII_CUSTOMER_TYPE] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_PASSWORD_RESET_REQUEST] (
        [Id] bigint NOT NULL IDENTITY,
        [UserId] bigint NOT NULL,
        [TokenHash] nvarchar(2000) NOT NULL,
        [ExpiresAt] datetime2 NOT NULL,
        [UsedAt] datetime2 NULL,
        [CreatedDate] datetime2 NOT NULL,
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [CreatedByUserId] bigint NULL,
        [UpdatedBy] bigint NULL,
        [UpdatedByUserId] bigint NULL,
        [DeletedBy] bigint NULL,
        [DeletedByUserId] bigint NULL,
        CONSTRAINT [PK_RII_PASSWORD_RESET_REQUEST] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_PAYMENT_TYPE] (
        [Id] bigint NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Description] nvarchar(500) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_PAYMENT_TYPE] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_PRICING_RULE_HEADER] (
        [Id] bigint NOT NULL IDENTITY,
        [RuleType] int NOT NULL,
        [RuleCode] nvarchar(50) NOT NULL,
        [RuleName] nvarchar(250) NOT NULL,
        [ValidFrom] datetime2 NOT NULL,
        [ValidTo] datetime2 NOT NULL,
        [CustomerId] bigint NULL,
        [ErpCustomerCode] nvarchar(50) NULL,
        [BranchCode] smallint NULL,
        [PriceIncludesVat] bit NOT NULL DEFAULT CAST(0 AS bit),
        [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_PRICING_RULE_HEADER] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_PRICING_RULE_HEADER_RII_CUSTOMER_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [RII_CUSTOMER] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_PRICING_RULE_LINE] (
        [Id] bigint NOT NULL IDENTITY,
        [PricingRuleHeaderId] bigint NOT NULL,
        [StokCode] nvarchar(50) NOT NULL,
        [MinQuantity] decimal(18,6) NOT NULL,
        [MaxQuantity] decimal(18,6) NULL,
        [FixedUnitPrice] decimal(18,6) NULL,
        [CurrencyCode] nvarchar(10) NOT NULL DEFAULT N'TRY',
        [DiscountRate1] decimal(18,6) NOT NULL DEFAULT 0.0,
        [DiscountAmount1] decimal(18,6) NOT NULL DEFAULT 0.0,
        [DiscountRate2] decimal(18,6) NOT NULL DEFAULT 0.0,
        [DiscountAmount2] decimal(18,6) NOT NULL DEFAULT 0.0,
        [DiscountRate3] decimal(18,6) NOT NULL DEFAULT 0.0,
        [DiscountAmount3] decimal(18,6) NOT NULL DEFAULT 0.0,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_PRICING_RULE_LINE] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_PRICING_RULE_LINE_RII_PRICING_RULE_HEADER_PricingRuleHeaderId] FOREIGN KEY ([PricingRuleHeaderId]) REFERENCES [RII_PRICING_RULE_HEADER] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_PRICING_RULE_SALESMAN] (
        [Id] bigint NOT NULL IDENTITY,
        [PricingRuleHeaderId] bigint NOT NULL,
        [SalesmanId] bigint NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_PRICING_RULE_SALESMAN] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_PRICING_RULE_SALESMAN_RII_PRICING_RULE_HEADER_PricingRuleHeaderId] FOREIGN KEY ([PricingRuleHeaderId]) REFERENCES [RII_PRICING_RULE_HEADER] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_PRODUCT_PRICING] (
        [Id] bigint NOT NULL IDENTITY,
        [ErpProductCode] nvarchar(50) NOT NULL,
        [ErpGroupCode] nvarchar(50) NOT NULL,
        [Currency] nvarchar(50) NOT NULL,
        [ListPrice] decimal(18,6) NOT NULL,
        [CostPrice] decimal(18,6) NOT NULL,
        [Discount1] decimal(18,6) NULL,
        [Discount2] decimal(18,6) NULL,
        [Discount3] decimal(18,6) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_PRODUCT_PRICING] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_PRODUCT_PRICING_GROUP_BY] (
        [Id] bigint NOT NULL IDENTITY,
        [ErpGroupCode] nvarchar(50) NOT NULL,
        [Currency] nvarchar(50) NOT NULL,
        [ListPrice] decimal(18,6) NOT NULL,
        [CostPrice] decimal(18,6) NOT NULL,
        [Discount1] decimal(18,6) NULL,
        [Discount2] decimal(18,6) NULL,
        [Discount3] decimal(18,6) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_PRODUCT_PRICING_GROUP_BY] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_QUOTATION] (
        [Id] bigint NOT NULL IDENTITY,
        [PotentialCustomerId] bigint NULL,
        [ErpCustomerCode] nvarchar(50) NULL,
        [ContactId] bigint NULL,
        [ValidUntil] datetime2 NULL,
        [DeliveryDate] datetime2 NULL,
        [ShippingAddressId] bigint NULL,
        [RepresentativeId] bigint NULL,
        [ActivityId] bigint NULL,
        [Status] int NULL,
        [Description] nvarchar(500) NULL,
        [PaymentTypeId] bigint NULL,
        [OfferType] nvarchar(50) NOT NULL,
        [OfferDate] datetime2 NULL,
        [OfferNo] nvarchar(50) NULL,
        [RevisionNo] nvarchar(50) NULL,
        [RevisionId] bigint NULL,
        [Currency] nvarchar(50) NOT NULL,
        [HasCustomerSpecificDiscount] bit NOT NULL DEFAULT CAST(0 AS bit),
        [Total] decimal(18,6) NOT NULL DEFAULT 0.0,
        [GrandTotal] decimal(18,6) NOT NULL DEFAULT 0.0,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        [Year] nvarchar(4) NOT NULL,
        [CompletionDate] datetime2 NULL,
        [IsCompleted] bit NOT NULL DEFAULT CAST(0 AS bit),
        [IsPendingApproval] bit NOT NULL DEFAULT CAST(0 AS bit),
        [ApprovalStatus] bit NULL,
        [RejectedReason] nvarchar(250) NULL,
        [ApprovedByUserId] bigint NULL,
        [ApprovalDate] datetime2 NULL,
        [IsERPIntegrated] bit NOT NULL DEFAULT CAST(0 AS bit),
        [ERPIntegrationNumber] nvarchar(100) NULL,
        [LastSyncDate] datetime2 NULL,
        [CountTriedBy] int NOT NULL DEFAULT 0,
        CONSTRAINT [PK_RII_QUOTATION] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_QUOTATION_RII_ACTIVITY_ActivityId] FOREIGN KEY ([ActivityId]) REFERENCES [RII_ACTIVITY] ([Id]),
        CONSTRAINT [FK_RII_QUOTATION_RII_CONTACT_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [RII_CONTACT] ([Id]),
        CONSTRAINT [FK_RII_QUOTATION_RII_CUSTOMER_PotentialCustomerId] FOREIGN KEY ([PotentialCustomerId]) REFERENCES [RII_CUSTOMER] ([Id]),
        CONSTRAINT [FK_RII_QUOTATION_RII_PAYMENT_TYPE_PaymentTypeId] FOREIGN KEY ([PaymentTypeId]) REFERENCES [RII_PAYMENT_TYPE] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_QUOTATION_EXCHANGE_RATE] (
        [Id] bigint NOT NULL IDENTITY,
        [QuotationId] bigint NOT NULL,
        [Currency] nvarchar(50) NOT NULL,
        [ExchangeRate] decimal(18,6) NOT NULL,
        [ExchangeRateDate] datetime2 NOT NULL,
        [IsOfficial] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_QUOTATION_EXCHANGE_RATE] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_QUOTATION_EXCHANGE_RATE_RII_QUOTATION_QuotationId] FOREIGN KEY ([QuotationId]) REFERENCES [RII_QUOTATION] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_QUOTATION_LINE] (
        [Id] bigint NOT NULL IDENTITY,
        [QuotationId] bigint NOT NULL,
        [ProductCode] nvarchar(100) NULL,
        [Quantity] decimal(18,6) NOT NULL,
        [UnitPrice] decimal(18,6) NOT NULL,
        [DiscountRate1] decimal(18,6) NOT NULL,
        [DiscountAmount1] decimal(18,6) NOT NULL,
        [DiscountRate2] decimal(18,6) NOT NULL,
        [DiscountAmount2] decimal(18,6) NOT NULL,
        [DiscountRate3] decimal(18,6) NOT NULL,
        [DiscountAmount3] decimal(18,6) NOT NULL,
        [VatRate] decimal(18,6) NOT NULL,
        [VatAmount] decimal(18,6) NOT NULL,
        [LineTotal] decimal(18,6) NOT NULL,
        [LineGrandTotal] decimal(18,6) NOT NULL,
        [Description] nvarchar(250) NULL,
        [ApprovalStatus] int NOT NULL,
        [PricingRuleHeaderId] bigint NULL,
        [RelatedStockId] bigint NULL,
        [RelatedProductKey] nvarchar(100) NULL,
        [IsMainRelatedProduct] bit NOT NULL DEFAULT CAST(0 AS bit),
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_QUOTATION_LINE] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_QUOTATION_LINE_RII_PRICING_RULE_HEADER_PricingRuleHeaderId] FOREIGN KEY ([PricingRuleHeaderId]) REFERENCES [RII_PRICING_RULE_HEADER] ([Id]) ON DELETE SET NULL,
        CONSTRAINT [FK_RII_QUOTATION_LINE_RII_QUOTATION_QuotationId] FOREIGN KEY ([QuotationId]) REFERENCES [RII_QUOTATION] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_SHIPPING_ADDRESS] (
        [Id] bigint NOT NULL IDENTITY,
        [Address] nvarchar(500) NOT NULL,
        [PostalCode] nvarchar(20) NULL,
        [ContactPerson] nvarchar(100) NULL,
        [Phone] nvarchar(20) NULL,
        [Notes] nvarchar(100) NULL,
        [CustomerId] bigint NOT NULL,
        [CountryId] bigint NULL,
        [CityId] bigint NULL,
        [DistrictId] bigint NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_SHIPPING_ADDRESS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_SHIPPING_ADDRESS_RII_CITY_CityId] FOREIGN KEY ([CityId]) REFERENCES [RII_CITY] ([Id]),
        CONSTRAINT [FK_RII_SHIPPING_ADDRESS_RII_COUNTRY_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [RII_COUNTRY] ([Id]),
        CONSTRAINT [FK_RII_SHIPPING_ADDRESS_RII_CUSTOMER_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [RII_CUSTOMER] ([Id]),
        CONSTRAINT [FK_RII_SHIPPING_ADDRESS_RII_DISTRICT_DistrictId] FOREIGN KEY ([DistrictId]) REFERENCES [RII_DISTRICT] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_STOCK] (
        [Id] bigint NOT NULL IDENTITY,
        [ErpStockCode] nvarchar(50) NOT NULL,
        [StockName] nvarchar(250) NOT NULL,
        [Unit] nvarchar(20) NULL,
        [UreticiKodu] nvarchar(50) NULL,
        [GrupKodu] nvarchar(50) NULL,
        [GrupAdi] nvarchar(250) NULL,
        [Kod1] nvarchar(50) NULL,
        [Kod1Adi] nvarchar(250) NULL,
        [Kod2] nvarchar(50) NULL,
        [Kod2Adi] nvarchar(250) NULL,
        [Kod3] nvarchar(50) NULL,
        [Kod3Adi] nvarchar(250) NULL,
        [Kod4] nvarchar(50) NULL,
        [Kod4Adi] nvarchar(250) NULL,
        [Kod5] nvarchar(50) NULL,
        [Kod5Adi] nvarchar(250) NULL,
        [BranchCode] int NOT NULL DEFAULT 0,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_STOCK] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_STOCK_DETAIL] (
        [Id] bigint NOT NULL IDENTITY,
        [StockId] bigint NOT NULL,
        [HtmlDescription] nvarchar(max) NOT NULL,
        [TechnicalSpecsJson] nvarchar(max) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_STOCK_DETAIL] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_STOCK_DETAIL_RII_STOCK_StockId] FOREIGN KEY ([StockId]) REFERENCES [RII_STOCK] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_STOCK_IMAGE] (
        [Id] bigint NOT NULL IDENTITY,
        [StockId] bigint NOT NULL,
        [FilePath] nvarchar(500) NOT NULL,
        [AltText] nvarchar(200) NULL,
        [SortOrder] int NOT NULL DEFAULT 0,
        [IsPrimary] bit NOT NULL DEFAULT CAST(0 AS bit),
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_STOCK_IMAGE] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_STOCK_IMAGE_RII_STOCK_StockId] FOREIGN KEY ([StockId]) REFERENCES [RII_STOCK] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_STOCK_RELATION] (
        [Id] bigint NOT NULL IDENTITY,
        [StockId] bigint NOT NULL,
        [RelatedStockId] bigint NOT NULL,
        [Quantity] decimal(18,6) NOT NULL,
        [Description] nvarchar(500) NULL,
        [IsMandatory] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_STOCK_RELATION] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_STOCK_RELATION_RII_STOCK_RelatedStockId] FOREIGN KEY ([RelatedStockId]) REFERENCES [RII_STOCK] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RII_STOCK_RELATION_RII_STOCK_StockId] FOREIGN KEY ([StockId]) REFERENCES [RII_STOCK] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_TITLE] (
        [Id] bigint NOT NULL IDENTITY,
        [TitleName] nvarchar(100) NOT NULL,
        [Code] nvarchar(10) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_TITLE] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_USER_AUTHORITY] (
        [Id] bigint NOT NULL IDENTITY,
        [Title] nvarchar(30) NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_USER_AUTHORITY] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_USERS] (
        [Id] bigint NOT NULL IDENTITY,
        [Username] nvarchar(50) NOT NULL,
        [Email] nvarchar(100) NOT NULL,
        [PasswordHash] nvarchar(255) NOT NULL,
        [FirstName] nvarchar(50) NULL,
        [LastName] nvarchar(50) NULL,
        [PhoneNumber] nvarchar(20) NULL,
        [RoleId] bigint NOT NULL,
        [IsEmailConfirmed] bit NOT NULL DEFAULT CAST(0 AS bit),
        [LastLoginDate] datetime2 NULL,
        [RefreshToken] nvarchar(500) NULL,
        [RefreshTokenExpiryTime] datetime2 NULL,
        [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_USERS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_USERS_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_USERS_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_USERS_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_USERS_RII_USER_AUTHORITY_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [RII_USER_AUTHORITY] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_USER_DETAIL] (
        [Id] bigint NOT NULL IDENTITY,
        [UserId] bigint NOT NULL,
        [ProfilePictureUrl] nvarchar(500) NULL,
        [Height] decimal(5,2) NULL,
        [Weight] decimal(5,2) NULL,
        [Description] nvarchar(2000) NULL,
        [Gender] tinyint NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_USER_DETAIL] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_USER_DETAIL_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_USER_DETAIL_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_USER_DETAIL_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_USER_DETAIL_RII_USERS_UserId] FOREIGN KEY ([UserId]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_USER_DISCOUNT_LIMIT] (
        [Id] bigint NOT NULL IDENTITY,
        [ErpProductGroupCode] nvarchar(50) NOT NULL,
        [SalespersonId] bigint NOT NULL,
        [MaxDiscount1] decimal(18,6) NOT NULL,
        [MaxDiscount2] decimal(18,6) NULL,
        [MaxDiscount3] decimal(18,6) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_USER_DISCOUNT_LIMIT] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_USER_DISCOUNT_LIMIT_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_USER_DISCOUNT_LIMIT_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_USER_DISCOUNT_LIMIT_RII_USERS_SalespersonId] FOREIGN KEY ([SalespersonId]) REFERENCES [RII_USERS] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RII_USER_DISCOUNT_LIMIT_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE TABLE [RII_USER_SESSION] (
        [Id] bigint NOT NULL IDENTITY,
        [UserId] bigint NOT NULL,
        [SessionId] uniqueidentifier NOT NULL,
        [Token] nvarchar(2000) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [RevokedAt] datetime2 NULL,
        [IpAddress] nvarchar(100) NULL,
        [UserAgent] nvarchar(500) NULL,
        [DeviceInfo] nvarchar(100) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_USER_SESSION] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_USER_SESSION_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_USER_SESSION_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_USER_SESSION_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_USER_SESSION_RII_USERS_UserId] FOREIGN KEY ([UserId]) REFERENCES [RII_USERS] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedBy', N'CreatedDate', N'DeletedBy', N'DeletedDate', N'IsDeleted', N'Title', N'UpdatedBy', N'UpdatedDate') AND [object_id] = OBJECT_ID(N'[RII_USER_AUTHORITY]'))
        SET IDENTITY_INSERT [RII_USER_AUTHORITY] ON;
    EXEC(N'INSERT INTO [RII_USER_AUTHORITY] ([Id], [CreatedBy], [CreatedDate], [DeletedBy], [DeletedDate], [IsDeleted], [Title], [UpdatedBy], [UpdatedDate])
    VALUES (CAST(1 AS bigint), NULL, ''2024-01-01T00:00:00.0000000Z'', NULL, NULL, CAST(0 AS bit), N''User'', NULL, NULL),
    (CAST(2 AS bigint), NULL, ''2024-01-01T00:00:00.0000000Z'', NULL, NULL, CAST(0 AS bit), N''Manager'', NULL, NULL),
    (CAST(3 AS bigint), NULL, ''2024-01-01T00:00:00.0000000Z'', NULL, NULL, CAST(0 AS bit), N''Admin'', NULL, NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedBy', N'CreatedDate', N'DeletedBy', N'DeletedDate', N'IsDeleted', N'Title', N'UpdatedBy', N'UpdatedDate') AND [object_id] = OBJECT_ID(N'[RII_USER_AUTHORITY]'))
        SET IDENTITY_INSERT [RII_USER_AUTHORITY] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedBy', N'CreatedDate', N'DeletedBy', N'DeletedDate', N'Email', N'FirstName', N'IsActive', N'IsDeleted', N'IsEmailConfirmed', N'LastLoginDate', N'LastName', N'PasswordHash', N'PhoneNumber', N'RefreshToken', N'RefreshTokenExpiryTime', N'RoleId', N'UpdatedBy', N'UpdatedDate', N'Username') AND [object_id] = OBJECT_ID(N'[RII_USERS]'))
        SET IDENTITY_INSERT [RII_USERS] ON;
    EXEC(N'INSERT INTO [RII_USERS] ([Id], [CreatedBy], [CreatedDate], [DeletedBy], [DeletedDate], [Email], [FirstName], [IsActive], [IsDeleted], [IsEmailConfirmed], [LastLoginDate], [LastName], [PasswordHash], [PhoneNumber], [RefreshToken], [RefreshTokenExpiryTime], [RoleId], [UpdatedBy], [UpdatedDate], [Username])
    VALUES (CAST(1 AS bigint), NULL, ''2024-01-01T00:00:00.0000000Z'', NULL, NULL, N''v3rii@v3rii.com'', N''Admin'', CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), NULL, N''User'', N''$2a$11$abcdefghijklmnopqrstuuNIZsBQfUYLG05oQWoW6wLHKeQreQYs6'', NULL, NULL, NULL, CAST(3 AS bigint), NULL, NULL, N''admin@v3rii.com'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedBy', N'CreatedDate', N'DeletedBy', N'DeletedDate', N'Email', N'FirstName', N'IsActive', N'IsDeleted', N'IsEmailConfirmed', N'LastLoginDate', N'LastName', N'PasswordHash', N'PhoneNumber', N'RefreshToken', N'RefreshTokenExpiryTime', N'RoleId', N'UpdatedBy', N'UpdatedDate', N'Username') AND [object_id] = OBJECT_ID(N'[RII_USERS]'))
        SET IDENTITY_INSERT [RII_USERS] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Activity_ActivityDate] ON [RII_ACTIVITY] ([ActivityDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Activity_ActivityTypeId] ON [RII_ACTIVITY] ([ActivityTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Activity_AssignedUserId] ON [RII_ACTIVITY] ([AssignedUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Activity_ContactId] ON [RII_ACTIVITY] ([ContactId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Activity_IsCompleted] ON [RII_ACTIVITY] ([IsCompleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Activity_IsDeleted] ON [RII_ACTIVITY] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Activity_PotentialCustomerId] ON [RII_ACTIVITY] ([PotentialCustomerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Activity_Status] ON [RII_ACTIVITY] ([Status]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Activity_Subject] ON [RII_ACTIVITY] ([Subject]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_ACTIVITY_CreatedBy] ON [RII_ACTIVITY] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_ACTIVITY_DeletedBy] ON [RII_ACTIVITY] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_ACTIVITY_UpdatedBy] ON [RII_ACTIVITY] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ActivityType_CreatedDate] ON [RII_ACTIVITY_TYPE] ([CreatedDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ActivityType_IsDeleted] ON [RII_ACTIVITY_TYPE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ActivityType_Name] ON [RII_ACTIVITY_TYPE] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_ACTIVITY_TYPE_CreatedBy] ON [RII_ACTIVITY_TYPE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_ACTIVITY_TYPE_DeletedBy] ON [RII_ACTIVITY_TYPE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_ACTIVITY_TYPE_UpdatedBy] ON [RII_ACTIVITY_TYPE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ApprovalAction_ApprovalRequestId] ON [RII_APPROVAL_ACTION] ([ApprovalRequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ApprovalAction_ApprovedByUserId] ON [RII_APPROVAL_ACTION] ([ApprovedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ApprovalAction_IsDeleted] ON [RII_APPROVAL_ACTION] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_ACTION_CreatedBy] ON [RII_APPROVAL_ACTION] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_ACTION_DeletedBy] ON [RII_APPROVAL_ACTION] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_ACTION_UpdatedBy] ON [RII_APPROVAL_ACTION] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ApprovalFlow_DocumentType] ON [RII_APPROVAL_FLOW] ([DocumentType]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ApprovalFlow_IsDeleted] ON [RII_APPROVAL_FLOW] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_FLOW_CreatedBy] ON [RII_APPROVAL_FLOW] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_FLOW_DeletedBy] ON [RII_APPROVAL_FLOW] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_FLOW_UpdatedBy] ON [RII_APPROVAL_FLOW] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ApprovalFlowStep_ApprovalFlowId] ON [RII_APPROVAL_FLOW_STEP] ([ApprovalFlowId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ApprovalFlowStep_ApprovalRoleGroupId] ON [RII_APPROVAL_FLOW_STEP] ([ApprovalRoleGroupId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ApprovalFlowStep_IsDeleted] ON [RII_APPROVAL_FLOW_STEP] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_FLOW_STEP_CreatedBy] ON [RII_APPROVAL_FLOW_STEP] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_FLOW_STEP_DeletedBy] ON [RII_APPROVAL_FLOW_STEP] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_FLOW_STEP_UpdatedBy] ON [RII_APPROVAL_FLOW_STEP] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ApprovalRequest_ApprovalFlowId] ON [RII_APPROVAL_REQUEST] ([ApprovalFlowId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ApprovalRequest_DocumentType] ON [RII_APPROVAL_REQUEST] ([DocumentType]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ApprovalRequest_EntityId] ON [RII_APPROVAL_REQUEST] ([EntityId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ApprovalRequest_IsDeleted] ON [RII_APPROVAL_REQUEST] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_REQUEST_CreatedBy] ON [RII_APPROVAL_REQUEST] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_REQUEST_DeletedBy] ON [RII_APPROVAL_REQUEST] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_REQUEST_UpdatedBy] ON [RII_APPROVAL_REQUEST] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ApprovalRole_ApprovalRoleGroupId] ON [RII_APPROVAL_ROLE] ([ApprovalRoleGroupId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ApprovalRole_IsDeleted] ON [RII_APPROVAL_ROLE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ApprovalRole_Name] ON [RII_APPROVAL_ROLE] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_ROLE_CreatedBy] ON [RII_APPROVAL_ROLE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_ROLE_DeletedBy] ON [RII_APPROVAL_ROLE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_ROLE_UpdatedBy] ON [RII_APPROVAL_ROLE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ApprovalRoleGroup_IsDeleted] ON [RII_APPROVAL_ROLE_GROUP] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ApprovalRoleGroup_Name] ON [RII_APPROVAL_ROLE_GROUP] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_ROLE_GROUP_CreatedBy] ON [RII_APPROVAL_ROLE_GROUP] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_ROLE_GROUP_DeletedBy] ON [RII_APPROVAL_ROLE_GROUP] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_ROLE_GROUP_UpdatedBy] ON [RII_APPROVAL_ROLE_GROUP] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_TRANSACTION_ApprovedByUserId] ON [RII_APPROVAL_TRANSACTION] ([ApprovedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_TRANSACTION_CreatedByUserId] ON [RII_APPROVAL_TRANSACTION] ([CreatedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_TRANSACTION_DeletedByUserId] ON [RII_APPROVAL_TRANSACTION] ([DeletedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_TRANSACTION_DocumentId] ON [RII_APPROVAL_TRANSACTION] ([DocumentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_TRANSACTION_LineId] ON [RII_APPROVAL_TRANSACTION] ([LineId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_TRANSACTION_UpdatedByUserId] ON [RII_APPROVAL_TRANSACTION] ([UpdatedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ApprovalUserRole_ApprovalRoleId] ON [RII_APPROVAL_USER_ROLE] ([ApprovalRoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ApprovalUserRole_IsDeleted] ON [RII_APPROVAL_USER_ROLE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ApprovalUserRole_UserId] ON [RII_APPROVAL_USER_ROLE] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_USER_ROLE_CreatedBy] ON [RII_APPROVAL_USER_ROLE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_USER_ROLE_DeletedBy] ON [RII_APPROVAL_USER_ROLE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_USER_ROLE_UpdatedBy] ON [RII_APPROVAL_USER_ROLE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_WORKFLOW_CreatedByUserId] ON [RII_APPROVAL_WORKFLOW] ([CreatedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_WORKFLOW_CustomerTypeId] ON [RII_APPROVAL_WORKFLOW] ([CustomerTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_WORKFLOW_DeletedByUserId] ON [RII_APPROVAL_WORKFLOW] ([DeletedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_WORKFLOW_UpdatedByUserId] ON [RII_APPROVAL_WORKFLOW] ([UpdatedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_APPROVAL_WORKFLOW_UserId] ON [RII_APPROVAL_WORKFLOW] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_City_CountryId] ON [RII_CITY] ([CountryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_City_IsDeleted] ON [RII_CITY] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_City_Name] ON [RII_CITY] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_CITY_CreatedBy] ON [RII_CITY] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_CITY_DeletedBy] ON [RII_CITY] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_CITY_UpdatedBy] ON [RII_CITY] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Contact_CustomerId] ON [RII_CONTACT] ([CustomerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Contact_Email] ON [RII_CONTACT] ([Email]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Contact_IsDeleted] ON [RII_CONTACT] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Contact_TitleId] ON [RII_CONTACT] ([TitleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_CONTACT_CountryId] ON [RII_CONTACT] ([CountryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_CONTACT_CreatedBy] ON [RII_CONTACT] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_CONTACT_DeletedBy] ON [RII_CONTACT] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_CONTACT_UpdatedBy] ON [RII_CONTACT] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Country_Code] ON [RII_COUNTRY] ([Code]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Country_IsDeleted] ON [RII_COUNTRY] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Country_Name] ON [RII_COUNTRY] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_COUNTRY_CreatedBy] ON [RII_COUNTRY] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_COUNTRY_DeletedBy] ON [RII_COUNTRY] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_COUNTRY_UpdatedBy] ON [RII_COUNTRY] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Customer_CityId] ON [RII_CUSTOMER] ([CityId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Customer_CountryId] ON [RII_CUSTOMER] ([CountryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Customer_CustomerCode] ON [RII_CUSTOMER] ([CustomerCode]) WHERE [CustomerCode] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Customer_CustomerTypeId] ON [RII_CUSTOMER] ([CustomerTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Customer_DistrictId] ON [RII_CUSTOMER] ([DistrictId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    EXEC(N'CREATE INDEX [IX_Customer_Email] ON [RII_CUSTOMER] ([Email]) WHERE [Email] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Customer_IsDeleted] ON [RII_CUSTOMER] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    EXEC(N'CREATE INDEX [IX_Customer_TaxNumber] ON [RII_CUSTOMER] ([TaxNumber]) WHERE [TaxNumber] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    EXEC(N'CREATE INDEX [IX_Customer_TcknNumber] ON [RII_CUSTOMER] ([TcknNumber]) WHERE [TcknNumber] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_CUSTOMER_APPROVAL_DATE] ON [RII_CUSTOMER] ([APPROVAL_DATE]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_CUSTOMER_APPROVAL_STATUS] ON [RII_CUSTOMER] ([APPROVAL_STATUS]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_CUSTOMER_APPROVED_BY_USER_ID] ON [RII_CUSTOMER] ([APPROVED_BY_USER_ID]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_CUSTOMER_CreatedBy] ON [RII_CUSTOMER] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_CUSTOMER_DeletedBy] ON [RII_CUSTOMER] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_CUSTOMER_IS_COMPLETED] ON [RII_CUSTOMER] ([IS_COMPLETED]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_CUSTOMER_UpdatedBy] ON [RII_CUSTOMER] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_CustomerType_IsDeleted] ON [RII_CUSTOMER_TYPE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_CustomerType_Name] ON [RII_CUSTOMER_TYPE] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_CUSTOMER_TYPE_CreatedBy] ON [RII_CUSTOMER_TYPE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_CUSTOMER_TYPE_DeletedBy] ON [RII_CUSTOMER_TYPE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_CUSTOMER_TYPE_UpdatedBy] ON [RII_CUSTOMER_TYPE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_District_CityId] ON [RII_DISTRICT] ([CityId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_District_ERPCode] ON [RII_DISTRICT] ([ERPCode]) WHERE [ERPCode] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_District_IsDeleted] ON [RII_DISTRICT] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_District_Name] ON [RII_DISTRICT] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_DISTRICT_CreatedBy] ON [RII_DISTRICT] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_DISTRICT_DeletedBy] ON [RII_DISTRICT] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_DISTRICT_UpdatedBy] ON [RII_DISTRICT] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_DocumentSerialType_CustomerTypeId] ON [RII_DOCUMENT_SERIAL_TYPE] ([CustomerTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_DocumentSerialType_IsDeleted] ON [RII_DOCUMENT_SERIAL_TYPE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_DocumentSerialType_RuleType] ON [RII_DOCUMENT_SERIAL_TYPE] ([RuleType]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_DocumentSerialType_SalesRepId] ON [RII_DOCUMENT_SERIAL_TYPE] ([SalesRepId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_DOCUMENT_SERIAL_TYPE_CreatedBy] ON [RII_DOCUMENT_SERIAL_TYPE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_DOCUMENT_SERIAL_TYPE_DeletedBy] ON [RII_DOCUMENT_SERIAL_TYPE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_DOCUMENT_SERIAL_TYPE_UpdatedBy] ON [RII_DOCUMENT_SERIAL_TYPE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PASSWORD_RESET_REQUEST_CreatedByUserId] ON [RII_PASSWORD_RESET_REQUEST] ([CreatedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PASSWORD_RESET_REQUEST_DeletedByUserId] ON [RII_PASSWORD_RESET_REQUEST] ([DeletedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PASSWORD_RESET_REQUEST_UpdatedByUserId] ON [RII_PASSWORD_RESET_REQUEST] ([UpdatedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PASSWORD_RESET_REQUEST_UserId] ON [RII_PASSWORD_RESET_REQUEST] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PaymentType_CreatedDate] ON [RII_PAYMENT_TYPE] ([CreatedDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PaymentType_IsDeleted] ON [RII_PAYMENT_TYPE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PaymentType_Name] ON [RII_PAYMENT_TYPE] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PAYMENT_TYPE_CreatedBy] ON [RII_PAYMENT_TYPE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PAYMENT_TYPE_DeletedBy] ON [RII_PAYMENT_TYPE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PAYMENT_TYPE_UpdatedBy] ON [RII_PAYMENT_TYPE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PricingRuleHeader_CustomerId] ON [RII_PRICING_RULE_HEADER] ([CustomerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PricingRuleHeader_IsActive] ON [RII_PRICING_RULE_HEADER] ([IsActive]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PricingRuleHeader_IsDeleted] ON [RII_PRICING_RULE_HEADER] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PricingRuleHeader_RuleCode] ON [RII_PRICING_RULE_HEADER] ([RuleCode]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PricingRuleHeader_RuleType] ON [RII_PRICING_RULE_HEADER] ([RuleType]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PricingRuleHeader_ValidFrom] ON [RII_PRICING_RULE_HEADER] ([ValidFrom]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PricingRuleHeader_ValidTo] ON [RII_PRICING_RULE_HEADER] ([ValidTo]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PRICING_RULE_HEADER_CreatedBy] ON [RII_PRICING_RULE_HEADER] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PRICING_RULE_HEADER_DeletedBy] ON [RII_PRICING_RULE_HEADER] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PRICING_RULE_HEADER_UpdatedBy] ON [RII_PRICING_RULE_HEADER] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PricingRuleLine_IsDeleted] ON [RII_PRICING_RULE_LINE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PricingRuleLine_PricingRuleHeaderId] ON [RII_PRICING_RULE_LINE] ([PricingRuleHeaderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PricingRuleLine_StokCode] ON [RII_PRICING_RULE_LINE] ([StokCode]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PRICING_RULE_LINE_CreatedBy] ON [RII_PRICING_RULE_LINE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PRICING_RULE_LINE_DeletedBy] ON [RII_PRICING_RULE_LINE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PRICING_RULE_LINE_UpdatedBy] ON [RII_PRICING_RULE_LINE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_PricingRuleSalesman_Header_Salesman_Unique] ON [RII_PRICING_RULE_SALESMAN] ([PricingRuleHeaderId], [SalesmanId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PricingRuleSalesman_IsDeleted] ON [RII_PRICING_RULE_SALESMAN] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PricingRuleSalesman_PricingRuleHeaderId] ON [RII_PRICING_RULE_SALESMAN] ([PricingRuleHeaderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PricingRuleSalesman_SalesmanId] ON [RII_PRICING_RULE_SALESMAN] ([SalesmanId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PRICING_RULE_SALESMAN_CreatedBy] ON [RII_PRICING_RULE_SALESMAN] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PRICING_RULE_SALESMAN_DeletedBy] ON [RII_PRICING_RULE_SALESMAN] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PRICING_RULE_SALESMAN_UpdatedBy] ON [RII_PRICING_RULE_SALESMAN] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ProductPricing_CreatedDate] ON [RII_PRODUCT_PRICING] ([CreatedDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ProductPricing_ErpGroupCode] ON [RII_PRODUCT_PRICING] ([ErpGroupCode]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ProductPricing_ErpProductCode] ON [RII_PRODUCT_PRICING] ([ErpProductCode]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ProductPricing_ErpProductCode_ErpGroupCode] ON [RII_PRODUCT_PRICING] ([ErpProductCode], [ErpGroupCode]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ProductPricing_IsDeleted] ON [RII_PRODUCT_PRICING] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PRODUCT_PRICING_CreatedBy] ON [RII_PRODUCT_PRICING] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PRODUCT_PRICING_DeletedBy] ON [RII_PRODUCT_PRICING] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PRODUCT_PRICING_UpdatedBy] ON [RII_PRODUCT_PRICING] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ProductPricingGroupBy_ErpGroupCode] ON [RII_PRODUCT_PRICING_GROUP_BY] ([ErpGroupCode]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ProductPricingGroupBy_IsDeleted] ON [RII_PRODUCT_PRICING_GROUP_BY] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PRODUCT_PRICING_GROUP_BY_CreatedBy] ON [RII_PRODUCT_PRICING_GROUP_BY] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PRODUCT_PRICING_GROUP_BY_DeletedBy] ON [RII_PRODUCT_PRICING_GROUP_BY] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_PRODUCT_PRICING_GROUP_BY_UpdatedBy] ON [RII_PRODUCT_PRICING_GROUP_BY] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Quotation_ActivityId] ON [RII_QUOTATION] ([ActivityId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Quotation_ApprovalDate] ON [RII_QUOTATION] ([ApprovalDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Quotation_ApprovalStatus] ON [RII_QUOTATION] ([ApprovalStatus]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Quotation_ApprovedByUserId] ON [RII_QUOTATION] ([ApprovedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Quotation_ContactId] ON [RII_QUOTATION] ([ContactId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Quotation_DeliveryDate] ON [RII_QUOTATION] ([DeliveryDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Quotation_IsCompleted] ON [RII_QUOTATION] ([IsCompleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Quotation_IsDeleted] ON [RII_QUOTATION] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Quotation_OfferDate] ON [RII_QUOTATION] ([OfferDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Quotation_OfferNo] ON [RII_QUOTATION] ([OfferNo]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Quotation_PaymentTypeId] ON [RII_QUOTATION] ([PaymentTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Quotation_PotentialCustomerId] ON [RII_QUOTATION] ([PotentialCustomerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Quotation_RepresentativeId] ON [RII_QUOTATION] ([RepresentativeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Quotation_ShippingAddressId] ON [RII_QUOTATION] ([ShippingAddressId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Quotation_Status] ON [RII_QUOTATION] ([Status]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Quotation_ValidUntil] ON [RII_QUOTATION] ([ValidUntil]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Quotation_Year] ON [RII_QUOTATION] ([Year]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_QUOTATION_CreatedBy] ON [RII_QUOTATION] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_QUOTATION_DeletedBy] ON [RII_QUOTATION] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_QUOTATION_UpdatedBy] ON [RII_QUOTATION] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_QuotationExchangeRate_Currency] ON [RII_QUOTATION_EXCHANGE_RATE] ([Currency]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_QuotationExchangeRate_ExchangeRateDate] ON [RII_QUOTATION_EXCHANGE_RATE] ([ExchangeRateDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_QuotationExchangeRate_IsDeleted] ON [RII_QUOTATION_EXCHANGE_RATE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_QuotationExchangeRate_IsOfficial] ON [RII_QUOTATION_EXCHANGE_RATE] ([IsOfficial]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_QuotationExchangeRate_QuotationId] ON [RII_QUOTATION_EXCHANGE_RATE] ([QuotationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_QUOTATION_EXCHANGE_RATE_CreatedBy] ON [RII_QUOTATION_EXCHANGE_RATE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_QUOTATION_EXCHANGE_RATE_DeletedBy] ON [RII_QUOTATION_EXCHANGE_RATE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_QUOTATION_EXCHANGE_RATE_UpdatedBy] ON [RII_QUOTATION_EXCHANGE_RATE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_QuotationLine_IsDeleted] ON [RII_QUOTATION_LINE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_QuotationLine_PricingRuleHeaderId] ON [RII_QUOTATION_LINE] ([PricingRuleHeaderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_QuotationLine_ProductCode] ON [RII_QUOTATION_LINE] ([ProductCode]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_QuotationLine_QuotationId] ON [RII_QUOTATION_LINE] ([QuotationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_QuotationLine_RelatedStockId] ON [RII_QUOTATION_LINE] ([RelatedStockId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_QUOTATION_LINE_CreatedBy] ON [RII_QUOTATION_LINE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_QUOTATION_LINE_DeletedBy] ON [RII_QUOTATION_LINE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_QUOTATION_LINE_UpdatedBy] ON [RII_QUOTATION_LINE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_SHIPPING_ADDRESS_CreatedBy] ON [RII_SHIPPING_ADDRESS] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_SHIPPING_ADDRESS_DeletedBy] ON [RII_SHIPPING_ADDRESS] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_SHIPPING_ADDRESS_UpdatedBy] ON [RII_SHIPPING_ADDRESS] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ShippingAddress_CityId] ON [RII_SHIPPING_ADDRESS] ([CityId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ShippingAddress_CountryId] ON [RII_SHIPPING_ADDRESS] ([CountryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ShippingAddress_CustomerId] ON [RII_SHIPPING_ADDRESS] ([CustomerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ShippingAddress_DistrictId] ON [RII_SHIPPING_ADDRESS] ([DistrictId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ShippingAddress_IsDeleted] ON [RII_SHIPPING_ADDRESS] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_STOCK_CreatedBy] ON [RII_STOCK] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_STOCK_DeletedBy] ON [RII_STOCK] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_STOCK_UpdatedBy] ON [RII_STOCK] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Stock_ErpStockCode] ON [RII_STOCK] ([ErpStockCode]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Stock_IsDeleted] ON [RII_STOCK] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Stock_StockName] ON [RII_STOCK] ([StockName]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_STOCK_DETAIL_CreatedBy] ON [RII_STOCK_DETAIL] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_STOCK_DETAIL_DeletedBy] ON [RII_STOCK_DETAIL] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_STOCK_DETAIL_UpdatedBy] ON [RII_STOCK_DETAIL] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_StockDetail_IsDeleted] ON [RII_STOCK_DETAIL] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_StockDetail_StockId] ON [RII_STOCK_DETAIL] ([StockId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_STOCK_IMAGE_CreatedBy] ON [RII_STOCK_IMAGE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_STOCK_IMAGE_DeletedBy] ON [RII_STOCK_IMAGE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_STOCK_IMAGE_UpdatedBy] ON [RII_STOCK_IMAGE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_StockImage_IsDeleted] ON [RII_STOCK_IMAGE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_StockImage_StockId] ON [RII_STOCK_IMAGE] ([StockId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_STOCK_RELATION_CreatedBy] ON [RII_STOCK_RELATION] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_STOCK_RELATION_DeletedBy] ON [RII_STOCK_RELATION] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_STOCK_RELATION_UpdatedBy] ON [RII_STOCK_RELATION] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_StockRelation_IsDeleted] ON [RII_STOCK_RELATION] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_StockRelation_RelatedStockId] ON [RII_STOCK_RELATION] ([RelatedStockId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_StockRelation_StockId] ON [RII_STOCK_RELATION] ([StockId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_StockRelation_StockId_RelatedStockId] ON [RII_STOCK_RELATION] ([StockId], [RelatedStockId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_TITLE_CreatedBy] ON [RII_TITLE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_TITLE_DeletedBy] ON [RII_TITLE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_TITLE_UpdatedBy] ON [RII_TITLE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Title_IsDeleted] ON [RII_TITLE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Title_TitleName] ON [RII_TITLE] ([TitleName]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_USER_AUTHORITY_CreatedBy] ON [RII_USER_AUTHORITY] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_USER_AUTHORITY_DeletedBy] ON [RII_USER_AUTHORITY] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_USER_AUTHORITY_UpdatedBy] ON [RII_USER_AUTHORITY] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_UserAuthority_IsDeleted] ON [RII_USER_AUTHORITY] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_UserAuthority_Title] ON [RII_USER_AUTHORITY] ([Title]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_USER_DETAIL_CreatedBy] ON [RII_USER_DETAIL] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_USER_DETAIL_DeletedBy] ON [RII_USER_DETAIL] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_USER_DETAIL_UpdatedBy] ON [RII_USER_DETAIL] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_UserDetail_IsDeleted] ON [RII_USER_DETAIL] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_UserDetail_UserId] ON [RII_USER_DETAIL] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_USER_DISCOUNT_LIMIT_CreatedBy] ON [RII_USER_DISCOUNT_LIMIT] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_USER_DISCOUNT_LIMIT_DeletedBy] ON [RII_USER_DISCOUNT_LIMIT] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_USER_DISCOUNT_LIMIT_UpdatedBy] ON [RII_USER_DISCOUNT_LIMIT] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_UserDiscountLimit_ErpProductGroupCode] ON [RII_USER_DISCOUNT_LIMIT] ([ErpProductGroupCode]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_UserDiscountLimit_IsDeleted] ON [RII_USER_DISCOUNT_LIMIT] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_UserDiscountLimit_SalespersonId] ON [RII_USER_DISCOUNT_LIMIT] ([SalespersonId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_UserDiscountLimit_SalespersonId_ErpProductGroupCode] ON [RII_USER_DISCOUNT_LIMIT] ([SalespersonId], [ErpProductGroupCode]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_USER_SESSION_CreatedBy] ON [RII_USER_SESSION] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_USER_SESSION_DeletedBy] ON [RII_USER_SESSION] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_USER_SESSION_UpdatedBy] ON [RII_USER_SESSION] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_UserSession_IsDeleted] ON [RII_USER_SESSION] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_UserSession_RevokedAt] ON [RII_USER_SESSION] ([RevokedAt]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_UserSession_SessionId] ON [RII_USER_SESSION] ([SessionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_UserSession_UserId] ON [RII_USER_SESSION] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_USERS_CreatedBy] ON [RII_USERS] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_USERS_DeletedBy] ON [RII_USERS] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_USERS_RoleId] ON [RII_USERS] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RII_USERS_UpdatedBy] ON [RII_USERS] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Users_Email] ON [RII_USERS] ([Email]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Users_IsDeleted] ON [RII_USERS] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Users_Username] ON [RII_USERS] ([Username]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_ACTIVITY] ADD CONSTRAINT [FK_RII_ACTIVITY_RII_ACTIVITY_TYPE_ActivityTypeId] FOREIGN KEY ([ActivityTypeId]) REFERENCES [RII_ACTIVITY_TYPE] ([Id]) ON DELETE SET NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_ACTIVITY] ADD CONSTRAINT [FK_RII_ACTIVITY_RII_CONTACT_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [RII_CONTACT] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_ACTIVITY] ADD CONSTRAINT [FK_RII_ACTIVITY_RII_CUSTOMER_PotentialCustomerId] FOREIGN KEY ([PotentialCustomerId]) REFERENCES [RII_CUSTOMER] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_ACTIVITY] ADD CONSTRAINT [FK_RII_ACTIVITY_RII_USERS_AssignedUserId] FOREIGN KEY ([AssignedUserId]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_ACTIVITY] ADD CONSTRAINT [FK_RII_ACTIVITY_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_ACTIVITY] ADD CONSTRAINT [FK_RII_ACTIVITY_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_ACTIVITY] ADD CONSTRAINT [FK_RII_ACTIVITY_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_ACTIVITY_TYPE] ADD CONSTRAINT [FK_RII_ACTIVITY_TYPE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_ACTIVITY_TYPE] ADD CONSTRAINT [FK_RII_ACTIVITY_TYPE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_ACTIVITY_TYPE] ADD CONSTRAINT [FK_RII_ACTIVITY_TYPE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_ACTION] ADD CONSTRAINT [FK_RII_APPROVAL_ACTION_RII_APPROVAL_REQUEST_ApprovalRequestId] FOREIGN KEY ([ApprovalRequestId]) REFERENCES [RII_APPROVAL_REQUEST] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_ACTION] ADD CONSTRAINT [FK_RII_APPROVAL_ACTION_RII_USERS_ApprovedByUserId] FOREIGN KEY ([ApprovedByUserId]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_ACTION] ADD CONSTRAINT [FK_RII_APPROVAL_ACTION_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_ACTION] ADD CONSTRAINT [FK_RII_APPROVAL_ACTION_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_ACTION] ADD CONSTRAINT [FK_RII_APPROVAL_ACTION_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_FLOW] ADD CONSTRAINT [FK_RII_APPROVAL_FLOW_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_FLOW] ADD CONSTRAINT [FK_RII_APPROVAL_FLOW_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_FLOW] ADD CONSTRAINT [FK_RII_APPROVAL_FLOW_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_FLOW_STEP] ADD CONSTRAINT [FK_RII_APPROVAL_FLOW_STEP_RII_APPROVAL_ROLE_GROUP_ApprovalRoleGroupId] FOREIGN KEY ([ApprovalRoleGroupId]) REFERENCES [RII_APPROVAL_ROLE_GROUP] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_FLOW_STEP] ADD CONSTRAINT [FK_RII_APPROVAL_FLOW_STEP_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_FLOW_STEP] ADD CONSTRAINT [FK_RII_APPROVAL_FLOW_STEP_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_FLOW_STEP] ADD CONSTRAINT [FK_RII_APPROVAL_FLOW_STEP_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_REQUEST] ADD CONSTRAINT [FK_RII_APPROVAL_REQUEST_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_REQUEST] ADD CONSTRAINT [FK_RII_APPROVAL_REQUEST_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_REQUEST] ADD CONSTRAINT [FK_RII_APPROVAL_REQUEST_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_ROLE] ADD CONSTRAINT [FK_RII_APPROVAL_ROLE_RII_APPROVAL_ROLE_GROUP_ApprovalRoleGroupId] FOREIGN KEY ([ApprovalRoleGroupId]) REFERENCES [RII_APPROVAL_ROLE_GROUP] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_ROLE] ADD CONSTRAINT [FK_RII_APPROVAL_ROLE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_ROLE] ADD CONSTRAINT [FK_RII_APPROVAL_ROLE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_ROLE] ADD CONSTRAINT [FK_RII_APPROVAL_ROLE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_ROLE_GROUP] ADD CONSTRAINT [FK_RII_APPROVAL_ROLE_GROUP_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_ROLE_GROUP] ADD CONSTRAINT [FK_RII_APPROVAL_ROLE_GROUP_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_ROLE_GROUP] ADD CONSTRAINT [FK_RII_APPROVAL_ROLE_GROUP_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_TRANSACTION] ADD CONSTRAINT [FK_RII_APPROVAL_TRANSACTION_RII_QUOTATION_DocumentId] FOREIGN KEY ([DocumentId]) REFERENCES [RII_QUOTATION] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_TRANSACTION] ADD CONSTRAINT [FK_RII_APPROVAL_TRANSACTION_RII_QUOTATION_LINE_LineId] FOREIGN KEY ([LineId]) REFERENCES [RII_QUOTATION_LINE] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_TRANSACTION] ADD CONSTRAINT [FK_RII_APPROVAL_TRANSACTION_RII_USERS_ApprovedByUserId] FOREIGN KEY ([ApprovedByUserId]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_TRANSACTION] ADD CONSTRAINT [FK_RII_APPROVAL_TRANSACTION_RII_USERS_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_TRANSACTION] ADD CONSTRAINT [FK_RII_APPROVAL_TRANSACTION_RII_USERS_DeletedByUserId] FOREIGN KEY ([DeletedByUserId]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_TRANSACTION] ADD CONSTRAINT [FK_RII_APPROVAL_TRANSACTION_RII_USERS_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_USER_ROLE] ADD CONSTRAINT [FK_RII_APPROVAL_USER_ROLE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_USER_ROLE] ADD CONSTRAINT [FK_RII_APPROVAL_USER_ROLE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_USER_ROLE] ADD CONSTRAINT [FK_RII_APPROVAL_USER_ROLE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_USER_ROLE] ADD CONSTRAINT [FK_RII_APPROVAL_USER_ROLE_RII_USERS_UserId] FOREIGN KEY ([UserId]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_WORKFLOW] ADD CONSTRAINT [FK_RII_APPROVAL_WORKFLOW_RII_CUSTOMER_TYPE_CustomerTypeId] FOREIGN KEY ([CustomerTypeId]) REFERENCES [RII_CUSTOMER_TYPE] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_WORKFLOW] ADD CONSTRAINT [FK_RII_APPROVAL_WORKFLOW_RII_USERS_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_WORKFLOW] ADD CONSTRAINT [FK_RII_APPROVAL_WORKFLOW_RII_USERS_DeletedByUserId] FOREIGN KEY ([DeletedByUserId]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_WORKFLOW] ADD CONSTRAINT [FK_RII_APPROVAL_WORKFLOW_RII_USERS_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_WORKFLOW] ADD CONSTRAINT [FK_RII_APPROVAL_WORKFLOW_RII_USERS_UserId] FOREIGN KEY ([UserId]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_CITY] ADD CONSTRAINT [FK_RII_CITY_RII_COUNTRY_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [RII_COUNTRY] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_CITY] ADD CONSTRAINT [FK_RII_CITY_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_CITY] ADD CONSTRAINT [FK_RII_CITY_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_CITY] ADD CONSTRAINT [FK_RII_CITY_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_CONTACT] ADD CONSTRAINT [FK_RII_CONTACT_RII_COUNTRY_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [RII_COUNTRY] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_CONTACT] ADD CONSTRAINT [FK_RII_CONTACT_RII_CUSTOMER_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [RII_CUSTOMER] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_CONTACT] ADD CONSTRAINT [FK_RII_CONTACT_RII_TITLE_TitleId] FOREIGN KEY ([TitleId]) REFERENCES [RII_TITLE] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_CONTACT] ADD CONSTRAINT [FK_RII_CONTACT_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_CONTACT] ADD CONSTRAINT [FK_RII_CONTACT_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_CONTACT] ADD CONSTRAINT [FK_RII_CONTACT_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_COUNTRY] ADD CONSTRAINT [FK_RII_COUNTRY_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_COUNTRY] ADD CONSTRAINT [FK_RII_COUNTRY_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_COUNTRY] ADD CONSTRAINT [FK_RII_COUNTRY_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_CUSTOMER] ADD CONSTRAINT [FK_RII_CUSTOMER_RII_CUSTOMER_TYPE_CustomerTypeId] FOREIGN KEY ([CustomerTypeId]) REFERENCES [RII_CUSTOMER_TYPE] ([Id]) ON DELETE SET NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_CUSTOMER] ADD CONSTRAINT [FK_RII_CUSTOMER_RII_DISTRICT_DistrictId] FOREIGN KEY ([DistrictId]) REFERENCES [RII_DISTRICT] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_CUSTOMER] ADD CONSTRAINT [FK_RII_CUSTOMER_RII_USERS_APPROVED_BY_USER_ID] FOREIGN KEY ([APPROVED_BY_USER_ID]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_CUSTOMER] ADD CONSTRAINT [FK_RII_CUSTOMER_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_CUSTOMER] ADD CONSTRAINT [FK_RII_CUSTOMER_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_CUSTOMER] ADD CONSTRAINT [FK_RII_CUSTOMER_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_CUSTOMER_TYPE] ADD CONSTRAINT [FK_RII_CUSTOMER_TYPE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_CUSTOMER_TYPE] ADD CONSTRAINT [FK_RII_CUSTOMER_TYPE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_CUSTOMER_TYPE] ADD CONSTRAINT [FK_RII_CUSTOMER_TYPE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_DISTRICT] ADD CONSTRAINT [FK_RII_DISTRICT_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_DISTRICT] ADD CONSTRAINT [FK_RII_DISTRICT_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_DISTRICT] ADD CONSTRAINT [FK_RII_DISTRICT_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_DOCUMENT_SERIAL_TYPE] ADD CONSTRAINT [FK_RII_DOCUMENT_SERIAL_TYPE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_DOCUMENT_SERIAL_TYPE] ADD CONSTRAINT [FK_RII_DOCUMENT_SERIAL_TYPE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_DOCUMENT_SERIAL_TYPE] ADD CONSTRAINT [FK_RII_DOCUMENT_SERIAL_TYPE_RII_USERS_SalesRepId] FOREIGN KEY ([SalesRepId]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_DOCUMENT_SERIAL_TYPE] ADD CONSTRAINT [FK_RII_DOCUMENT_SERIAL_TYPE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PASSWORD_RESET_REQUEST] ADD CONSTRAINT [FK_RII_PASSWORD_RESET_REQUEST_RII_USERS_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PASSWORD_RESET_REQUEST] ADD CONSTRAINT [FK_RII_PASSWORD_RESET_REQUEST_RII_USERS_DeletedByUserId] FOREIGN KEY ([DeletedByUserId]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PASSWORD_RESET_REQUEST] ADD CONSTRAINT [FK_RII_PASSWORD_RESET_REQUEST_RII_USERS_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PASSWORD_RESET_REQUEST] ADD CONSTRAINT [FK_RII_PASSWORD_RESET_REQUEST_RII_USERS_UserId] FOREIGN KEY ([UserId]) REFERENCES [RII_USERS] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PAYMENT_TYPE] ADD CONSTRAINT [FK_RII_PAYMENT_TYPE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PAYMENT_TYPE] ADD CONSTRAINT [FK_RII_PAYMENT_TYPE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PAYMENT_TYPE] ADD CONSTRAINT [FK_RII_PAYMENT_TYPE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PRICING_RULE_HEADER] ADD CONSTRAINT [FK_RII_PRICING_RULE_HEADER_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PRICING_RULE_HEADER] ADD CONSTRAINT [FK_RII_PRICING_RULE_HEADER_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PRICING_RULE_HEADER] ADD CONSTRAINT [FK_RII_PRICING_RULE_HEADER_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PRICING_RULE_LINE] ADD CONSTRAINT [FK_RII_PRICING_RULE_LINE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PRICING_RULE_LINE] ADD CONSTRAINT [FK_RII_PRICING_RULE_LINE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PRICING_RULE_LINE] ADD CONSTRAINT [FK_RII_PRICING_RULE_LINE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PRICING_RULE_SALESMAN] ADD CONSTRAINT [FK_RII_PRICING_RULE_SALESMAN_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PRICING_RULE_SALESMAN] ADD CONSTRAINT [FK_RII_PRICING_RULE_SALESMAN_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PRICING_RULE_SALESMAN] ADD CONSTRAINT [FK_RII_PRICING_RULE_SALESMAN_RII_USERS_SalesmanId] FOREIGN KEY ([SalesmanId]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PRICING_RULE_SALESMAN] ADD CONSTRAINT [FK_RII_PRICING_RULE_SALESMAN_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PRODUCT_PRICING] ADD CONSTRAINT [FK_RII_PRODUCT_PRICING_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PRODUCT_PRICING] ADD CONSTRAINT [FK_RII_PRODUCT_PRICING_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PRODUCT_PRICING] ADD CONSTRAINT [FK_RII_PRODUCT_PRICING_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PRODUCT_PRICING_GROUP_BY] ADD CONSTRAINT [FK_RII_PRODUCT_PRICING_GROUP_BY_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PRODUCT_PRICING_GROUP_BY] ADD CONSTRAINT [FK_RII_PRODUCT_PRICING_GROUP_BY_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_PRODUCT_PRICING_GROUP_BY] ADD CONSTRAINT [FK_RII_PRODUCT_PRICING_GROUP_BY_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_QUOTATION] ADD CONSTRAINT [FK_RII_QUOTATION_RII_SHIPPING_ADDRESS_ShippingAddressId] FOREIGN KEY ([ShippingAddressId]) REFERENCES [RII_SHIPPING_ADDRESS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_QUOTATION] ADD CONSTRAINT [FK_RII_QUOTATION_RII_USERS_ApprovedByUserId] FOREIGN KEY ([ApprovedByUserId]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_QUOTATION] ADD CONSTRAINT [FK_RII_QUOTATION_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_QUOTATION] ADD CONSTRAINT [FK_RII_QUOTATION_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_QUOTATION] ADD CONSTRAINT [FK_RII_QUOTATION_RII_USERS_RepresentativeId] FOREIGN KEY ([RepresentativeId]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_QUOTATION] ADD CONSTRAINT [FK_RII_QUOTATION_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_QUOTATION_EXCHANGE_RATE] ADD CONSTRAINT [FK_RII_QUOTATION_EXCHANGE_RATE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_QUOTATION_EXCHANGE_RATE] ADD CONSTRAINT [FK_RII_QUOTATION_EXCHANGE_RATE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_QUOTATION_EXCHANGE_RATE] ADD CONSTRAINT [FK_RII_QUOTATION_EXCHANGE_RATE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_QUOTATION_LINE] ADD CONSTRAINT [FK_RII_QUOTATION_LINE_RII_STOCK_RelatedStockId] FOREIGN KEY ([RelatedStockId]) REFERENCES [RII_STOCK] ([Id]) ON DELETE SET NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_QUOTATION_LINE] ADD CONSTRAINT [FK_RII_QUOTATION_LINE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_QUOTATION_LINE] ADD CONSTRAINT [FK_RII_QUOTATION_LINE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_QUOTATION_LINE] ADD CONSTRAINT [FK_RII_QUOTATION_LINE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_SHIPPING_ADDRESS] ADD CONSTRAINT [FK_RII_SHIPPING_ADDRESS_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_SHIPPING_ADDRESS] ADD CONSTRAINT [FK_RII_SHIPPING_ADDRESS_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_SHIPPING_ADDRESS] ADD CONSTRAINT [FK_RII_SHIPPING_ADDRESS_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_STOCK] ADD CONSTRAINT [FK_RII_STOCK_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_STOCK] ADD CONSTRAINT [FK_RII_STOCK_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_STOCK] ADD CONSTRAINT [FK_RII_STOCK_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_STOCK_DETAIL] ADD CONSTRAINT [FK_RII_STOCK_DETAIL_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_STOCK_DETAIL] ADD CONSTRAINT [FK_RII_STOCK_DETAIL_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_STOCK_DETAIL] ADD CONSTRAINT [FK_RII_STOCK_DETAIL_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_STOCK_IMAGE] ADD CONSTRAINT [FK_RII_STOCK_IMAGE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_STOCK_IMAGE] ADD CONSTRAINT [FK_RII_STOCK_IMAGE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_STOCK_IMAGE] ADD CONSTRAINT [FK_RII_STOCK_IMAGE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_STOCK_RELATION] ADD CONSTRAINT [FK_RII_STOCK_RELATION_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_STOCK_RELATION] ADD CONSTRAINT [FK_RII_STOCK_RELATION_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_STOCK_RELATION] ADD CONSTRAINT [FK_RII_STOCK_RELATION_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_TITLE] ADD CONSTRAINT [FK_RII_TITLE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_TITLE] ADD CONSTRAINT [FK_RII_TITLE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_TITLE] ADD CONSTRAINT [FK_RII_TITLE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_USER_AUTHORITY] ADD CONSTRAINT [FK_RII_USER_AUTHORITY_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_USER_AUTHORITY] ADD CONSTRAINT [FK_RII_USER_AUTHORITY_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    ALTER TABLE [RII_USER_AUTHORITY] ADD CONSTRAINT [FK_RII_USER_AUTHORITY_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260118170226_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260118170226_InitialCreate', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260120065400_AddMaxAmountToApprovalRole'
)
BEGIN
    ALTER TABLE [RII_APPROVAL_ROLE] ADD [MaxAmount] decimal(18,6) NOT NULL DEFAULT 0.0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260120065400_AddMaxAmountToApprovalRole'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260120065400_AddMaxAmountToApprovalRole', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260121072652_AddSerialPrefixAndLengthToDocumentSerialType'
)
BEGIN
    DROP TABLE [RII_APPROVAL_TRANSACTION];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260121072652_AddSerialPrefixAndLengthToDocumentSerialType'
)
BEGIN
    DROP TABLE [RII_APPROVAL_WORKFLOW];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260121072652_AddSerialPrefixAndLengthToDocumentSerialType'
)
BEGIN
    ALTER TABLE [RII_DOCUMENT_SERIAL_TYPE] ADD [SerialLength] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260121072652_AddSerialPrefixAndLengthToDocumentSerialType'
)
BEGIN
    ALTER TABLE [RII_DOCUMENT_SERIAL_TYPE] ADD [SerialPrefix] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260121072652_AddSerialPrefixAndLengthToDocumentSerialType'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260121072652_AddSerialPrefixAndLengthToDocumentSerialType', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260121080224_AddSerialFieldsToDocumentSerialType'
)
BEGIN
    ALTER TABLE [RII_DOCUMENT_SERIAL_TYPE] ADD [SerialCurrent] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260121080224_AddSerialFieldsToDocumentSerialType'
)
BEGIN
    ALTER TABLE [RII_DOCUMENT_SERIAL_TYPE] ADD [SerialIncrement] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260121080224_AddSerialFieldsToDocumentSerialType'
)
BEGIN
    ALTER TABLE [RII_DOCUMENT_SERIAL_TYPE] ADD [SerialStart] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260121080224_AddSerialFieldsToDocumentSerialType'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260121080224_AddSerialFieldsToDocumentSerialType', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122135832_AddDocumentSerialTypeToQuotation'
)
BEGIN
    ALTER TABLE [RII_QUOTATION] ADD [DocumentSerialTypeId] bigint NOT NULL DEFAULT CAST(0 AS bigint);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122135832_AddDocumentSerialTypeToQuotation'
)
BEGIN
    CREATE INDEX [IX_Quotation_DocumentSerialTypeId] ON [RII_QUOTATION] ([DocumentSerialTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122135832_AddDocumentSerialTypeToQuotation'
)
BEGIN
    ALTER TABLE [RII_QUOTATION] ADD CONSTRAINT [FK_RII_QUOTATION_RII_DOCUMENT_SERIAL_TYPE_DocumentSerialTypeId] FOREIGN KEY ([DocumentSerialTypeId]) REFERENCES [RII_DOCUMENT_SERIAL_TYPE] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260122135832_AddDocumentSerialTypeToQuotation'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260122135832_AddDocumentSerialTypeToQuotation', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE TABLE [RII_DEMAND] (
        [Id] bigint NOT NULL IDENTITY,
        [PotentialCustomerId] bigint NULL,
        [ErpCustomerCode] nvarchar(50) NULL,
        [ContactId] bigint NULL,
        [ValidUntil] datetime2 NULL,
        [DeliveryDate] datetime2 NULL,
        [ShippingAddressId] bigint NULL,
        [RepresentativeId] bigint NULL,
        [ActivityId] bigint NULL,
        [Status] int NULL,
        [Description] nvarchar(500) NULL,
        [PaymentTypeId] bigint NULL,
        [DocumentSerialTypeId] bigint NOT NULL,
        [OfferType] nvarchar(50) NOT NULL,
        [OfferDate] datetime2 NULL,
        [OfferNo] nvarchar(50) NULL,
        [RevisionNo] nvarchar(50) NULL,
        [RevisionId] bigint NULL,
        [Currency] nvarchar(50) NOT NULL,
        [HasCustomerSpecificDiscount] bit NOT NULL DEFAULT CAST(0 AS bit),
        [Total] decimal(18,6) NOT NULL DEFAULT 0.0,
        [GrandTotal] decimal(18,6) NOT NULL DEFAULT 0.0,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        [Year] nvarchar(4) NOT NULL,
        [CompletionDate] datetime2 NULL,
        [IsCompleted] bit NOT NULL DEFAULT CAST(0 AS bit),
        [IsPendingApproval] bit NOT NULL DEFAULT CAST(0 AS bit),
        [ApprovalStatus] bit NULL,
        [RejectedReason] nvarchar(250) NULL,
        [ApprovedByUserId] bigint NULL,
        [ApprovalDate] datetime2 NULL,
        [IsERPIntegrated] bit NOT NULL DEFAULT CAST(0 AS bit),
        [ERPIntegrationNumber] nvarchar(100) NULL,
        [LastSyncDate] datetime2 NULL,
        [CountTriedBy] int NOT NULL DEFAULT 0,
        CONSTRAINT [PK_RII_DEMAND] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_DEMAND_RII_ACTIVITY_ActivityId] FOREIGN KEY ([ActivityId]) REFERENCES [RII_ACTIVITY] ([Id]),
        CONSTRAINT [FK_RII_DEMAND_RII_CONTACT_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [RII_CONTACT] ([Id]),
        CONSTRAINT [FK_RII_DEMAND_RII_CUSTOMER_PotentialCustomerId] FOREIGN KEY ([PotentialCustomerId]) REFERENCES [RII_CUSTOMER] ([Id]),
        CONSTRAINT [FK_RII_DEMAND_RII_DOCUMENT_SERIAL_TYPE_DocumentSerialTypeId] FOREIGN KEY ([DocumentSerialTypeId]) REFERENCES [RII_DOCUMENT_SERIAL_TYPE] ([Id]),
        CONSTRAINT [FK_RII_DEMAND_RII_PAYMENT_TYPE_PaymentTypeId] FOREIGN KEY ([PaymentTypeId]) REFERENCES [RII_PAYMENT_TYPE] ([Id]),
        CONSTRAINT [FK_RII_DEMAND_RII_SHIPPING_ADDRESS_ShippingAddressId] FOREIGN KEY ([ShippingAddressId]) REFERENCES [RII_SHIPPING_ADDRESS] ([Id]),
        CONSTRAINT [FK_RII_DEMAND_RII_USERS_ApprovedByUserId] FOREIGN KEY ([ApprovedByUserId]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_DEMAND_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_DEMAND_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_DEMAND_RII_USERS_RepresentativeId] FOREIGN KEY ([RepresentativeId]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_DEMAND_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE TABLE [RII_ORDER] (
        [Id] bigint NOT NULL IDENTITY,
        [PotentialCustomerId] bigint NULL,
        [ErpCustomerCode] nvarchar(50) NULL,
        [ContactId] bigint NULL,
        [ValidUntil] datetime2 NULL,
        [DeliveryDate] datetime2 NULL,
        [ShippingAddressId] bigint NULL,
        [RepresentativeId] bigint NULL,
        [ActivityId] bigint NULL,
        [Status] int NULL,
        [Description] nvarchar(500) NULL,
        [PaymentTypeId] bigint NULL,
        [DocumentSerialTypeId] bigint NOT NULL,
        [OfferType] nvarchar(50) NOT NULL,
        [OfferDate] datetime2 NULL,
        [OfferNo] nvarchar(50) NULL,
        [RevisionNo] nvarchar(50) NULL,
        [RevisionId] bigint NULL,
        [Currency] nvarchar(50) NOT NULL,
        [HasCustomerSpecificDiscount] bit NOT NULL DEFAULT CAST(0 AS bit),
        [Total] decimal(18,6) NOT NULL DEFAULT 0.0,
        [GrandTotal] decimal(18,6) NOT NULL DEFAULT 0.0,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        [Year] nvarchar(4) NOT NULL,
        [CompletionDate] datetime2 NULL,
        [IsCompleted] bit NOT NULL DEFAULT CAST(0 AS bit),
        [IsPendingApproval] bit NOT NULL DEFAULT CAST(0 AS bit),
        [ApprovalStatus] bit NULL,
        [RejectedReason] nvarchar(250) NULL,
        [ApprovedByUserId] bigint NULL,
        [ApprovalDate] datetime2 NULL,
        [IsERPIntegrated] bit NOT NULL DEFAULT CAST(0 AS bit),
        [ERPIntegrationNumber] nvarchar(100) NULL,
        [LastSyncDate] datetime2 NULL,
        [CountTriedBy] int NOT NULL DEFAULT 0,
        CONSTRAINT [PK_RII_ORDER] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_ORDER_RII_ACTIVITY_ActivityId] FOREIGN KEY ([ActivityId]) REFERENCES [RII_ACTIVITY] ([Id]),
        CONSTRAINT [FK_RII_ORDER_RII_CONTACT_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [RII_CONTACT] ([Id]),
        CONSTRAINT [FK_RII_ORDER_RII_CUSTOMER_PotentialCustomerId] FOREIGN KEY ([PotentialCustomerId]) REFERENCES [RII_CUSTOMER] ([Id]),
        CONSTRAINT [FK_RII_ORDER_RII_DOCUMENT_SERIAL_TYPE_DocumentSerialTypeId] FOREIGN KEY ([DocumentSerialTypeId]) REFERENCES [RII_DOCUMENT_SERIAL_TYPE] ([Id]),
        CONSTRAINT [FK_RII_ORDER_RII_PAYMENT_TYPE_PaymentTypeId] FOREIGN KEY ([PaymentTypeId]) REFERENCES [RII_PAYMENT_TYPE] ([Id]),
        CONSTRAINT [FK_RII_ORDER_RII_SHIPPING_ADDRESS_ShippingAddressId] FOREIGN KEY ([ShippingAddressId]) REFERENCES [RII_SHIPPING_ADDRESS] ([Id]),
        CONSTRAINT [FK_RII_ORDER_RII_USERS_ApprovedByUserId] FOREIGN KEY ([ApprovedByUserId]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_ORDER_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_ORDER_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_ORDER_RII_USERS_RepresentativeId] FOREIGN KEY ([RepresentativeId]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_ORDER_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE TABLE [RII_DEMAND_EXCHANGE_RATE] (
        [Id] bigint NOT NULL IDENTITY,
        [DemandId] bigint NOT NULL,
        [Currency] nvarchar(50) NOT NULL,
        [ExchangeRate] decimal(18,6) NOT NULL,
        [ExchangeRateDate] datetime2 NOT NULL,
        [IsOfficial] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_DEMAND_EXCHANGE_RATE] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_DEMAND_EXCHANGE_RATE_RII_DEMAND_DemandId] FOREIGN KEY ([DemandId]) REFERENCES [RII_DEMAND] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RII_DEMAND_EXCHANGE_RATE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_DEMAND_EXCHANGE_RATE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_DEMAND_EXCHANGE_RATE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE TABLE [RII_DEMAND_LINE] (
        [Id] bigint NOT NULL IDENTITY,
        [DemandId] bigint NOT NULL,
        [ProductCode] nvarchar(100) NULL,
        [Quantity] decimal(18,6) NOT NULL,
        [UnitPrice] decimal(18,6) NOT NULL,
        [DiscountRate1] decimal(18,6) NOT NULL,
        [DiscountAmount1] decimal(18,6) NOT NULL,
        [DiscountRate2] decimal(18,6) NOT NULL,
        [DiscountAmount2] decimal(18,6) NOT NULL,
        [DiscountRate3] decimal(18,6) NOT NULL,
        [DiscountAmount3] decimal(18,6) NOT NULL,
        [VatRate] decimal(18,6) NOT NULL,
        [VatAmount] decimal(18,6) NOT NULL,
        [LineTotal] decimal(18,6) NOT NULL,
        [LineGrandTotal] decimal(18,6) NOT NULL,
        [Description] nvarchar(250) NULL,
        [ApprovalStatus] int NOT NULL,
        [PricingRuleHeaderId] bigint NULL,
        [RelatedStockId] bigint NULL,
        [RelatedProductKey] nvarchar(100) NULL,
        [IsMainRelatedProduct] bit NOT NULL DEFAULT CAST(0 AS bit),
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_DEMAND_LINE] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_DEMAND_LINE_RII_DEMAND_DemandId] FOREIGN KEY ([DemandId]) REFERENCES [RII_DEMAND] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RII_DEMAND_LINE_RII_PRICING_RULE_HEADER_PricingRuleHeaderId] FOREIGN KEY ([PricingRuleHeaderId]) REFERENCES [RII_PRICING_RULE_HEADER] ([Id]) ON DELETE SET NULL,
        CONSTRAINT [FK_RII_DEMAND_LINE_RII_STOCK_RelatedStockId] FOREIGN KEY ([RelatedStockId]) REFERENCES [RII_STOCK] ([Id]) ON DELETE SET NULL,
        CONSTRAINT [FK_RII_DEMAND_LINE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_DEMAND_LINE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_DEMAND_LINE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE TABLE [RII_ORDER_EXCHANGE_RATE] (
        [Id] bigint NOT NULL IDENTITY,
        [OrderId] bigint NOT NULL,
        [Currency] nvarchar(50) NOT NULL,
        [ExchangeRate] decimal(18,6) NOT NULL,
        [ExchangeRateDate] datetime2 NOT NULL,
        [IsOfficial] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_ORDER_EXCHANGE_RATE] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_ORDER_EXCHANGE_RATE_RII_ORDER_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [RII_ORDER] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RII_ORDER_EXCHANGE_RATE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_ORDER_EXCHANGE_RATE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_ORDER_EXCHANGE_RATE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE TABLE [RII_ORDER_LINE] (
        [Id] bigint NOT NULL IDENTITY,
        [OrderId] bigint NOT NULL,
        [ProductCode] nvarchar(100) NULL,
        [Quantity] decimal(18,6) NOT NULL,
        [UnitPrice] decimal(18,6) NOT NULL,
        [DiscountRate1] decimal(18,6) NOT NULL,
        [DiscountAmount1] decimal(18,6) NOT NULL,
        [DiscountRate2] decimal(18,6) NOT NULL,
        [DiscountAmount2] decimal(18,6) NOT NULL,
        [DiscountRate3] decimal(18,6) NOT NULL,
        [DiscountAmount3] decimal(18,6) NOT NULL,
        [VatRate] decimal(18,6) NOT NULL,
        [VatAmount] decimal(18,6) NOT NULL,
        [LineTotal] decimal(18,6) NOT NULL,
        [LineGrandTotal] decimal(18,6) NOT NULL,
        [Description] nvarchar(250) NULL,
        [ApprovalStatus] int NOT NULL,
        [PricingRuleHeaderId] bigint NULL,
        [RelatedStockId] bigint NULL,
        [RelatedProductKey] nvarchar(100) NULL,
        [IsMainRelatedProduct] bit NOT NULL DEFAULT CAST(0 AS bit),
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_ORDER_LINE] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_ORDER_LINE_RII_ORDER_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [RII_ORDER] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RII_ORDER_LINE_RII_PRICING_RULE_HEADER_PricingRuleHeaderId] FOREIGN KEY ([PricingRuleHeaderId]) REFERENCES [RII_PRICING_RULE_HEADER] ([Id]) ON DELETE SET NULL,
        CONSTRAINT [FK_RII_ORDER_LINE_RII_STOCK_RelatedStockId] FOREIGN KEY ([RelatedStockId]) REFERENCES [RII_STOCK] ([Id]) ON DELETE SET NULL,
        CONSTRAINT [FK_RII_ORDER_LINE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_ORDER_LINE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_ORDER_LINE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Demand_ActivityId] ON [RII_DEMAND] ([ActivityId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Demand_ApprovalDate] ON [RII_DEMAND] ([ApprovalDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Demand_ApprovalStatus] ON [RII_DEMAND] ([ApprovalStatus]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Demand_ApprovedByUserId] ON [RII_DEMAND] ([ApprovedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Demand_ContactId] ON [RII_DEMAND] ([ContactId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Demand_DeliveryDate] ON [RII_DEMAND] ([DeliveryDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Demand_DocumentSerialTypeId] ON [RII_DEMAND] ([DocumentSerialTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Demand_IsCompleted] ON [RII_DEMAND] ([IsCompleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Demand_IsDeleted] ON [RII_DEMAND] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Demand_OfferDate] ON [RII_DEMAND] ([OfferDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Demand_OfferNo] ON [RII_DEMAND] ([OfferNo]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Demand_PaymentTypeId] ON [RII_DEMAND] ([PaymentTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Demand_PotentialCustomerId] ON [RII_DEMAND] ([PotentialCustomerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Demand_RepresentativeId] ON [RII_DEMAND] ([RepresentativeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Demand_ShippingAddressId] ON [RII_DEMAND] ([ShippingAddressId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Demand_Status] ON [RII_DEMAND] ([Status]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Demand_ValidUntil] ON [RII_DEMAND] ([ValidUntil]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Demand_Year] ON [RII_DEMAND] ([Year]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_RII_DEMAND_CreatedBy] ON [RII_DEMAND] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_RII_DEMAND_DeletedBy] ON [RII_DEMAND] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_RII_DEMAND_UpdatedBy] ON [RII_DEMAND] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_DemandExchangeRate_Currency] ON [RII_DEMAND_EXCHANGE_RATE] ([Currency]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_DemandExchangeRate_DemandId] ON [RII_DEMAND_EXCHANGE_RATE] ([DemandId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_DemandExchangeRate_ExchangeRateDate] ON [RII_DEMAND_EXCHANGE_RATE] ([ExchangeRateDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_DemandExchangeRate_IsDeleted] ON [RII_DEMAND_EXCHANGE_RATE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_DemandExchangeRate_IsOfficial] ON [RII_DEMAND_EXCHANGE_RATE] ([IsOfficial]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_RII_DEMAND_EXCHANGE_RATE_CreatedBy] ON [RII_DEMAND_EXCHANGE_RATE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_RII_DEMAND_EXCHANGE_RATE_DeletedBy] ON [RII_DEMAND_EXCHANGE_RATE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_RII_DEMAND_EXCHANGE_RATE_UpdatedBy] ON [RII_DEMAND_EXCHANGE_RATE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_DemandLine_DemandId] ON [RII_DEMAND_LINE] ([DemandId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_DemandLine_IsDeleted] ON [RII_DEMAND_LINE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_DemandLine_PricingRuleHeaderId] ON [RII_DEMAND_LINE] ([PricingRuleHeaderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_DemandLine_ProductCode] ON [RII_DEMAND_LINE] ([ProductCode]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_DemandLine_RelatedStockId] ON [RII_DEMAND_LINE] ([RelatedStockId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_RII_DEMAND_LINE_CreatedBy] ON [RII_DEMAND_LINE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_RII_DEMAND_LINE_DeletedBy] ON [RII_DEMAND_LINE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_RII_DEMAND_LINE_UpdatedBy] ON [RII_DEMAND_LINE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Order_ActivityId] ON [RII_ORDER] ([ActivityId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Order_ApprovalDate] ON [RII_ORDER] ([ApprovalDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Order_ApprovalStatus] ON [RII_ORDER] ([ApprovalStatus]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Order_ApprovedByUserId] ON [RII_ORDER] ([ApprovedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Order_ContactId] ON [RII_ORDER] ([ContactId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Order_DeliveryDate] ON [RII_ORDER] ([DeliveryDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Order_DocumentSerialTypeId] ON [RII_ORDER] ([DocumentSerialTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Order_IsCompleted] ON [RII_ORDER] ([IsCompleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Order_IsDeleted] ON [RII_ORDER] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Order_OfferDate] ON [RII_ORDER] ([OfferDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Order_OfferNo] ON [RII_ORDER] ([OfferNo]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Order_PaymentTypeId] ON [RII_ORDER] ([PaymentTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Order_PotentialCustomerId] ON [RII_ORDER] ([PotentialCustomerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Order_RepresentativeId] ON [RII_ORDER] ([RepresentativeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Order_ShippingAddressId] ON [RII_ORDER] ([ShippingAddressId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Order_Status] ON [RII_ORDER] ([Status]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Order_ValidUntil] ON [RII_ORDER] ([ValidUntil]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_Order_Year] ON [RII_ORDER] ([Year]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_RII_ORDER_CreatedBy] ON [RII_ORDER] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_RII_ORDER_DeletedBy] ON [RII_ORDER] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_RII_ORDER_UpdatedBy] ON [RII_ORDER] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_OrderExchangeRate_Currency] ON [RII_ORDER_EXCHANGE_RATE] ([Currency]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_OrderExchangeRate_ExchangeRateDate] ON [RII_ORDER_EXCHANGE_RATE] ([ExchangeRateDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_OrderExchangeRate_IsDeleted] ON [RII_ORDER_EXCHANGE_RATE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_OrderExchangeRate_IsOfficial] ON [RII_ORDER_EXCHANGE_RATE] ([IsOfficial]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_OrderExchangeRate_OrderId] ON [RII_ORDER_EXCHANGE_RATE] ([OrderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_RII_ORDER_EXCHANGE_RATE_CreatedBy] ON [RII_ORDER_EXCHANGE_RATE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_RII_ORDER_EXCHANGE_RATE_DeletedBy] ON [RII_ORDER_EXCHANGE_RATE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_RII_ORDER_EXCHANGE_RATE_UpdatedBy] ON [RII_ORDER_EXCHANGE_RATE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_OrderLine_IsDeleted] ON [RII_ORDER_LINE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_OrderLine_OrderId] ON [RII_ORDER_LINE] ([OrderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_OrderLine_PricingRuleHeaderId] ON [RII_ORDER_LINE] ([PricingRuleHeaderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_OrderLine_ProductCode] ON [RII_ORDER_LINE] ([ProductCode]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_OrderLine_RelatedStockId] ON [RII_ORDER_LINE] ([RelatedStockId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_RII_ORDER_LINE_CreatedBy] ON [RII_ORDER_LINE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_RII_ORDER_LINE_DeletedBy] ON [RII_ORDER_LINE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    CREATE INDEX [IX_RII_ORDER_LINE_UpdatedBy] ON [RII_ORDER_LINE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127063315_NewMigration'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260127063315_NewMigration', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127151708_AddNotificationTable'
)
BEGIN
    CREATE TABLE [Notifications] (
        [Id] bigint NOT NULL IDENTITY,
        [TitleKey] nvarchar(200) NOT NULL,
        [TitleArgs] nvarchar(1000) NULL,
        [MessageKey] nvarchar(200) NOT NULL,
        [MessageArgs] nvarchar(2000) NULL,
        [IsRead] bit NOT NULL,
        [UserId] bigint NOT NULL,
        [RelatedEntityName] nvarchar(100) NULL,
        [RelatedEntityId] bigint NULL,
        [NotificationType] nvarchar(50) NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_Notifications] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Notifications_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_Notifications_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_Notifications_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_Notifications_RII_USERS_UserId] FOREIGN KEY ([UserId]) REFERENCES [RII_USERS] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127151708_AddNotificationTable'
)
BEGIN
    CREATE INDEX [IX_Notifications_CreatedBy] ON [Notifications] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127151708_AddNotificationTable'
)
BEGIN
    CREATE INDEX [IX_Notifications_DeletedBy] ON [Notifications] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127151708_AddNotificationTable'
)
BEGIN
    CREATE INDEX [IX_Notifications_IsRead] ON [Notifications] ([IsRead]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127151708_AddNotificationTable'
)
BEGIN
    CREATE INDEX [IX_Notifications_UpdatedBy] ON [Notifications] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127151708_AddNotificationTable'
)
BEGIN
    CREATE INDEX [IX_Notifications_UserId] ON [Notifications] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260127151708_AddNotificationTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260127151708_AddNotificationTable', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260131185316_AddDemandIdToQuotationAndQuotationIdToOrder'
)
BEGIN
    ALTER TABLE [RII_QUOTATION] ADD [DemandId] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260131185316_AddDemandIdToQuotationAndQuotationIdToOrder'
)
BEGIN
    CREATE INDEX [IX_Quotation_DemandId] ON [RII_QUOTATION] ([DemandId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260131185316_AddDemandIdToQuotationAndQuotationIdToOrder'
)
BEGIN
    ALTER TABLE [RII_QUOTATION] ADD CONSTRAINT [FK_RII_QUOTATION_RII_DEMAND_DemandId] FOREIGN KEY ([DemandId]) REFERENCES [RII_DEMAND] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260131185316_AddDemandIdToQuotationAndQuotationIdToOrder'
)
BEGIN
    ALTER TABLE [RII_ORDER] ADD [QuotationId] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260131185316_AddDemandIdToQuotationAndQuotationIdToOrder'
)
BEGIN
    CREATE INDEX [IX_Order_QuotationId] ON [RII_ORDER] ([QuotationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260131185316_AddDemandIdToQuotationAndQuotationIdToOrder'
)
BEGIN
    ALTER TABLE [RII_ORDER] ADD CONSTRAINT [FK_RII_ORDER_RII_QUOTATION_QuotationId] FOREIGN KEY ([QuotationId]) REFERENCES [RII_QUOTATION] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260131185316_AddDemandIdToQuotationAndQuotationIdToOrder'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260131185316_AddDemandIdToQuotationAndQuotationIdToOrder', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202194541_AddReportTemplateTable'
)
BEGIN
    CREATE TABLE [ReportTemplates] (
        [Id] bigint NOT NULL IDENTITY,
        [RuleType] int NOT NULL,
        [Title] nvarchar(200) NOT NULL,
        [TemplateJson] nvarchar(max) NOT NULL,
        [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedByUserId] bigint NULL,
        [UpdatedByUserId] bigint NULL,
        [CreatedDate] datetime2 NOT NULL,
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        [DeletedByUserId] bigint NULL,
        CONSTRAINT [PK_ReportTemplates] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ReportTemplates_RII_USERS_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_ReportTemplates_RII_USERS_DeletedByUserId] FOREIGN KEY ([DeletedByUserId]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_ReportTemplates_RII_USERS_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202194541_AddReportTemplateTable'
)
BEGIN
    CREATE INDEX [IX_ReportTemplates_CreatedByUserId] ON [ReportTemplates] ([CreatedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202194541_AddReportTemplateTable'
)
BEGIN
    CREATE INDEX [IX_ReportTemplates_DeletedByUserId] ON [ReportTemplates] ([DeletedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202194541_AddReportTemplateTable'
)
BEGIN
    CREATE INDEX [IX_ReportTemplates_IsActive] ON [ReportTemplates] ([IsActive]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202194541_AddReportTemplateTable'
)
BEGIN
    CREATE INDEX [IX_ReportTemplates_RuleType] ON [ReportTemplates] ([RuleType]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202194541_AddReportTemplateTable'
)
BEGIN
    CREATE INDEX [IX_ReportTemplates_RuleType_IsActive] ON [ReportTemplates] ([RuleType], [IsActive]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202194541_AddReportTemplateTable'
)
BEGIN
    CREATE INDEX [IX_ReportTemplates_UpdatedByUserId] ON [ReportTemplates] ([UpdatedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202194541_AddReportTemplateTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260202194541_AddReportTemplateTable', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202200000_AddDefaultAndRenameReportTemplatesToRII'
)
BEGIN
    EXEC sp_rename N'[ReportTemplates]', N'RII_REPORT_TEMPLATES';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202200000_AddDefaultAndRenameReportTemplatesToRII'
)
BEGIN
    DROP INDEX [IX_ReportTemplates_CreatedByUserId] ON [RII_REPORT_TEMPLATES];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202200000_AddDefaultAndRenameReportTemplatesToRII'
)
BEGIN
    DROP INDEX [IX_ReportTemplates_DeletedByUserId] ON [RII_REPORT_TEMPLATES];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202200000_AddDefaultAndRenameReportTemplatesToRII'
)
BEGIN
    DROP INDEX [IX_ReportTemplates_IsActive] ON [RII_REPORT_TEMPLATES];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202200000_AddDefaultAndRenameReportTemplatesToRII'
)
BEGIN
    DROP INDEX [IX_ReportTemplates_RuleType] ON [RII_REPORT_TEMPLATES];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202200000_AddDefaultAndRenameReportTemplatesToRII'
)
BEGIN
    DROP INDEX [IX_ReportTemplates_RuleType_IsActive] ON [RII_REPORT_TEMPLATES];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202200000_AddDefaultAndRenameReportTemplatesToRII'
)
BEGIN
    DROP INDEX [IX_ReportTemplates_UpdatedByUserId] ON [RII_REPORT_TEMPLATES];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202200000_AddDefaultAndRenameReportTemplatesToRII'
)
BEGIN
    CREATE INDEX [IX_RII_REPORT_TEMPLATES_CreatedByUserId] ON [RII_REPORT_TEMPLATES] ([CreatedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202200000_AddDefaultAndRenameReportTemplatesToRII'
)
BEGIN
    CREATE INDEX [IX_RII_REPORT_TEMPLATES_DeletedByUserId] ON [RII_REPORT_TEMPLATES] ([DeletedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202200000_AddDefaultAndRenameReportTemplatesToRII'
)
BEGIN
    CREATE INDEX [IX_RII_REPORT_TEMPLATES_IsActive] ON [RII_REPORT_TEMPLATES] ([IsActive]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202200000_AddDefaultAndRenameReportTemplatesToRII'
)
BEGIN
    CREATE INDEX [IX_RII_REPORT_TEMPLATES_RuleType] ON [RII_REPORT_TEMPLATES] ([RuleType]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202200000_AddDefaultAndRenameReportTemplatesToRII'
)
BEGIN
    CREATE INDEX [IX_RII_REPORT_TEMPLATES_RuleType_IsActive] ON [RII_REPORT_TEMPLATES] ([RuleType], [IsActive]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202200000_AddDefaultAndRenameReportTemplatesToRII'
)
BEGIN
    CREATE INDEX [IX_RII_REPORT_TEMPLATES_UpdatedByUserId] ON [RII_REPORT_TEMPLATES] ([UpdatedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202200000_AddDefaultAndRenameReportTemplatesToRII'
)
BEGIN
    ALTER TABLE [RII_REPORT_TEMPLATES] ADD [Default] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202200000_AddDefaultAndRenameReportTemplatesToRII'
)
BEGIN
    CREATE INDEX [IX_RII_REPORT_TEMPLATES_RuleType_Default] ON [RII_REPORT_TEMPLATES] ([RuleType], [Default]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202200000_AddDefaultAndRenameReportTemplatesToRII'
)
BEGIN

                    UPDATE RII_REPORT_TEMPLATES SET [Default] = 1
                    WHERE Id IN (
                        SELECT MIN(Id) FROM RII_REPORT_TEMPLATES WHERE IsDeleted = 0 GROUP BY RuleType
                    );
                
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260202200000_AddDefaultAndRenameReportTemplatesToRII'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260202200000_AddDefaultAndRenameReportTemplatesToRII', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260203152514_AddReportBuilderReportDefinitions'
)
BEGIN

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

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260203152514_AddReportBuilderReportDefinitions'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260203152514_AddReportBuilderReportDefinitions', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260204114846_AddSmtpSettingsTable'
)
BEGIN
    CREATE TABLE [SmtpSettings] (
        [Id] bigint NOT NULL IDENTITY,
        [Host] nvarchar(200) NOT NULL,
        [Port] int NOT NULL,
        [EnableSsl] bit NOT NULL,
        [Username] nvarchar(200) NOT NULL,
        [PasswordEncrypted] nvarchar(2000) NOT NULL,
        [FromEmail] nvarchar(200) NOT NULL,
        [FromName] nvarchar(200) NOT NULL,
        [Timeout] int NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [CreatedByUserId] bigint NULL,
        [UpdatedBy] bigint NULL,
        [UpdatedByUserId] bigint NULL,
        [DeletedBy] bigint NULL,
        [DeletedByUserId] bigint NULL,
        CONSTRAINT [PK_SmtpSettings] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SmtpSettings_RII_USERS_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_SmtpSettings_RII_USERS_DeletedByUserId] FOREIGN KEY ([DeletedByUserId]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_SmtpSettings_RII_USERS_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260204114846_AddSmtpSettingsTable'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedBy', N'CreatedByUserId', N'CreatedDate', N'DeletedBy', N'DeletedByUserId', N'DeletedDate', N'EnableSsl', N'FromEmail', N'FromName', N'Host', N'IsDeleted', N'PasswordEncrypted', N'Port', N'Timeout', N'UpdatedBy', N'UpdatedByUserId', N'UpdatedDate', N'Username') AND [object_id] = OBJECT_ID(N'[SmtpSettings]'))
        SET IDENTITY_INSERT [SmtpSettings] ON;
    EXEC(N'INSERT INTO [SmtpSettings] ([Id], [CreatedBy], [CreatedByUserId], [CreatedDate], [DeletedBy], [DeletedByUserId], [DeletedDate], [EnableSsl], [FromEmail], [FromName], [Host], [IsDeleted], [PasswordEncrypted], [Port], [Timeout], [UpdatedBy], [UpdatedByUserId], [UpdatedDate], [Username])
    VALUES (CAST(1 AS bigint), NULL, NULL, ''2026-02-04T11:48:43.5983942Z'', NULL, NULL, NULL, CAST(1 AS bit), N'''', N''V3RII CRM System'', N''smtp.gmail.com'', CAST(0 AS bit), N'''', 587, 30, NULL, NULL, NULL, N'''')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedBy', N'CreatedByUserId', N'CreatedDate', N'DeletedBy', N'DeletedByUserId', N'DeletedDate', N'EnableSsl', N'FromEmail', N'FromName', N'Host', N'IsDeleted', N'PasswordEncrypted', N'Port', N'Timeout', N'UpdatedBy', N'UpdatedByUserId', N'UpdatedDate', N'Username') AND [object_id] = OBJECT_ID(N'[SmtpSettings]'))
        SET IDENTITY_INSERT [SmtpSettings] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260204114846_AddSmtpSettingsTable'
)
BEGIN
    CREATE INDEX [IX_SmtpSettings_CreatedByUserId] ON [SmtpSettings] ([CreatedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260204114846_AddSmtpSettingsTable'
)
BEGIN
    CREATE INDEX [IX_SmtpSettings_DeletedByUserId] ON [SmtpSettings] ([DeletedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260204114846_AddSmtpSettingsTable'
)
BEGIN
    CREATE INDEX [IX_SmtpSettings_UpdatedByUserId] ON [SmtpSettings] ([UpdatedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260204114846_AddSmtpSettingsTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260204114846_AddSmtpSettingsTable', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260204115353_AddSmtpSettingsTableNameChanged'
)
BEGIN
    ALTER TABLE [SmtpSettings] DROP CONSTRAINT [FK_SmtpSettings_RII_USERS_CreatedByUserId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260204115353_AddSmtpSettingsTableNameChanged'
)
BEGIN
    ALTER TABLE [SmtpSettings] DROP CONSTRAINT [FK_SmtpSettings_RII_USERS_DeletedByUserId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260204115353_AddSmtpSettingsTableNameChanged'
)
BEGIN
    ALTER TABLE [SmtpSettings] DROP CONSTRAINT [FK_SmtpSettings_RII_USERS_UpdatedByUserId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260204115353_AddSmtpSettingsTableNameChanged'
)
BEGIN
    ALTER TABLE [SmtpSettings] DROP CONSTRAINT [PK_SmtpSettings];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260204115353_AddSmtpSettingsTableNameChanged'
)
BEGIN
    EXEC sp_rename N'[SmtpSettings]', N'RII_SMTP_SETTING';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260204115353_AddSmtpSettingsTableNameChanged'
)
BEGIN
    EXEC sp_rename N'[RII_SMTP_SETTING].[IX_SmtpSettings_UpdatedByUserId]', N'IX_RII_SMTP_SETTING_UpdatedByUserId', N'INDEX';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260204115353_AddSmtpSettingsTableNameChanged'
)
BEGIN
    EXEC sp_rename N'[RII_SMTP_SETTING].[IX_SmtpSettings_DeletedByUserId]', N'IX_RII_SMTP_SETTING_DeletedByUserId', N'INDEX';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260204115353_AddSmtpSettingsTableNameChanged'
)
BEGIN
    EXEC sp_rename N'[RII_SMTP_SETTING].[IX_SmtpSettings_CreatedByUserId]', N'IX_RII_SMTP_SETTING_CreatedByUserId', N'INDEX';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260204115353_AddSmtpSettingsTableNameChanged'
)
BEGIN
    ALTER TABLE [RII_SMTP_SETTING] ADD CONSTRAINT [PK_RII_SMTP_SETTING] PRIMARY KEY ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260204115353_AddSmtpSettingsTableNameChanged'
)
BEGIN
    EXEC(N'UPDATE [RII_SMTP_SETTING] SET [CreatedDate] = ''2026-02-04T11:53:50.5118839Z''
    WHERE [Id] = CAST(1 AS bigint);
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260204115353_AddSmtpSettingsTableNameChanged'
)
BEGIN
    ALTER TABLE [RII_SMTP_SETTING] ADD CONSTRAINT [FK_RII_SMTP_SETTING_RII_USERS_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260204115353_AddSmtpSettingsTableNameChanged'
)
BEGIN
    ALTER TABLE [RII_SMTP_SETTING] ADD CONSTRAINT [FK_RII_SMTP_SETTING_RII_USERS_DeletedByUserId] FOREIGN KEY ([DeletedByUserId]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260204115353_AddSmtpSettingsTableNameChanged'
)
BEGIN
    ALTER TABLE [RII_SMTP_SETTING] ADD CONSTRAINT [FK_RII_SMTP_SETTING_RII_USERS_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [RII_USERS] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260204115353_AddSmtpSettingsTableNameChanged'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260204115353_AddSmtpSettingsTableNameChanged', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE TABLE [RII_POWERBI_GROUPS] (
        [Id] bigint NOT NULL IDENTITY,
        [Name] nvarchar(150) NOT NULL,
        [Description] nvarchar(500) NULL,
        [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_POWERBI_GROUPS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_POWERBI_GROUPS_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_POWERBI_GROUPS_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_POWERBI_GROUPS_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE TABLE [RII_POWERBI_REPORT_DEFINITIONS] (
        [Id] bigint NOT NULL IDENTITY,
        [Name] nvarchar(200) NOT NULL,
        [Description] nvarchar(1000) NULL,
        [WorkspaceId] uniqueidentifier NOT NULL,
        [ReportId] uniqueidentifier NOT NULL,
        [DatasetId] uniqueidentifier NULL,
        [EmbedUrl] nvarchar(500) NULL,
        [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
        [RlsRoles] nvarchar(max) NULL,
        [AllowedUserIds] nvarchar(max) NULL,
        [AllowedRoleIds] nvarchar(max) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_POWERBI_REPORT_DEFINITIONS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_POWERBI_REPORT_DEFINITIONS_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_POWERBI_REPORT_DEFINITIONS_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_POWERBI_REPORT_DEFINITIONS_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE TABLE [RII_POWERBI_GROUP_REPORT_DEFINITIONS] (
        [Id] bigint NOT NULL IDENTITY,
        [GroupId] bigint NOT NULL,
        [ReportDefinitionId] bigint NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_POWERBI_GROUP_REPORT_DEFINITIONS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_POWERBI_GROUP_REPORT_DEFINITIONS_RII_POWERBI_GROUPS_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [RII_POWERBI_GROUPS] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RII_POWERBI_GROUP_REPORT_DEFINITIONS_RII_POWERBI_REPORT_DEFINITIONS_ReportDefinitionId] FOREIGN KEY ([ReportDefinitionId]) REFERENCES [RII_POWERBI_REPORT_DEFINITIONS] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RII_POWERBI_GROUP_REPORT_DEFINITIONS_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_POWERBI_GROUP_REPORT_DEFINITIONS_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_POWERBI_GROUP_REPORT_DEFINITIONS_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE TABLE [RII_USER_POWERBI_GROUPS] (
        [Id] bigint NOT NULL IDENTITY,
        [UserId] bigint NOT NULL,
        [GroupId] bigint NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_USER_POWERBI_GROUPS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_USER_POWERBI_GROUPS_RII_POWERBI_GROUPS_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [RII_POWERBI_GROUPS] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RII_USER_POWERBI_GROUPS_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_USER_POWERBI_GROUPS_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_USER_POWERBI_GROUPS_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_USER_POWERBI_GROUPS_RII_USERS_UserId] FOREIGN KEY ([UserId]) REFERENCES [RII_USERS] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE INDEX [IX_RII_POWERBI_GROUPS_CreatedBy] ON [RII_POWERBI_GROUPS] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE INDEX [IX_RII_POWERBI_GROUPS_DeletedBy] ON [RII_POWERBI_GROUPS] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE INDEX [IX_RII_POWERBI_GROUPS_IsDeleted] ON [RII_POWERBI_GROUPS] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE UNIQUE INDEX [UX_PowerBIGroups_Name] ON [RII_POWERBI_GROUPS] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE INDEX [IX_RII_POWERBI_GROUPS_UpdatedBy] ON [RII_POWERBI_GROUPS] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE INDEX [IX_RII_POWERBI_REPORT_DEFINITIONS_CreatedBy] ON [RII_POWERBI_REPORT_DEFINITIONS] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE INDEX [IX_RII_POWERBI_REPORT_DEFINITIONS_DeletedBy] ON [RII_POWERBI_REPORT_DEFINITIONS] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE INDEX [IX_PowerBIReportDefinitions_IsDeleted] ON [RII_POWERBI_REPORT_DEFINITIONS] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE INDEX [IX_PowerBIReportDefinitions_Name] ON [RII_POWERBI_REPORT_DEFINITIONS] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE INDEX [IX_RII_POWERBI_REPORT_DEFINITIONS_UpdatedBy] ON [RII_POWERBI_REPORT_DEFINITIONS] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE UNIQUE INDEX [UX_PowerBIReportDefinitions_Workspace_Report] ON [RII_POWERBI_REPORT_DEFINITIONS] ([WorkspaceId], [ReportId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE INDEX [IX_PowerBIGroupReportDefinitions_IsDeleted] ON [RII_POWERBI_GROUP_REPORT_DEFINITIONS] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE INDEX [IX_RII_POWERBI_GROUP_REPORT_DEFINITIONS_CreatedBy] ON [RII_POWERBI_GROUP_REPORT_DEFINITIONS] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE INDEX [IX_RII_POWERBI_GROUP_REPORT_DEFINITIONS_DeletedBy] ON [RII_POWERBI_GROUP_REPORT_DEFINITIONS] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE INDEX [IX_RII_POWERBI_GROUP_REPORT_DEFINITIONS_ReportDefinitionId] ON [RII_POWERBI_GROUP_REPORT_DEFINITIONS] ([ReportDefinitionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE INDEX [IX_RII_POWERBI_GROUP_REPORT_DEFINITIONS_UpdatedBy] ON [RII_POWERBI_GROUP_REPORT_DEFINITIONS] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE UNIQUE INDEX [UX_PowerBIGroupReportDefinitions_Group_Report] ON [RII_POWERBI_GROUP_REPORT_DEFINITIONS] ([GroupId], [ReportDefinitionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE INDEX [IX_RII_USER_POWERBI_GROUPS_CreatedBy] ON [RII_USER_POWERBI_GROUPS] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE INDEX [IX_RII_USER_POWERBI_GROUPS_DeletedBy] ON [RII_USER_POWERBI_GROUPS] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE INDEX [IX_RII_USER_POWERBI_GROUPS_GroupId] ON [RII_USER_POWERBI_GROUPS] ([GroupId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE INDEX [IX_UserPowerBIGroups_IsDeleted] ON [RII_USER_POWERBI_GROUPS] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE INDEX [IX_RII_USER_POWERBI_GROUPS_UpdatedBy] ON [RII_USER_POWERBI_GROUPS] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    CREATE UNIQUE INDEX [UX_UserPowerBIGroups_User_Group] ON [RII_USER_POWERBI_GROUPS] ([UserId], [GroupId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205103353_PowerBIReportDefinitionInitial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260205103353_PowerBIReportDefinitionInitial', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205123350_AddPowerBIConfigurationTable'
)
BEGIN
    CREATE TABLE [RII_POWERBI_CONFIGURATION] (
        [Id] bigint NOT NULL IDENTITY,
        [TenantId] nvarchar(100) NOT NULL,
        [ClientId] nvarchar(100) NOT NULL,
        [WorkspaceId] uniqueidentifier NOT NULL,
        [ApiBaseUrl] nvarchar(200) NULL,
        [Scope] nvarchar(200) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_POWERBI_CONFIGURATION] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_POWERBI_CONFIGURATION_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_POWERBI_CONFIGURATION_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_POWERBI_CONFIGURATION_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205123350_AddPowerBIConfigurationTable'
)
BEGIN
    EXEC(N'UPDATE [RII_SMTP_SETTING] SET [CreatedDate] = ''2026-02-05T12:33:49.8093440Z''
    WHERE [Id] = CAST(1 AS bigint);
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205123350_AddPowerBIConfigurationTable'
)
BEGIN
    CREATE INDEX [IX_PowerBIConfiguration_IsDeleted] ON [RII_POWERBI_CONFIGURATION] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205123350_AddPowerBIConfigurationTable'
)
BEGIN
    CREATE INDEX [IX_RII_POWERBI_CONFIGURATION_CreatedBy] ON [RII_POWERBI_CONFIGURATION] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205123350_AddPowerBIConfigurationTable'
)
BEGIN
    CREATE INDEX [IX_RII_POWERBI_CONFIGURATION_DeletedBy] ON [RII_POWERBI_CONFIGURATION] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205123350_AddPowerBIConfigurationTable'
)
BEGIN
    CREATE INDEX [IX_RII_POWERBI_CONFIGURATION_UpdatedBy] ON [RII_POWERBI_CONFIGURATION] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205123350_AddPowerBIConfigurationTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260205123350_AddPowerBIConfigurationTable', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205150824_AddPowerBIReportRoleMappings'
)
BEGIN
    CREATE TABLE [RII_POWERBI_REPORT_ROLE_MAPPINGS] (
        [Id] bigint NOT NULL IDENTITY,
        [PowerBIReportDefinitionId] bigint NOT NULL,
        [RoleId] bigint NOT NULL,
        [RlsRoles] nvarchar(max) NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_POWERBI_REPORT_ROLE_MAPPINGS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_POWERBI_REPORT_ROLE_MAPPINGS_RII_POWERBI_REPORT_DEFINITIONS_PowerBIReportDefinitionId] FOREIGN KEY ([PowerBIReportDefinitionId]) REFERENCES [RII_POWERBI_REPORT_DEFINITIONS] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RII_POWERBI_REPORT_ROLE_MAPPINGS_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_POWERBI_REPORT_ROLE_MAPPINGS_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_POWERBI_REPORT_ROLE_MAPPINGS_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_POWERBI_REPORT_ROLE_MAPPINGS_RII_USER_AUTHORITY_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [RII_USER_AUTHORITY] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205150824_AddPowerBIReportRoleMappings'
)
BEGIN
    EXEC(N'UPDATE [RII_SMTP_SETTING] SET [CreatedDate] = ''2026-02-05T15:08:23.6350750Z''
    WHERE [Id] = CAST(1 AS bigint);
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205150824_AddPowerBIReportRoleMappings'
)
BEGIN
    CREATE INDEX [IX_PowerBIReportRoleMappings_IsDeleted] ON [RII_POWERBI_REPORT_ROLE_MAPPINGS] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205150824_AddPowerBIReportRoleMappings'
)
BEGIN
    CREATE INDEX [IX_RII_POWERBI_REPORT_ROLE_MAPPINGS_CreatedBy] ON [RII_POWERBI_REPORT_ROLE_MAPPINGS] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205150824_AddPowerBIReportRoleMappings'
)
BEGIN
    CREATE INDEX [IX_RII_POWERBI_REPORT_ROLE_MAPPINGS_DeletedBy] ON [RII_POWERBI_REPORT_ROLE_MAPPINGS] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205150824_AddPowerBIReportRoleMappings'
)
BEGIN
    CREATE INDEX [IX_RII_POWERBI_REPORT_ROLE_MAPPINGS_RoleId] ON [RII_POWERBI_REPORT_ROLE_MAPPINGS] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205150824_AddPowerBIReportRoleMappings'
)
BEGIN
    CREATE INDEX [IX_RII_POWERBI_REPORT_ROLE_MAPPINGS_UpdatedBy] ON [RII_POWERBI_REPORT_ROLE_MAPPINGS] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205150824_AddPowerBIReportRoleMappings'
)
BEGIN
    CREATE UNIQUE INDEX [UX_PowerBIReportRoleMappings_Report_Role] ON [RII_POWERBI_REPORT_ROLE_MAPPINGS] ([PowerBIReportDefinitionId], [RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205150824_AddPowerBIReportRoleMappings'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260205150824_AddPowerBIReportRoleMappings', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE TABLE [RII_PERMISSION_DEFINITIONS] (
        [Id] bigint NOT NULL IDENTITY,
        [Code] nvarchar(120) NOT NULL,
        [Name] nvarchar(150) NOT NULL,
        [Description] nvarchar(500) NULL,
        [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_PERMISSION_DEFINITIONS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_PERMISSION_DEFINITIONS_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_PERMISSION_DEFINITIONS_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_PERMISSION_DEFINITIONS_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE TABLE [RII_PERMISSION_GROUPS] (
        [Id] bigint NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Description] nvarchar(500) NULL,
        [IsSystemAdmin] bit NOT NULL DEFAULT CAST(0 AS bit),
        [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_PERMISSION_GROUPS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_PERMISSION_GROUPS_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_PERMISSION_GROUPS_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_PERMISSION_GROUPS_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE TABLE [RII_PERMISSION_GROUP_PERMISSIONS] (
        [Id] bigint NOT NULL IDENTITY,
        [PermissionGroupId] bigint NOT NULL,
        [PermissionDefinitionId] bigint NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_PERMISSION_GROUP_PERMISSIONS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_PERMISSION_GROUP_PERMISSIONS_RII_PERMISSION_DEFINITIONS_PermissionDefinitionId] FOREIGN KEY ([PermissionDefinitionId]) REFERENCES [RII_PERMISSION_DEFINITIONS] ([Id]),
        CONSTRAINT [FK_RII_PERMISSION_GROUP_PERMISSIONS_RII_PERMISSION_GROUPS_PermissionGroupId] FOREIGN KEY ([PermissionGroupId]) REFERENCES [RII_PERMISSION_GROUPS] ([Id]),
        CONSTRAINT [FK_RII_PERMISSION_GROUP_PERMISSIONS_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_PERMISSION_GROUP_PERMISSIONS_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_PERMISSION_GROUP_PERMISSIONS_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE TABLE [RII_USER_PERMISSION_GROUPS] (
        [Id] bigint NOT NULL IDENTITY,
        [UserId] bigint NOT NULL,
        [PermissionGroupId] bigint NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_USER_PERMISSION_GROUPS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_USER_PERMISSION_GROUPS_RII_PERMISSION_GROUPS_PermissionGroupId] FOREIGN KEY ([PermissionGroupId]) REFERENCES [RII_PERMISSION_GROUPS] ([Id]),
        CONSTRAINT [FK_RII_USER_PERMISSION_GROUPS_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_USER_PERMISSION_GROUPS_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_USER_PERMISSION_GROUPS_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_USER_PERMISSION_GROUPS_RII_USERS_UserId] FOREIGN KEY ([UserId]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'CreatedBy', N'CreatedDate', N'DeletedBy', N'DeletedDate', N'Description', N'IsActive', N'IsDeleted', N'Name', N'UpdatedBy', N'UpdatedDate') AND [object_id] = OBJECT_ID(N'[RII_PERMISSION_DEFINITIONS]'))
        SET IDENTITY_INSERT [RII_PERMISSION_DEFINITIONS] ON;
    EXEC(N'INSERT INTO [RII_PERMISSION_DEFINITIONS] ([Id], [Code], [CreatedBy], [CreatedDate], [DeletedBy], [DeletedDate], [Description], [IsActive], [IsDeleted], [Name], [UpdatedBy], [UpdatedDate])
    VALUES (CAST(1 AS bigint), N''dashboard.view'', NULL, ''2024-01-01T00:00:00.0000000Z'', NULL, NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), N''Dashboard View'', NULL, NULL),
    (CAST(2 AS bigint), N''customers.view'', NULL, ''2024-01-01T00:00:00.0000000Z'', NULL, NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), N''Customers View'', NULL, NULL),
    (CAST(3 AS bigint), N''salesmen360.view'', NULL, ''2024-01-01T00:00:00.0000000Z'', NULL, NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), N''Salesmen 360 View'', NULL, NULL),
    (CAST(4 AS bigint), N''customer360.view'', NULL, ''2024-01-01T00:00:00.0000000Z'', NULL, NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), N''Customer 360 View'', NULL, NULL),
    (CAST(5 AS bigint), N''powerbi.view'', NULL, ''2024-01-01T00:00:00.0000000Z'', NULL, NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), N''Power BI View'', NULL, NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'CreatedBy', N'CreatedDate', N'DeletedBy', N'DeletedDate', N'Description', N'IsActive', N'IsDeleted', N'Name', N'UpdatedBy', N'UpdatedDate') AND [object_id] = OBJECT_ID(N'[RII_PERMISSION_DEFINITIONS]'))
        SET IDENTITY_INSERT [RII_PERMISSION_DEFINITIONS] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedBy', N'CreatedDate', N'DeletedBy', N'DeletedDate', N'Description', N'IsActive', N'IsDeleted', N'IsSystemAdmin', N'Name', N'UpdatedBy', N'UpdatedDate') AND [object_id] = OBJECT_ID(N'[RII_PERMISSION_GROUPS]'))
        SET IDENTITY_INSERT [RII_PERMISSION_GROUPS] ON;
    EXEC(N'INSERT INTO [RII_PERMISSION_GROUPS] ([Id], [CreatedBy], [CreatedDate], [DeletedBy], [DeletedDate], [Description], [IsActive], [IsDeleted], [IsSystemAdmin], [Name], [UpdatedBy], [UpdatedDate])
    VALUES (CAST(1 AS bigint), NULL, ''2024-01-01T00:00:00.0000000Z'', NULL, NULL, N''Full system access'', CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), N''System Admin'', NULL, NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedBy', N'CreatedDate', N'DeletedBy', N'DeletedDate', N'Description', N'IsActive', N'IsDeleted', N'IsSystemAdmin', N'Name', N'UpdatedBy', N'UpdatedDate') AND [object_id] = OBJECT_ID(N'[RII_PERMISSION_GROUPS]'))
        SET IDENTITY_INSERT [RII_PERMISSION_GROUPS] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedBy', N'CreatedDate', N'DeletedBy', N'DeletedDate', N'IsDeleted', N'PermissionGroupId', N'UpdatedBy', N'UpdatedDate', N'UserId') AND [object_id] = OBJECT_ID(N'[RII_USER_PERMISSION_GROUPS]'))
        SET IDENTITY_INSERT [RII_USER_PERMISSION_GROUPS] ON;
    EXEC(N'INSERT INTO [RII_USER_PERMISSION_GROUPS] ([Id], [CreatedBy], [CreatedDate], [DeletedBy], [DeletedDate], [IsDeleted], [PermissionGroupId], [UpdatedBy], [UpdatedDate], [UserId])
    VALUES (CAST(1 AS bigint), NULL, ''2024-01-01T00:00:00.0000000Z'', NULL, NULL, CAST(0 AS bit), CAST(1 AS bigint), NULL, NULL, CAST(1 AS bigint))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedBy', N'CreatedDate', N'DeletedBy', N'DeletedDate', N'IsDeleted', N'PermissionGroupId', N'UpdatedBy', N'UpdatedDate', N'UserId') AND [object_id] = OBJECT_ID(N'[RII_USER_PERMISSION_GROUPS]'))
        SET IDENTITY_INSERT [RII_USER_PERMISSION_GROUPS] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE UNIQUE INDEX [IX_PermissionDefinitions_Code] ON [RII_PERMISSION_DEFINITIONS] ([Code]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE INDEX [IX_PermissionDefinitions_IsDeleted] ON [RII_PERMISSION_DEFINITIONS] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE INDEX [IX_RII_PERMISSION_DEFINITIONS_CreatedBy] ON [RII_PERMISSION_DEFINITIONS] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE INDEX [IX_RII_PERMISSION_DEFINITIONS_DeletedBy] ON [RII_PERMISSION_DEFINITIONS] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE INDEX [IX_RII_PERMISSION_DEFINITIONS_UpdatedBy] ON [RII_PERMISSION_DEFINITIONS] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE UNIQUE INDEX [IX_PermissionGroupPermission_GroupId_DefinitionId] ON [RII_PERMISSION_GROUP_PERMISSIONS] ([PermissionGroupId], [PermissionDefinitionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE INDEX [IX_PermissionGroupPermission_IsDeleted] ON [RII_PERMISSION_GROUP_PERMISSIONS] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE INDEX [IX_RII_PERMISSION_GROUP_PERMISSIONS_CreatedBy] ON [RII_PERMISSION_GROUP_PERMISSIONS] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE INDEX [IX_RII_PERMISSION_GROUP_PERMISSIONS_DeletedBy] ON [RII_PERMISSION_GROUP_PERMISSIONS] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE INDEX [IX_RII_PERMISSION_GROUP_PERMISSIONS_PermissionDefinitionId] ON [RII_PERMISSION_GROUP_PERMISSIONS] ([PermissionDefinitionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE INDEX [IX_RII_PERMISSION_GROUP_PERMISSIONS_UpdatedBy] ON [RII_PERMISSION_GROUP_PERMISSIONS] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE INDEX [IX_PermissionGroups_IsDeleted] ON [RII_PERMISSION_GROUPS] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE UNIQUE INDEX [IX_PermissionGroups_Name] ON [RII_PERMISSION_GROUPS] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE INDEX [IX_RII_PERMISSION_GROUPS_CreatedBy] ON [RII_PERMISSION_GROUPS] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE INDEX [IX_RII_PERMISSION_GROUPS_DeletedBy] ON [RII_PERMISSION_GROUPS] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE INDEX [IX_RII_PERMISSION_GROUPS_UpdatedBy] ON [RII_PERMISSION_GROUPS] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE INDEX [IX_RII_USER_PERMISSION_GROUPS_CreatedBy] ON [RII_USER_PERMISSION_GROUPS] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE INDEX [IX_RII_USER_PERMISSION_GROUPS_DeletedBy] ON [RII_USER_PERMISSION_GROUPS] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE INDEX [IX_RII_USER_PERMISSION_GROUPS_PermissionGroupId] ON [RII_USER_PERMISSION_GROUPS] ([PermissionGroupId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE INDEX [IX_RII_USER_PERMISSION_GROUPS_UpdatedBy] ON [RII_USER_PERMISSION_GROUPS] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE INDEX [IX_UserPermissionGroup_IsDeleted] ON [RII_USER_PERMISSION_GROUPS] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    CREATE UNIQUE INDEX [IX_UserPermissionGroup_UserId_GroupId] ON [RII_USER_PERMISSION_GROUPS] ([UserId], [PermissionGroupId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260207124433_AddDynamicPermissionModel'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260207124433_AddDynamicPermissionModel', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260209144010_AddCustomerSalutationType'
)
BEGIN

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
                
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260209144010_AddCustomerSalutationType'
)
BEGIN

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
                
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260209144010_AddCustomerSalutationType'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_CONTACT]') AND [c].[name] = N'TitleId');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [RII_CONTACT] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [RII_CONTACT] ALTER COLUMN [TitleId] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260209144010_AddCustomerSalutationType'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_CONTACT]') AND [c].[name] = N'FullName');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [RII_CONTACT] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [RII_CONTACT] ALTER COLUMN [FullName] nvarchar(250) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260209144010_AddCustomerSalutationType'
)
BEGIN

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
                
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260209144010_AddCustomerSalutationType'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260209144010_AddCustomerSalutationType', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260209181155_AddActivityReminderSupport'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260209181155_AddActivityReminderSupport', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260209192739_AlignActivitySchemaForReminderModel'
)
BEGIN

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

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260209192739_AlignActivitySchemaForReminderModel'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260209192739_AlignActivitySchemaForReminderModel', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260210112406_AddGeneralDiscountFieldsToQuotation'
)
BEGIN
    ALTER TABLE [RII_QUOTATION] ADD [GeneralDiscountAmount] decimal(18,6) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260210112406_AddGeneralDiscountFieldsToQuotation'
)
BEGIN
    ALTER TABLE [RII_QUOTATION] ADD [GeneralDiscountRate] decimal(18,6) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260210112406_AddGeneralDiscountFieldsToQuotation'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260210112406_AddGeneralDiscountFieldsToQuotation', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260210115542_AddGeneralDiscountFieldsToDemandAndOrder'
)
BEGIN
    ALTER TABLE [RII_ORDER] ADD [GeneralDiscountAmount] decimal(18,6) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260210115542_AddGeneralDiscountFieldsToDemandAndOrder'
)
BEGIN
    ALTER TABLE [RII_ORDER] ADD [GeneralDiscountRate] decimal(18,6) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260210115542_AddGeneralDiscountFieldsToDemandAndOrder'
)
BEGIN
    ALTER TABLE [RII_DEMAND] ADD [GeneralDiscountAmount] decimal(18,6) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260210115542_AddGeneralDiscountFieldsToDemandAndOrder'
)
BEGIN
    ALTER TABLE [RII_DEMAND] ADD [GeneralDiscountRate] decimal(18,6) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260210115542_AddGeneralDiscountFieldsToDemandAndOrder'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260210115542_AddGeneralDiscountFieldsToDemandAndOrder', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211060130_AddQuotationNotes'
)
BEGIN
    CREATE TABLE [RII_QUOTATION_NOTES] (
        [Id] bigint NOT NULL IDENTITY,
        [QuotationId] bigint NOT NULL,
        [Note1] nvarchar(100) NULL,
        [Note2] nvarchar(100) NULL,
        [Note3] nvarchar(100) NULL,
        [Note4] nvarchar(100) NULL,
        [Note5] nvarchar(100) NULL,
        [Note6] nvarchar(100) NULL,
        [Note7] nvarchar(100) NULL,
        [Note8] nvarchar(100) NULL,
        [Note9] nvarchar(100) NULL,
        [Note10] nvarchar(100) NULL,
        [Note11] nvarchar(100) NULL,
        [Note12] nvarchar(100) NULL,
        [Note13] nvarchar(100) NULL,
        [Note14] nvarchar(100) NULL,
        [Note15] nvarchar(100) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_QUOTATION_NOTES] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_QUOTATION_NOTES_RII_QUOTATION_QuotationId] FOREIGN KEY ([QuotationId]) REFERENCES [RII_QUOTATION] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RII_QUOTATION_NOTES_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_QUOTATION_NOTES_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_QUOTATION_NOTES_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211060130_AddQuotationNotes'
)
BEGIN
    CREATE INDEX [IX_QuotationNotes_IsDeleted] ON [RII_QUOTATION_NOTES] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211060130_AddQuotationNotes'
)
BEGIN
    CREATE UNIQUE INDEX [IX_QuotationNotes_QuotationId] ON [RII_QUOTATION_NOTES] ([QuotationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211060130_AddQuotationNotes'
)
BEGIN
    CREATE INDEX [IX_RII_QUOTATION_NOTES_CreatedBy] ON [RII_QUOTATION_NOTES] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211060130_AddQuotationNotes'
)
BEGIN
    CREATE INDEX [IX_RII_QUOTATION_NOTES_DeletedBy] ON [RII_QUOTATION_NOTES] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211060130_AddQuotationNotes'
)
BEGIN
    CREATE INDEX [IX_RII_QUOTATION_NOTES_UpdatedBy] ON [RII_QUOTATION_NOTES] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211060130_AddQuotationNotes'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260211060130_AddQuotationNotes', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211071308_AddDemandAndOrderNotes'
)
BEGIN
    CREATE TABLE [RII_DEMAND_NOTES] (
        [Id] bigint NOT NULL IDENTITY,
        [DemandId] bigint NOT NULL,
        [Note1] nvarchar(100) NULL,
        [Note2] nvarchar(100) NULL,
        [Note3] nvarchar(100) NULL,
        [Note4] nvarchar(100) NULL,
        [Note5] nvarchar(100) NULL,
        [Note6] nvarchar(100) NULL,
        [Note7] nvarchar(100) NULL,
        [Note8] nvarchar(100) NULL,
        [Note9] nvarchar(100) NULL,
        [Note10] nvarchar(100) NULL,
        [Note11] nvarchar(100) NULL,
        [Note12] nvarchar(100) NULL,
        [Note13] nvarchar(100) NULL,
        [Note14] nvarchar(100) NULL,
        [Note15] nvarchar(100) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_DEMAND_NOTES] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_DEMAND_NOTES_RII_DEMAND_DemandId] FOREIGN KEY ([DemandId]) REFERENCES [RII_DEMAND] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RII_DEMAND_NOTES_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_DEMAND_NOTES_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_DEMAND_NOTES_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211071308_AddDemandAndOrderNotes'
)
BEGIN
    CREATE TABLE [RII_ORDER_NOTES] (
        [Id] bigint NOT NULL IDENTITY,
        [OrderId] bigint NOT NULL,
        [Note1] nvarchar(100) NULL,
        [Note2] nvarchar(100) NULL,
        [Note3] nvarchar(100) NULL,
        [Note4] nvarchar(100) NULL,
        [Note5] nvarchar(100) NULL,
        [Note6] nvarchar(100) NULL,
        [Note7] nvarchar(100) NULL,
        [Note8] nvarchar(100) NULL,
        [Note9] nvarchar(100) NULL,
        [Note10] nvarchar(100) NULL,
        [Note11] nvarchar(100) NULL,
        [Note12] nvarchar(100) NULL,
        [Note13] nvarchar(100) NULL,
        [Note14] nvarchar(100) NULL,
        [Note15] nvarchar(100) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_ORDER_NOTES] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_ORDER_NOTES_RII_ORDER_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [RII_ORDER] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RII_ORDER_NOTES_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_ORDER_NOTES_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_ORDER_NOTES_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211071308_AddDemandAndOrderNotes'
)
BEGIN
    CREATE UNIQUE INDEX [IX_DemandNotes_DemandId] ON [RII_DEMAND_NOTES] ([DemandId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211071308_AddDemandAndOrderNotes'
)
BEGIN
    CREATE INDEX [IX_DemandNotes_IsDeleted] ON [RII_DEMAND_NOTES] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211071308_AddDemandAndOrderNotes'
)
BEGIN
    CREATE INDEX [IX_RII_DEMAND_NOTES_CreatedBy] ON [RII_DEMAND_NOTES] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211071308_AddDemandAndOrderNotes'
)
BEGIN
    CREATE INDEX [IX_RII_DEMAND_NOTES_DeletedBy] ON [RII_DEMAND_NOTES] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211071308_AddDemandAndOrderNotes'
)
BEGIN
    CREATE INDEX [IX_RII_DEMAND_NOTES_UpdatedBy] ON [RII_DEMAND_NOTES] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211071308_AddDemandAndOrderNotes'
)
BEGIN
    CREATE INDEX [IX_OrderNotes_IsDeleted] ON [RII_ORDER_NOTES] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211071308_AddDemandAndOrderNotes'
)
BEGIN
    CREATE UNIQUE INDEX [IX_OrderNotes_OrderId] ON [RII_ORDER_NOTES] ([OrderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211071308_AddDemandAndOrderNotes'
)
BEGIN
    CREATE INDEX [IX_RII_ORDER_NOTES_CreatedBy] ON [RII_ORDER_NOTES] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211071308_AddDemandAndOrderNotes'
)
BEGIN
    CREATE INDEX [IX_RII_ORDER_NOTES_DeletedBy] ON [RII_ORDER_NOTES] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211071308_AddDemandAndOrderNotes'
)
BEGIN
    CREATE INDEX [IX_RII_ORDER_NOTES_UpdatedBy] ON [RII_ORDER_NOTES] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211071308_AddDemandAndOrderNotes'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260211071308_AddDemandAndOrderNotes', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211082723_AddSalesTypeDefinition'
)
BEGIN
    CREATE TABLE [RII_SALES_TYPE_DEFINITION] (
        [Id] bigint NOT NULL IDENTITY,
        [SalesType] nvarchar(20) NOT NULL,
        [Name] nvarchar(150) NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_SALES_TYPE_DEFINITION] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_SALES_TYPE_DEFINITION_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_SALES_TYPE_DEFINITION_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_SALES_TYPE_DEFINITION_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211082723_AddSalesTypeDefinition'
)
BEGIN
    CREATE INDEX [IX_RII_SALES_TYPE_DEFINITION_CreatedBy] ON [RII_SALES_TYPE_DEFINITION] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211082723_AddSalesTypeDefinition'
)
BEGIN
    CREATE INDEX [IX_RII_SALES_TYPE_DEFINITION_DeletedBy] ON [RII_SALES_TYPE_DEFINITION] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211082723_AddSalesTypeDefinition'
)
BEGIN
    CREATE INDEX [IX_RII_SALES_TYPE_DEFINITION_UpdatedBy] ON [RII_SALES_TYPE_DEFINITION] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211082723_AddSalesTypeDefinition'
)
BEGIN
    CREATE INDEX [IX_SalesTypeDefinition_IsDeleted] ON [RII_SALES_TYPE_DEFINITION] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211082723_AddSalesTypeDefinition'
)
BEGIN
    CREATE INDEX [IX_SalesTypeDefinition_Name] ON [RII_SALES_TYPE_DEFINITION] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211082723_AddSalesTypeDefinition'
)
BEGIN
    CREATE INDEX [IX_SalesTypeDefinition_SalesType] ON [RII_SALES_TYPE_DEFINITION] ([SalesType]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211082723_AddSalesTypeDefinition'
)
BEGIN
    CREATE UNIQUE INDEX [IX_SalesTypeDefinition_SalesType_Name] ON [RII_SALES_TYPE_DEFINITION] ([SalesType], [Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211082723_AddSalesTypeDefinition'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260211082723_AddSalesTypeDefinition', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211092229_AddActivityImages'
)
BEGIN
    CREATE TABLE [RII_ACTIVITY_IMAGE] (
        [Id] bigint NOT NULL IDENTITY,
        [ActivityId] bigint NOT NULL,
        [ResimAciklama] nvarchar(500) NULL,
        [ResimUrl] nvarchar(1000) NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_ACTIVITY_IMAGE] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_ACTIVITY_IMAGE_RII_ACTIVITY_ActivityId] FOREIGN KEY ([ActivityId]) REFERENCES [RII_ACTIVITY] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RII_ACTIVITY_IMAGE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_ACTIVITY_IMAGE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_ACTIVITY_IMAGE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211092229_AddActivityImages'
)
BEGIN
    CREATE INDEX [IX_ActivityImage_ActivityId] ON [RII_ACTIVITY_IMAGE] ([ActivityId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211092229_AddActivityImages'
)
BEGIN
    CREATE INDEX [IX_ActivityImage_IsDeleted] ON [RII_ACTIVITY_IMAGE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211092229_AddActivityImages'
)
BEGIN
    CREATE INDEX [IX_RII_ACTIVITY_IMAGE_CreatedBy] ON [RII_ACTIVITY_IMAGE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211092229_AddActivityImages'
)
BEGIN
    CREATE INDEX [IX_RII_ACTIVITY_IMAGE_DeletedBy] ON [RII_ACTIVITY_IMAGE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211092229_AddActivityImages'
)
BEGIN
    CREATE INDEX [IX_RII_ACTIVITY_IMAGE_UpdatedBy] ON [RII_ACTIVITY_IMAGE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211092229_AddActivityImages'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260211092229_AddActivityImages', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211121506_AddQuotationSalesTypeAndErpProjectCode'
)
BEGIN
    ALTER TABLE [RII_QUOTATION_LINE] ADD [ErpProjectCode] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211121506_AddQuotationSalesTypeAndErpProjectCode'
)
BEGIN
    ALTER TABLE [RII_QUOTATION] ADD [ErpProjectCode] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211121506_AddQuotationSalesTypeAndErpProjectCode'
)
BEGIN
    ALTER TABLE [RII_QUOTATION] ADD [SalesTypeDefinitionId] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211121506_AddQuotationSalesTypeAndErpProjectCode'
)
BEGIN
    CREATE INDEX [IX_Quotation_SalesTypeDefinitionId] ON [RII_QUOTATION] ([SalesTypeDefinitionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211121506_AddQuotationSalesTypeAndErpProjectCode'
)
BEGIN
    ALTER TABLE [RII_QUOTATION] ADD CONSTRAINT [FK_RII_QUOTATION_RII_SALES_TYPE_DEFINITION_SalesTypeDefinitionId] FOREIGN KEY ([SalesTypeDefinitionId]) REFERENCES [RII_SALES_TYPE_DEFINITION] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211121506_AddQuotationSalesTypeAndErpProjectCode'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260211121506_AddQuotationSalesTypeAndErpProjectCode', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211122704_AddDemandOrderSalesTypeAndErpProjectCode'
)
BEGIN
    ALTER TABLE [RII_ORDER_LINE] ADD [ErpProjectCode] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211122704_AddDemandOrderSalesTypeAndErpProjectCode'
)
BEGIN
    ALTER TABLE [RII_ORDER] ADD [ErpProjectCode] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211122704_AddDemandOrderSalesTypeAndErpProjectCode'
)
BEGIN
    ALTER TABLE [RII_ORDER] ADD [SalesTypeDefinitionId] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211122704_AddDemandOrderSalesTypeAndErpProjectCode'
)
BEGIN
    ALTER TABLE [RII_DEMAND_LINE] ADD [ErpProjectCode] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211122704_AddDemandOrderSalesTypeAndErpProjectCode'
)
BEGIN
    ALTER TABLE [RII_DEMAND] ADD [ErpProjectCode] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211122704_AddDemandOrderSalesTypeAndErpProjectCode'
)
BEGIN
    ALTER TABLE [RII_DEMAND] ADD [SalesTypeDefinitionId] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211122704_AddDemandOrderSalesTypeAndErpProjectCode'
)
BEGIN
    CREATE INDEX [IX_Order_SalesTypeDefinitionId] ON [RII_ORDER] ([SalesTypeDefinitionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211122704_AddDemandOrderSalesTypeAndErpProjectCode'
)
BEGIN
    CREATE INDEX [IX_Demand_SalesTypeDefinitionId] ON [RII_DEMAND] ([SalesTypeDefinitionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211122704_AddDemandOrderSalesTypeAndErpProjectCode'
)
BEGIN
    ALTER TABLE [RII_DEMAND] ADD CONSTRAINT [FK_RII_DEMAND_RII_SALES_TYPE_DEFINITION_SalesTypeDefinitionId] FOREIGN KEY ([SalesTypeDefinitionId]) REFERENCES [RII_SALES_TYPE_DEFINITION] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211122704_AddDemandOrderSalesTypeAndErpProjectCode'
)
BEGIN
    ALTER TABLE [RII_ORDER] ADD CONSTRAINT [FK_RII_ORDER_RII_SALES_TYPE_DEFINITION_SalesTypeDefinitionId] FOREIGN KEY ([SalesTypeDefinitionId]) REFERENCES [RII_SALES_TYPE_DEFINITION] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260211122704_AddDemandOrderSalesTypeAndErpProjectCode'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260211122704_AddDemandOrderSalesTypeAndErpProjectCode', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260216060219_AddQuotationLineDescription123'
)
BEGIN
    DROP INDEX [IX_Country_Name] ON [RII_COUNTRY];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260216060219_AddQuotationLineDescription123'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Country_Name] ON [RII_COUNTRY] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260216060219_AddQuotationLineDescription123'
)
BEGIN
    ALTER TABLE [RII_QUOTATION_LINE] ADD [Description1] nvarchar(200) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260216060219_AddQuotationLineDescription123'
)
BEGIN
    ALTER TABLE [RII_QUOTATION_LINE] ADD [Description2] nvarchar(200) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260216060219_AddQuotationLineDescription123'
)
BEGIN
    ALTER TABLE [RII_QUOTATION_LINE] ADD [Description3] nvarchar(200) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260216060219_AddQuotationLineDescription123'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260216060219_AddQuotationLineDescription123', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260216074500_AddDemandOrderLineDescription123'
)
BEGIN
    ALTER TABLE [RII_DEMAND_LINE] ADD [Description1] nvarchar(200) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260216074500_AddDemandOrderLineDescription123'
)
BEGIN
    ALTER TABLE [RII_DEMAND_LINE] ADD [Description2] nvarchar(200) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260216074500_AddDemandOrderLineDescription123'
)
BEGIN
    ALTER TABLE [RII_DEMAND_LINE] ADD [Description3] nvarchar(200) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260216074500_AddDemandOrderLineDescription123'
)
BEGIN
    ALTER TABLE [RII_ORDER_LINE] ADD [Description1] nvarchar(200) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260216074500_AddDemandOrderLineDescription123'
)
BEGIN
    ALTER TABLE [RII_ORDER_LINE] ADD [Description2] nvarchar(200) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260216074500_AddDemandOrderLineDescription123'
)
BEGIN
    ALTER TABLE [RII_ORDER_LINE] ADD [Description3] nvarchar(200) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260216074500_AddDemandOrderLineDescription123'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260216074500_AddDemandOrderLineDescription123', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260219172631_AddCustomerImageTable'
)
BEGIN
    CREATE TABLE [RII_CUSTOMER_IMAGE] (
        [Id] bigint NOT NULL IDENTITY,
        [CustomerId] bigint NOT NULL,
        [ImageUrl] nvarchar(1000) NOT NULL,
        [ImageDescription] nvarchar(500) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_CUSTOMER_IMAGE] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_CUSTOMER_IMAGE_RII_CUSTOMER_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [RII_CUSTOMER] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RII_CUSTOMER_IMAGE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_CUSTOMER_IMAGE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_CUSTOMER_IMAGE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260219172631_AddCustomerImageTable'
)
BEGIN
    CREATE INDEX [IX_CustomerImage_CustomerId] ON [RII_CUSTOMER_IMAGE] ([CustomerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260219172631_AddCustomerImageTable'
)
BEGIN
    CREATE INDEX [IX_CustomerImage_IsDeleted] ON [RII_CUSTOMER_IMAGE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260219172631_AddCustomerImageTable'
)
BEGIN
    CREATE INDEX [IX_RII_CUSTOMER_IMAGE_CreatedBy] ON [RII_CUSTOMER_IMAGE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260219172631_AddCustomerImageTable'
)
BEGIN
    CREATE INDEX [IX_RII_CUSTOMER_IMAGE_DeletedBy] ON [RII_CUSTOMER_IMAGE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260219172631_AddCustomerImageTable'
)
BEGIN
    CREATE INDEX [IX_RII_CUSTOMER_IMAGE_UpdatedBy] ON [RII_CUSTOMER_IMAGE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260219172631_AddCustomerImageTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260219172631_AddCustomerImageTable', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260220083546_AddCustomerAndShippingAddressCoordinates'
)
BEGIN
    ALTER TABLE [RII_SHIPPING_ADDRESS] ADD [Latitude] decimal(9,6) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260220083546_AddCustomerAndShippingAddressCoordinates'
)
BEGIN
    ALTER TABLE [RII_SHIPPING_ADDRESS] ADD [Longitude] decimal(9,6) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260220083546_AddCustomerAndShippingAddressCoordinates'
)
BEGIN
    ALTER TABLE [RII_CUSTOMER] ADD [Latitude] decimal(9,6) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260220083546_AddCustomerAndShippingAddressCoordinates'
)
BEGIN
    ALTER TABLE [RII_CUSTOMER] ADD [Longitude] decimal(9,6) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260220083546_AddCustomerAndShippingAddressCoordinates'
)
BEGIN
    CREATE INDEX [IX_ShippingAddress_Latitude_Longitude] ON [RII_SHIPPING_ADDRESS] ([Latitude], [Longitude]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260220083546_AddCustomerAndShippingAddressCoordinates'
)
BEGIN
    CREATE INDEX [IX_Customer_Latitude_Longitude] ON [RII_CUSTOMER] ([Latitude], [Longitude]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260220083546_AddCustomerAndShippingAddressCoordinates'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260220083546_AddCustomerAndShippingAddressCoordinates', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260303122500_AddUserGoogleAccountIntegration'
)
BEGIN
    CREATE TABLE [RII_USER_GOOGLE_ACCOUNTS] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] bigint NOT NULL,
        [GoogleEmail] nvarchar(256) NULL,
        [RefreshTokenEncrypted] nvarchar(4000) NULL,
        [AccessTokenEncrypted] nvarchar(4000) NULL,
        [ExpiresAt] datetimeoffset NULL,
        [Scopes] nvarchar(2000) NULL,
        [IsConnected] bit NOT NULL DEFAULT CAST(0 AS bit),
        [CreatedAt] datetimeoffset NOT NULL DEFAULT (SYSUTCDATETIME()),
        [UpdatedAt] datetimeoffset NOT NULL DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_RII_USER_GOOGLE_ACCOUNTS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_USER_GOOGLE_ACCOUNTS_RII_USERS_UserId] FOREIGN KEY ([UserId]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260303122500_AddUserGoogleAccountIntegration'
)
BEGIN
    CREATE UNIQUE INDEX [IX_UserGoogleAccounts_UserId] ON [RII_USER_GOOGLE_ACCOUNTS] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260303122500_AddUserGoogleAccountIntegration'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260303122500_AddUserGoogleAccountIntegration', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260303181603_AddTenantGoogleOAuthSettings'
)
BEGIN
    ALTER TABLE [RII_USER_GOOGLE_ACCOUNTS] ADD [TenantId] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260303181603_AddTenantGoogleOAuthSettings'
)
BEGIN
    CREATE TABLE [RII_TENANT_GOOGLE_OAUTH_SETTINGS] (
        [Id] uniqueidentifier NOT NULL,
        [TenantId] uniqueidentifier NOT NULL,
        [ClientId] nvarchar(256) NOT NULL,
        [ClientSecretEncrypted] nvarchar(4000) NOT NULL,
        [RedirectUri] nvarchar(512) NULL,
        [Scopes] nvarchar(2000) NULL,
        [IsEnabled] bit NOT NULL DEFAULT CAST(0 AS bit),
        [CreatedAt] datetimeoffset NOT NULL DEFAULT (SYSUTCDATETIME()),
        [UpdatedAt] datetimeoffset NOT NULL DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_RII_TENANT_GOOGLE_OAUTH_SETTINGS] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260303181603_AddTenantGoogleOAuthSettings'
)
BEGIN
    CREATE INDEX [IX_UserGoogleAccounts_TenantId] ON [RII_USER_GOOGLE_ACCOUNTS] ([TenantId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260303181603_AddTenantGoogleOAuthSettings'
)
BEGIN
    CREATE UNIQUE INDEX [IX_TenantGoogleOAuthSettings_TenantId] ON [RII_TENANT_GOOGLE_OAUTH_SETTINGS] ([TenantId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260303181603_AddTenantGoogleOAuthSettings'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260303181603_AddTenantGoogleOAuthSettings', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260304064319_AddGoogleCalendarEventIdToActivity'
)
BEGIN
    ALTER TABLE [RII_ACTIVITY] ADD [GoogleCalendarEventId] nvarchar(512) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260304064319_AddGoogleCalendarEventIdToActivity'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260304064319_AddGoogleCalendarEventIdToActivity', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260304112551_AddGoogleIntegrationLogs'
)
BEGIN
    CREATE TABLE [RII_GOOGLE_INTEGRATION_LOGS] (
        [Id] bigint NOT NULL IDENTITY,
        [TenantId] uniqueidentifier NOT NULL,
        [UserId] bigint NULL,
        [Operation] nvarchar(120) NOT NULL,
        [IsSuccess] bit NOT NULL,
        [Severity] nvarchar(32) NOT NULL,
        [Provider] nvarchar(64) NOT NULL,
        [Message] nvarchar(2000) NULL,
        [ErrorCode] nvarchar(256) NULL,
        [ActivityId] bigint NULL,
        [GoogleCalendarEventId] nvarchar(512) NULL,
        [MetadataJson] nvarchar(4000) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_GOOGLE_INTEGRATION_LOGS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_GOOGLE_INTEGRATION_LOGS_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_GOOGLE_INTEGRATION_LOGS_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_GOOGLE_INTEGRATION_LOGS_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_GOOGLE_INTEGRATION_LOGS_RII_USERS_UserId] FOREIGN KEY ([UserId]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260304112551_AddGoogleIntegrationLogs'
)
BEGIN
    CREATE INDEX [IX_GoogleIntegrationLogs_CreatedDate] ON [RII_GOOGLE_INTEGRATION_LOGS] ([CreatedDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260304112551_AddGoogleIntegrationLogs'
)
BEGIN
    CREATE INDEX [IX_GoogleIntegrationLogs_TenantId] ON [RII_GOOGLE_INTEGRATION_LOGS] ([TenantId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260304112551_AddGoogleIntegrationLogs'
)
BEGIN
    CREATE INDEX [IX_GoogleIntegrationLogs_UserId] ON [RII_GOOGLE_INTEGRATION_LOGS] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260304112551_AddGoogleIntegrationLogs'
)
BEGIN
    CREATE INDEX [IX_RII_GOOGLE_INTEGRATION_LOGS_CreatedBy] ON [RII_GOOGLE_INTEGRATION_LOGS] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260304112551_AddGoogleIntegrationLogs'
)
BEGIN
    CREATE INDEX [IX_RII_GOOGLE_INTEGRATION_LOGS_DeletedBy] ON [RII_GOOGLE_INTEGRATION_LOGS] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260304112551_AddGoogleIntegrationLogs'
)
BEGIN
    CREATE INDEX [IX_RII_GOOGLE_INTEGRATION_LOGS_UpdatedBy] ON [RII_GOOGLE_INTEGRATION_LOGS] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260304112551_AddGoogleIntegrationLogs'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260304112551_AddGoogleIntegrationLogs', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260305090645_Add_RII_GOOGLE_CUSTOMER_MAIL_LOGS'
)
BEGIN
    CREATE TABLE [RII_GOOGLE_CUSTOMER_MAIL_LOGS] (
        [Id] bigint NOT NULL IDENTITY,
        [TenantId] uniqueidentifier NOT NULL,
        [CustomerId] bigint NOT NULL,
        [ContactId] bigint NULL,
        [SentByUserId] bigint NOT NULL,
        [Provider] nvarchar(64) NOT NULL,
        [SenderEmail] nvarchar(320) NULL,
        [ToEmails] nvarchar(4000) NOT NULL,
        [CcEmails] nvarchar(4000) NULL,
        [BccEmails] nvarchar(4000) NULL,
        [Subject] nvarchar(512) NOT NULL,
        [Body] nvarchar(max) NOT NULL,
        [IsHtml] bit NOT NULL,
        [TemplateKey] nvarchar(128) NULL,
        [TemplateName] nvarchar(256) NULL,
        [TemplateVersion] nvarchar(64) NULL,
        [IsSuccess] bit NOT NULL,
        [ErrorCode] nvarchar(128) NULL,
        [ErrorMessage] nvarchar(2000) NULL,
        [GoogleMessageId] nvarchar(512) NULL,
        [GoogleThreadId] nvarchar(512) NULL,
        [SentAt] datetimeoffset NULL,
        [MetadataJson] nvarchar(4000) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_GOOGLE_CUSTOMER_MAIL_LOGS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_GOOGLE_CUSTOMER_MAIL_LOGS_RII_CONTACT_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [RII_CONTACT] ([Id]),
        CONSTRAINT [FK_RII_GOOGLE_CUSTOMER_MAIL_LOGS_RII_CUSTOMER_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [RII_CUSTOMER] ([Id]),
        CONSTRAINT [FK_RII_GOOGLE_CUSTOMER_MAIL_LOGS_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_GOOGLE_CUSTOMER_MAIL_LOGS_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_GOOGLE_CUSTOMER_MAIL_LOGS_RII_USERS_SentByUserId] FOREIGN KEY ([SentByUserId]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_GOOGLE_CUSTOMER_MAIL_LOGS_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260305090645_Add_RII_GOOGLE_CUSTOMER_MAIL_LOGS'
)
BEGIN
    CREATE INDEX [IX_GoogleCustomerMailLogs_ContactId] ON [RII_GOOGLE_CUSTOMER_MAIL_LOGS] ([ContactId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260305090645_Add_RII_GOOGLE_CUSTOMER_MAIL_LOGS'
)
BEGIN
    CREATE INDEX [IX_GoogleCustomerMailLogs_CreatedDate] ON [RII_GOOGLE_CUSTOMER_MAIL_LOGS] ([CreatedDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260305090645_Add_RII_GOOGLE_CUSTOMER_MAIL_LOGS'
)
BEGIN
    CREATE INDEX [IX_GoogleCustomerMailLogs_CustomerId] ON [RII_GOOGLE_CUSTOMER_MAIL_LOGS] ([CustomerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260305090645_Add_RII_GOOGLE_CUSTOMER_MAIL_LOGS'
)
BEGIN
    CREATE INDEX [IX_GoogleCustomerMailLogs_IsSuccess] ON [RII_GOOGLE_CUSTOMER_MAIL_LOGS] ([IsSuccess]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260305090645_Add_RII_GOOGLE_CUSTOMER_MAIL_LOGS'
)
BEGIN
    CREATE INDEX [IX_GoogleCustomerMailLogs_SentByUserId] ON [RII_GOOGLE_CUSTOMER_MAIL_LOGS] ([SentByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260305090645_Add_RII_GOOGLE_CUSTOMER_MAIL_LOGS'
)
BEGIN
    CREATE INDEX [IX_GoogleCustomerMailLogs_TenantId] ON [RII_GOOGLE_CUSTOMER_MAIL_LOGS] ([TenantId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260305090645_Add_RII_GOOGLE_CUSTOMER_MAIL_LOGS'
)
BEGIN
    CREATE INDEX [IX_RII_GOOGLE_CUSTOMER_MAIL_LOGS_CreatedBy] ON [RII_GOOGLE_CUSTOMER_MAIL_LOGS] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260305090645_Add_RII_GOOGLE_CUSTOMER_MAIL_LOGS'
)
BEGIN
    CREATE INDEX [IX_RII_GOOGLE_CUSTOMER_MAIL_LOGS_DeletedBy] ON [RII_GOOGLE_CUSTOMER_MAIL_LOGS] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260305090645_Add_RII_GOOGLE_CUSTOMER_MAIL_LOGS'
)
BEGIN
    CREATE INDEX [IX_RII_GOOGLE_CUSTOMER_MAIL_LOGS_UpdatedBy] ON [RII_GOOGLE_CUSTOMER_MAIL_LOGS] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260305090645_Add_RII_GOOGLE_CUSTOMER_MAIL_LOGS'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260305090645_Add_RII_GOOGLE_CUSTOMER_MAIL_LOGS', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306091425_EnforceUniqueDefaultReportTemplatePerRuleType'
)
BEGIN
    DROP INDEX [IX_RII_REPORT_TEMPLATES_RuleType_Default] ON [RII_REPORT_TEMPLATES];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306091425_EnforceUniqueDefaultReportTemplatePerRuleType'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_RII_REPORT_TEMPLATES_RuleType_Default] ON [RII_REPORT_TEMPLATES] ([RuleType], [Default]) WHERE [Default] = 1 AND [IsDeleted] = 0');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306091425_EnforceUniqueDefaultReportTemplatePerRuleType'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260306091425_EnforceUniqueDefaultReportTemplatePerRuleType', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306145922_AddJobFailureLogTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260306145922_AddJobFailureLogTable', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306151324_EnsureJobFailureLogTableExistsV2'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260306151324_EnsureJobFailureLogTableExistsV2', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306200000_EnsureJobFailureLogTableExists'
)
BEGIN
    IF OBJECT_ID(N'[dbo].[RII_JOB_FAILURE_LOG]', N'U') IS NULL
    BEGIN
        CREATE TABLE [dbo].[RII_JOB_FAILURE_LOG](
            [Id] BIGINT IDENTITY(1,1) NOT NULL,
            [JobId] NVARCHAR(100) NOT NULL,
            [JobName] NVARCHAR(500) NOT NULL,
            [FailedAt] DATETIME2 NOT NULL,
            [Reason] NVARCHAR(2000) NULL,
            [ExceptionType] NVARCHAR(500) NULL,
            [ExceptionMessage] NVARCHAR(4000) NULL,
            [StackTrace] NVARCHAR(4000) NULL,
            [Queue] NVARCHAR(100) NULL,
            [RetryCount] INT NOT NULL,
            [CreatedDate] DATETIME2 NOT NULL CONSTRAINT [DF_RII_JOB_FAILURE_LOG_CreatedDate] DEFAULT (GETUTCDATE()),
            [UpdatedDate] DATETIME2 NULL,
            [DeletedDate] DATETIME2 NULL,
            [IsDeleted] BIT NOT NULL,
            [CreatedBy] BIGINT NULL,
            [UpdatedBy] BIGINT NULL,
            [DeletedBy] BIGINT NULL,
            CONSTRAINT [PK_RII_JOB_FAILURE_LOG] PRIMARY KEY CLUSTERED ([Id] ASC)
        );
    END
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306200000_EnsureJobFailureLogTableExists'
)
BEGIN
    IF OBJECT_ID(N'[dbo].[RII_JOB_FAILURE_LOG]', N'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_JobFailureLog_JobId' AND object_id = OBJECT_ID(N'[dbo].[RII_JOB_FAILURE_LOG]'))
    BEGIN
        CREATE INDEX [IX_JobFailureLog_JobId] ON [dbo].[RII_JOB_FAILURE_LOG]([JobId]);
    END
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306200000_EnsureJobFailureLogTableExists'
)
BEGIN
    IF OBJECT_ID(N'[dbo].[RII_JOB_FAILURE_LOG]', N'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_JobFailureLog_FailedAt' AND object_id = OBJECT_ID(N'[dbo].[RII_JOB_FAILURE_LOG]'))
    BEGIN
        CREATE INDEX [IX_JobFailureLog_FailedAt] ON [dbo].[RII_JOB_FAILURE_LOG]([FailedAt]);
    END
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306200000_EnsureJobFailureLogTableExists'
)
BEGIN
    IF OBJECT_ID(N'[dbo].[RII_JOB_FAILURE_LOG]', N'U') IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_JobFailureLog_JobName' AND object_id = OBJECT_ID(N'[dbo].[RII_JOB_FAILURE_LOG]'))
    BEGIN
        CREATE INDEX [IX_JobFailureLog_JobName] ON [dbo].[RII_JOB_FAILURE_LOG]([JobName]);
    END
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260306200000_EnsureJobFailureLogTableExists'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260306200000_EnsureJobFailureLogTableExists', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307142249_AddTempQuotationHeaderTablePhase1'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_JOB_FAILURE_LOG]') AND [c].[name] = N'StackTrace');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [RII_JOB_FAILURE_LOG] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [RII_JOB_FAILURE_LOG] ALTER COLUMN [StackTrace] nvarchar(4000) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307142249_AddTempQuotationHeaderTablePhase1'
)
BEGIN
    CREATE TABLE [RII_TEMP_QUOTATION] (
        [Id] bigint NOT NULL IDENTITY,
        [CustomerId] bigint NOT NULL,
        [OfferDate] datetime2 NOT NULL,
        [CurrencyCode] nvarchar(10) NOT NULL,
        [ExchangeRate] decimal(18,6) NOT NULL DEFAULT 1.0,
        [DiscountRate1] decimal(18,6) NOT NULL DEFAULT 0.0,
        [DiscountRate2] decimal(18,6) NOT NULL DEFAULT 0.0,
        [DiscountRate3] decimal(18,6) NOT NULL DEFAULT 0.0,
        [IsApproved] bit NOT NULL DEFAULT CAST(0 AS bit),
        [ApprovedDate] datetime2 NULL,
        [Description] nvarchar(500) NOT NULL DEFAULT N'',
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_TEMP_QUOTATION] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_TEMP_QUOTATION_RII_CUSTOMER_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [RII_CUSTOMER] ([Id]),
        CONSTRAINT [FK_RII_TEMP_QUOTATION_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_TEMP_QUOTATION_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_TEMP_QUOTATION_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307142249_AddTempQuotationHeaderTablePhase1'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATION_CreatedBy] ON [RII_TEMP_QUOTATION] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307142249_AddTempQuotationHeaderTablePhase1'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATION_CreatedDate] ON [RII_TEMP_QUOTATION] ([CreatedDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307142249_AddTempQuotationHeaderTablePhase1'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATION_CustomerId] ON [RII_TEMP_QUOTATION] ([CustomerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307142249_AddTempQuotationHeaderTablePhase1'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATION_DeletedBy] ON [RII_TEMP_QUOTATION] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307142249_AddTempQuotationHeaderTablePhase1'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATION_IsApproved] ON [RII_TEMP_QUOTATION] ([IsApproved]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307142249_AddTempQuotationHeaderTablePhase1'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATION_UpdatedBy] ON [RII_TEMP_QUOTATION] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307142249_AddTempQuotationHeaderTablePhase1'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260307142249_AddTempQuotationHeaderTablePhase1', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    DROP TABLE [RII_TEMP_QUOTATION];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    CREATE TABLE [RII_TEMP_QUOTATTION] (
        [Id] bigint NOT NULL IDENTITY,
        [CustomerId] bigint NOT NULL,
        [OfferDate] datetime2 NOT NULL,
        [CurrencyCode] nvarchar(10) NOT NULL,
        [ExchangeRate] decimal(18,6) NOT NULL DEFAULT 1.0,
        [DiscountRate1] decimal(18,6) NOT NULL DEFAULT 0.0,
        [DiscountRate2] decimal(18,6) NOT NULL DEFAULT 0.0,
        [DiscountRate3] decimal(18,6) NOT NULL DEFAULT 0.0,
        [IsApproved] bit NOT NULL DEFAULT CAST(0 AS bit),
        [ApprovedDate] datetime2 NULL,
        [Description] nvarchar(500) NOT NULL DEFAULT N'',
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_TEMP_QUOTATTION] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_TEMP_QUOTATTION_RII_CUSTOMER_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [RII_CUSTOMER] ([Id]),
        CONSTRAINT [FK_RII_TEMP_QUOTATTION_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_TEMP_QUOTATTION_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_TEMP_QUOTATTION_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    CREATE TABLE [RII_TEMP_QUOTATTION_EXCHANGE_LINE] (
        [Id] bigint NOT NULL IDENTITY,
        [TempQuotattionId] bigint NOT NULL,
        [Currency] nvarchar(10) NOT NULL,
        [ExchangeRate] decimal(18,6) NOT NULL,
        [ExchangeRateDate] datetime2 NOT NULL,
        [IsManual] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_TEMP_QUOTATTION_EXCHANGE_LINE] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_TEMP_QUOTATTION_EXCHANGE_LINE_RII_TEMP_QUOTATTION_TempQuotattionId] FOREIGN KEY ([TempQuotattionId]) REFERENCES [RII_TEMP_QUOTATTION] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RII_TEMP_QUOTATTION_EXCHANGE_LINE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_TEMP_QUOTATTION_EXCHANGE_LINE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_TEMP_QUOTATTION_EXCHANGE_LINE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    CREATE TABLE [RII_TEMP_QUOTATTION_LINE] (
        [Id] bigint NOT NULL IDENTITY,
        [TempQuotattionId] bigint NOT NULL,
        [ProductCode] nvarchar(100) NOT NULL,
        [ProductName] nvarchar(250) NOT NULL,
        [Quantity] decimal(18,6) NOT NULL,
        [UnitPrice] decimal(18,6) NOT NULL,
        [DiscountRate1] decimal(18,6) NOT NULL,
        [DiscountAmount1] decimal(18,6) NOT NULL,
        [DiscountRate2] decimal(18,6) NOT NULL,
        [DiscountAmount2] decimal(18,6) NOT NULL,
        [DiscountRate3] decimal(18,6) NOT NULL,
        [DiscountAmount3] decimal(18,6) NOT NULL,
        [VatRate] decimal(18,6) NOT NULL,
        [VatAmount] decimal(18,6) NOT NULL,
        [LineTotal] decimal(18,6) NOT NULL,
        [LineGrandTotal] decimal(18,6) NOT NULL,
        [Description] nvarchar(500) NOT NULL DEFAULT N'',
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_TEMP_QUOTATTION_LINE] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_TEMP_QUOTATTION_LINE_RII_TEMP_QUOTATTION_TempQuotattionId] FOREIGN KEY ([TempQuotattionId]) REFERENCES [RII_TEMP_QUOTATTION] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RII_TEMP_QUOTATTION_LINE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_TEMP_QUOTATTION_LINE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_TEMP_QUOTATTION_LINE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATTION_CreatedBy] ON [RII_TEMP_QUOTATTION] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATTION_CreatedDate] ON [RII_TEMP_QUOTATTION] ([CreatedDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATTION_CustomerId] ON [RII_TEMP_QUOTATTION] ([CustomerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATTION_DeletedBy] ON [RII_TEMP_QUOTATTION] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATTION_IsApproved] ON [RII_TEMP_QUOTATTION] ([IsApproved]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATTION_UpdatedBy] ON [RII_TEMP_QUOTATTION] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATTION_EXCHANGE_LINE_CreatedBy] ON [RII_TEMP_QUOTATTION_EXCHANGE_LINE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATTION_EXCHANGE_LINE_Currency] ON [RII_TEMP_QUOTATTION_EXCHANGE_LINE] ([Currency]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATTION_EXCHANGE_LINE_DeletedBy] ON [RII_TEMP_QUOTATTION_EXCHANGE_LINE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATTION_EXCHANGE_LINE_IsDeleted] ON [RII_TEMP_QUOTATTION_EXCHANGE_LINE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATTION_EXCHANGE_LINE_TempQuotattionId] ON [RII_TEMP_QUOTATTION_EXCHANGE_LINE] ([TempQuotattionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATTION_EXCHANGE_LINE_UpdatedBy] ON [RII_TEMP_QUOTATTION_EXCHANGE_LINE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATTION_LINE_CreatedBy] ON [RII_TEMP_QUOTATTION_LINE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATTION_LINE_DeletedBy] ON [RII_TEMP_QUOTATTION_LINE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATTION_LINE_IsDeleted] ON [RII_TEMP_QUOTATTION_LINE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATTION_LINE_ProductCode] ON [RII_TEMP_QUOTATTION_LINE] ([ProductCode]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATTION_LINE_TempQuotattionId] ON [RII_TEMP_QUOTATTION_LINE] ([TempQuotattionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATTION_LINE_UpdatedBy] ON [RII_TEMP_QUOTATTION_LINE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260307143607_AddTempQuotattionLineAndExchangeLineTablesPhase2', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260308131007_AddTenantOutlookOAuthSettings'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260308131007_AddTenantOutlookOAuthSettings', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260308131033_AddUserOutlookAccountTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260308131033_AddUserOutlookAccountTable', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260308131054_AddOutlookIntegrationLogs'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260308131054_AddOutlookIntegrationLogs', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260308131116_AddOutlookCustomerMailLogs'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260308131116_AddOutlookCustomerMailLogs', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260310081905_AddOutlookMailAndOAuthImplementation'
)
BEGIN
    CREATE TABLE [RII_OUTLOOK_CUSTOMER_MAIL_LOGS] (
        [Id] bigint NOT NULL IDENTITY,
        [TenantId] uniqueidentifier NOT NULL,
        [CustomerId] bigint NOT NULL,
        [ContactId] bigint NULL,
        [SentByUserId] bigint NOT NULL,
        [Provider] nvarchar(64) NOT NULL,
        [SenderEmail] nvarchar(320) NULL,
        [ToEmails] nvarchar(4000) NOT NULL,
        [CcEmails] nvarchar(4000) NULL,
        [BccEmails] nvarchar(4000) NULL,
        [Subject] nvarchar(512) NOT NULL,
        [Body] nvarchar(max) NOT NULL,
        [IsHtml] bit NOT NULL,
        [TemplateKey] nvarchar(128) NULL,
        [TemplateName] nvarchar(256) NULL,
        [TemplateVersion] nvarchar(64) NULL,
        [IsSuccess] bit NOT NULL,
        [ErrorCode] nvarchar(128) NULL,
        [ErrorMessage] nvarchar(2000) NULL,
        [OutlookMessageId] nvarchar(512) NULL,
        [OutlookConversationId] nvarchar(512) NULL,
        [SentAt] datetimeoffset NULL,
        [MetadataJson] nvarchar(4000) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_OUTLOOK_CUSTOMER_MAIL_LOGS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_OUTLOOK_CUSTOMER_MAIL_LOGS_RII_CONTACT_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [RII_CONTACT] ([Id]),
        CONSTRAINT [FK_RII_OUTLOOK_CUSTOMER_MAIL_LOGS_RII_CUSTOMER_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [RII_CUSTOMER] ([Id]),
        CONSTRAINT [FK_RII_OUTLOOK_CUSTOMER_MAIL_LOGS_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_OUTLOOK_CUSTOMER_MAIL_LOGS_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_OUTLOOK_CUSTOMER_MAIL_LOGS_RII_USERS_SentByUserId] FOREIGN KEY ([SentByUserId]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_OUTLOOK_CUSTOMER_MAIL_LOGS_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260310081905_AddOutlookMailAndOAuthImplementation'
)
BEGIN
    CREATE TABLE [RII_OUTLOOK_INTEGRATION_LOGS] (
        [Id] bigint NOT NULL IDENTITY,
        [TenantId] uniqueidentifier NOT NULL,
        [UserId] bigint NULL,
        [Operation] nvarchar(120) NOT NULL,
        [IsSuccess] bit NOT NULL,
        [Severity] nvarchar(32) NOT NULL,
        [Provider] nvarchar(64) NOT NULL,
        [Message] nvarchar(2000) NULL,
        [ErrorCode] nvarchar(256) NULL,
        [ActivityId] nvarchar(128) NULL,
        [ProviderEventId] nvarchar(512) NULL,
        [MetadataJson] nvarchar(4000) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_OUTLOOK_INTEGRATION_LOGS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_OUTLOOK_INTEGRATION_LOGS_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_OUTLOOK_INTEGRATION_LOGS_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_OUTLOOK_INTEGRATION_LOGS_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_OUTLOOK_INTEGRATION_LOGS_RII_USERS_UserId] FOREIGN KEY ([UserId]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260310081905_AddOutlookMailAndOAuthImplementation'
)
BEGIN
    CREATE TABLE [RII_USER_OUTLOOK_ACCOUNTS] (
        [Id] uniqueidentifier NOT NULL,
        [TenantId] uniqueidentifier NOT NULL,
        [UserId] bigint NOT NULL,
        [OutlookEmail] nvarchar(256) NULL,
        [RefreshTokenEncrypted] nvarchar(4000) NULL,
        [AccessTokenEncrypted] nvarchar(4000) NULL,
        [ExpiresAt] datetimeoffset NULL,
        [Scopes] nvarchar(2000) NULL,
        [IsConnected] bit NOT NULL DEFAULT CAST(0 AS bit),
        [CreatedAt] datetimeoffset NOT NULL DEFAULT (SYSUTCDATETIME()),
        [UpdatedAt] datetimeoffset NOT NULL DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_RII_USER_OUTLOOK_ACCOUNTS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_USER_OUTLOOK_ACCOUNTS_RII_USERS_UserId] FOREIGN KEY ([UserId]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260310081905_AddOutlookMailAndOAuthImplementation'
)
BEGIN
    CREATE INDEX [IX_OutlookCustomerMailLogs_ContactId] ON [RII_OUTLOOK_CUSTOMER_MAIL_LOGS] ([ContactId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260310081905_AddOutlookMailAndOAuthImplementation'
)
BEGIN
    CREATE INDEX [IX_OutlookCustomerMailLogs_CreatedDate] ON [RII_OUTLOOK_CUSTOMER_MAIL_LOGS] ([CreatedDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260310081905_AddOutlookMailAndOAuthImplementation'
)
BEGIN
    CREATE INDEX [IX_OutlookCustomerMailLogs_CustomerId] ON [RII_OUTLOOK_CUSTOMER_MAIL_LOGS] ([CustomerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260310081905_AddOutlookMailAndOAuthImplementation'
)
BEGIN
    CREATE INDEX [IX_OutlookCustomerMailLogs_IsSuccess] ON [RII_OUTLOOK_CUSTOMER_MAIL_LOGS] ([IsSuccess]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260310081905_AddOutlookMailAndOAuthImplementation'
)
BEGIN
    CREATE INDEX [IX_OutlookCustomerMailLogs_SentByUserId] ON [RII_OUTLOOK_CUSTOMER_MAIL_LOGS] ([SentByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260310081905_AddOutlookMailAndOAuthImplementation'
)
BEGIN
    CREATE INDEX [IX_OutlookCustomerMailLogs_TenantId] ON [RII_OUTLOOK_CUSTOMER_MAIL_LOGS] ([TenantId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260310081905_AddOutlookMailAndOAuthImplementation'
)
BEGIN
    CREATE INDEX [IX_RII_OUTLOOK_CUSTOMER_MAIL_LOGS_CreatedBy] ON [RII_OUTLOOK_CUSTOMER_MAIL_LOGS] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260310081905_AddOutlookMailAndOAuthImplementation'
)
BEGIN
    CREATE INDEX [IX_RII_OUTLOOK_CUSTOMER_MAIL_LOGS_DeletedBy] ON [RII_OUTLOOK_CUSTOMER_MAIL_LOGS] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260310081905_AddOutlookMailAndOAuthImplementation'
)
BEGIN
    CREATE INDEX [IX_RII_OUTLOOK_CUSTOMER_MAIL_LOGS_UpdatedBy] ON [RII_OUTLOOK_CUSTOMER_MAIL_LOGS] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260310081905_AddOutlookMailAndOAuthImplementation'
)
BEGIN
    CREATE INDEX [IX_OutlookIntegrationLogs_CreatedDate] ON [RII_OUTLOOK_INTEGRATION_LOGS] ([CreatedDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260310081905_AddOutlookMailAndOAuthImplementation'
)
BEGIN
    CREATE INDEX [IX_OutlookIntegrationLogs_TenantId] ON [RII_OUTLOOK_INTEGRATION_LOGS] ([TenantId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260310081905_AddOutlookMailAndOAuthImplementation'
)
BEGIN
    CREATE INDEX [IX_OutlookIntegrationLogs_UserId] ON [RII_OUTLOOK_INTEGRATION_LOGS] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260310081905_AddOutlookMailAndOAuthImplementation'
)
BEGIN
    CREATE INDEX [IX_RII_OUTLOOK_INTEGRATION_LOGS_CreatedBy] ON [RII_OUTLOOK_INTEGRATION_LOGS] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260310081905_AddOutlookMailAndOAuthImplementation'
)
BEGIN
    CREATE INDEX [IX_RII_OUTLOOK_INTEGRATION_LOGS_DeletedBy] ON [RII_OUTLOOK_INTEGRATION_LOGS] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260310081905_AddOutlookMailAndOAuthImplementation'
)
BEGIN
    CREATE INDEX [IX_RII_OUTLOOK_INTEGRATION_LOGS_UpdatedBy] ON [RII_OUTLOOK_INTEGRATION_LOGS] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260310081905_AddOutlookMailAndOAuthImplementation'
)
BEGIN
    CREATE INDEX [IX_UserOutlookAccounts_TenantId] ON [RII_USER_OUTLOOK_ACCOUNTS] ([TenantId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260310081905_AddOutlookMailAndOAuthImplementation'
)
BEGIN
    CREATE UNIQUE INDEX [IX_UserOutlookAccounts_UserId] ON [RII_USER_OUTLOOK_ACCOUNTS] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260310081905_AddOutlookMailAndOAuthImplementation'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260310081905_AddOutlookMailAndOAuthImplementation', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260311202537_AddQuotationLinkColumnsToTempQuotation'
)
BEGIN
    ALTER TABLE [RII_TEMP_QUOTATTION] ADD [QuotationId] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260311202537_AddQuotationLinkColumnsToTempQuotation'
)
BEGIN
    ALTER TABLE [RII_TEMP_QUOTATTION] ADD [QuotationNo] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260311202537_AddQuotationLinkColumnsToTempQuotation'
)
BEGIN
    ALTER TABLE [RII_TEMP_QUOTATTION] ADD [RevisionId] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260311202537_AddQuotationLinkColumnsToTempQuotation'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATTION_QuotationId] ON [RII_TEMP_QUOTATTION] ([QuotationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260311202537_AddQuotationLinkColumnsToTempQuotation'
)
BEGIN
    CREATE INDEX [IX_RII_TEMP_QUOTATTION_RevisionId] ON [RII_TEMP_QUOTATTION] ([RevisionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260311202537_AddQuotationLinkColumnsToTempQuotation'
)
BEGIN
    ALTER TABLE [RII_TEMP_QUOTATTION] ADD CONSTRAINT [FK_RII_TEMP_QUOTATTION_RII_QUOTATION_QuotationId] FOREIGN KEY ([QuotationId]) REFERENCES [RII_QUOTATION] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260311202537_AddQuotationLinkColumnsToTempQuotation'
)
BEGIN
    ALTER TABLE [RII_TEMP_QUOTATTION] ADD CONSTRAINT [FK_RII_TEMP_QUOTATTION_RII_TEMP_QUOTATTION_RevisionId] FOREIGN KEY ([RevisionId]) REFERENCES [RII_TEMP_QUOTATTION] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260311202537_AddQuotationLinkColumnsToTempQuotation'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260311202537_AddQuotationLinkColumnsToTempQuotation', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_USERS]') AND [c].[name] = N'CreatedDate');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [RII_USERS] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [RII_USERS] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_USER_SESSION]') AND [c].[name] = N'CreatedDate');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [RII_USER_SESSION] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [RII_USER_SESSION] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_USER_POWERBI_GROUPS]') AND [c].[name] = N'CreatedDate');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [RII_USER_POWERBI_GROUPS] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [RII_USER_POWERBI_GROUPS] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_USER_PERMISSION_GROUPS]') AND [c].[name] = N'CreatedDate');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [RII_USER_PERMISSION_GROUPS] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [RII_USER_PERMISSION_GROUPS] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_USER_DISCOUNT_LIMIT]') AND [c].[name] = N'CreatedDate');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [RII_USER_DISCOUNT_LIMIT] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [RII_USER_DISCOUNT_LIMIT] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var8 sysname;
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_USER_DETAIL]') AND [c].[name] = N'CreatedDate');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [RII_USER_DETAIL] DROP CONSTRAINT [' + @var8 + '];');
    ALTER TABLE [RII_USER_DETAIL] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var9 sysname;
    SELECT @var9 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_USER_AUTHORITY]') AND [c].[name] = N'CreatedDate');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [RII_USER_AUTHORITY] DROP CONSTRAINT [' + @var9 + '];');
    ALTER TABLE [RII_USER_AUTHORITY] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var10 sysname;
    SELECT @var10 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_TITLE]') AND [c].[name] = N'CreatedDate');
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [RII_TITLE] DROP CONSTRAINT [' + @var10 + '];');
    ALTER TABLE [RII_TITLE] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var11 sysname;
    SELECT @var11 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_TEMP_QUOTATTION_LINE]') AND [c].[name] = N'CreatedDate');
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [RII_TEMP_QUOTATTION_LINE] DROP CONSTRAINT [' + @var11 + '];');
    ALTER TABLE [RII_TEMP_QUOTATTION_LINE] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var12 sysname;
    SELECT @var12 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_TEMP_QUOTATTION_EXCHANGE_LINE]') AND [c].[name] = N'CreatedDate');
    IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [RII_TEMP_QUOTATTION_EXCHANGE_LINE] DROP CONSTRAINT [' + @var12 + '];');
    ALTER TABLE [RII_TEMP_QUOTATTION_EXCHANGE_LINE] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var13 sysname;
    SELECT @var13 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_TEMP_QUOTATTION]') AND [c].[name] = N'CreatedDate');
    IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [RII_TEMP_QUOTATTION] DROP CONSTRAINT [' + @var13 + '];');
    ALTER TABLE [RII_TEMP_QUOTATTION] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var14 sysname;
    SELECT @var14 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_STOCK_RELATION]') AND [c].[name] = N'CreatedDate');
    IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [RII_STOCK_RELATION] DROP CONSTRAINT [' + @var14 + '];');
    ALTER TABLE [RII_STOCK_RELATION] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var15 sysname;
    SELECT @var15 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_STOCK_IMAGE]') AND [c].[name] = N'CreatedDate');
    IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [RII_STOCK_IMAGE] DROP CONSTRAINT [' + @var15 + '];');
    ALTER TABLE [RII_STOCK_IMAGE] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var16 sysname;
    SELECT @var16 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_STOCK_DETAIL]') AND [c].[name] = N'CreatedDate');
    IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [RII_STOCK_DETAIL] DROP CONSTRAINT [' + @var16 + '];');
    ALTER TABLE [RII_STOCK_DETAIL] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var17 sysname;
    SELECT @var17 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_STOCK]') AND [c].[name] = N'CreatedDate');
    IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [RII_STOCK] DROP CONSTRAINT [' + @var17 + '];');
    ALTER TABLE [RII_STOCK] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var18 sysname;
    SELECT @var18 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_SHIPPING_ADDRESS]') AND [c].[name] = N'CreatedDate');
    IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [RII_SHIPPING_ADDRESS] DROP CONSTRAINT [' + @var18 + '];');
    ALTER TABLE [RII_SHIPPING_ADDRESS] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var19 sysname;
    SELECT @var19 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_SALES_TYPE_DEFINITION]') AND [c].[name] = N'CreatedDate');
    IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [RII_SALES_TYPE_DEFINITION] DROP CONSTRAINT [' + @var19 + '];');
    ALTER TABLE [RII_SALES_TYPE_DEFINITION] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var20 sysname;
    SELECT @var20 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_REPORT_DEFINITIONS]') AND [c].[name] = N'CreatedDate');
    IF @var20 IS NOT NULL EXEC(N'ALTER TABLE [RII_REPORT_DEFINITIONS] DROP CONSTRAINT [' + @var20 + '];');
    ALTER TABLE [RII_REPORT_DEFINITIONS] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var21 sysname;
    SELECT @var21 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_QUOTATION_NOTES]') AND [c].[name] = N'CreatedDate');
    IF @var21 IS NOT NULL EXEC(N'ALTER TABLE [RII_QUOTATION_NOTES] DROP CONSTRAINT [' + @var21 + '];');
    ALTER TABLE [RII_QUOTATION_NOTES] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var22 sysname;
    SELECT @var22 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_QUOTATION_LINE]') AND [c].[name] = N'CreatedDate');
    IF @var22 IS NOT NULL EXEC(N'ALTER TABLE [RII_QUOTATION_LINE] DROP CONSTRAINT [' + @var22 + '];');
    ALTER TABLE [RII_QUOTATION_LINE] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var23 sysname;
    SELECT @var23 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_QUOTATION_EXCHANGE_RATE]') AND [c].[name] = N'CreatedDate');
    IF @var23 IS NOT NULL EXEC(N'ALTER TABLE [RII_QUOTATION_EXCHANGE_RATE] DROP CONSTRAINT [' + @var23 + '];');
    ALTER TABLE [RII_QUOTATION_EXCHANGE_RATE] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var24 sysname;
    SELECT @var24 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_QUOTATION]') AND [c].[name] = N'CreatedDate');
    IF @var24 IS NOT NULL EXEC(N'ALTER TABLE [RII_QUOTATION] DROP CONSTRAINT [' + @var24 + '];');
    ALTER TABLE [RII_QUOTATION] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var25 sysname;
    SELECT @var25 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_PRODUCT_PRICING_GROUP_BY]') AND [c].[name] = N'CreatedDate');
    IF @var25 IS NOT NULL EXEC(N'ALTER TABLE [RII_PRODUCT_PRICING_GROUP_BY] DROP CONSTRAINT [' + @var25 + '];');
    ALTER TABLE [RII_PRODUCT_PRICING_GROUP_BY] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var26 sysname;
    SELECT @var26 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_PRODUCT_PRICING]') AND [c].[name] = N'CreatedDate');
    IF @var26 IS NOT NULL EXEC(N'ALTER TABLE [RII_PRODUCT_PRICING] DROP CONSTRAINT [' + @var26 + '];');
    ALTER TABLE [RII_PRODUCT_PRICING] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var27 sysname;
    SELECT @var27 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_PRICING_RULE_SALESMAN]') AND [c].[name] = N'CreatedDate');
    IF @var27 IS NOT NULL EXEC(N'ALTER TABLE [RII_PRICING_RULE_SALESMAN] DROP CONSTRAINT [' + @var27 + '];');
    ALTER TABLE [RII_PRICING_RULE_SALESMAN] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var28 sysname;
    SELECT @var28 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_PRICING_RULE_LINE]') AND [c].[name] = N'CreatedDate');
    IF @var28 IS NOT NULL EXEC(N'ALTER TABLE [RII_PRICING_RULE_LINE] DROP CONSTRAINT [' + @var28 + '];');
    ALTER TABLE [RII_PRICING_RULE_LINE] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var29 sysname;
    SELECT @var29 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_PRICING_RULE_HEADER]') AND [c].[name] = N'CreatedDate');
    IF @var29 IS NOT NULL EXEC(N'ALTER TABLE [RII_PRICING_RULE_HEADER] DROP CONSTRAINT [' + @var29 + '];');
    ALTER TABLE [RII_PRICING_RULE_HEADER] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var30 sysname;
    SELECT @var30 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_POWERBI_REPORT_ROLE_MAPPINGS]') AND [c].[name] = N'CreatedDate');
    IF @var30 IS NOT NULL EXEC(N'ALTER TABLE [RII_POWERBI_REPORT_ROLE_MAPPINGS] DROP CONSTRAINT [' + @var30 + '];');
    ALTER TABLE [RII_POWERBI_REPORT_ROLE_MAPPINGS] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var31 sysname;
    SELECT @var31 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_POWERBI_REPORT_DEFINITIONS]') AND [c].[name] = N'CreatedDate');
    IF @var31 IS NOT NULL EXEC(N'ALTER TABLE [RII_POWERBI_REPORT_DEFINITIONS] DROP CONSTRAINT [' + @var31 + '];');
    ALTER TABLE [RII_POWERBI_REPORT_DEFINITIONS] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var32 sysname;
    SELECT @var32 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_POWERBI_GROUPS]') AND [c].[name] = N'CreatedDate');
    IF @var32 IS NOT NULL EXEC(N'ALTER TABLE [RII_POWERBI_GROUPS] DROP CONSTRAINT [' + @var32 + '];');
    ALTER TABLE [RII_POWERBI_GROUPS] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var33 sysname;
    SELECT @var33 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_POWERBI_GROUP_REPORT_DEFINITIONS]') AND [c].[name] = N'CreatedDate');
    IF @var33 IS NOT NULL EXEC(N'ALTER TABLE [RII_POWERBI_GROUP_REPORT_DEFINITIONS] DROP CONSTRAINT [' + @var33 + '];');
    ALTER TABLE [RII_POWERBI_GROUP_REPORT_DEFINITIONS] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var34 sysname;
    SELECT @var34 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_POWERBI_CONFIGURATION]') AND [c].[name] = N'CreatedDate');
    IF @var34 IS NOT NULL EXEC(N'ALTER TABLE [RII_POWERBI_CONFIGURATION] DROP CONSTRAINT [' + @var34 + '];');
    ALTER TABLE [RII_POWERBI_CONFIGURATION] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var35 sysname;
    SELECT @var35 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_PERMISSION_GROUPS]') AND [c].[name] = N'CreatedDate');
    IF @var35 IS NOT NULL EXEC(N'ALTER TABLE [RII_PERMISSION_GROUPS] DROP CONSTRAINT [' + @var35 + '];');
    ALTER TABLE [RII_PERMISSION_GROUPS] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var36 sysname;
    SELECT @var36 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_PERMISSION_GROUP_PERMISSIONS]') AND [c].[name] = N'CreatedDate');
    IF @var36 IS NOT NULL EXEC(N'ALTER TABLE [RII_PERMISSION_GROUP_PERMISSIONS] DROP CONSTRAINT [' + @var36 + '];');
    ALTER TABLE [RII_PERMISSION_GROUP_PERMISSIONS] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var37 sysname;
    SELECT @var37 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_PERMISSION_DEFINITIONS]') AND [c].[name] = N'CreatedDate');
    IF @var37 IS NOT NULL EXEC(N'ALTER TABLE [RII_PERMISSION_DEFINITIONS] DROP CONSTRAINT [' + @var37 + '];');
    ALTER TABLE [RII_PERMISSION_DEFINITIONS] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var38 sysname;
    SELECT @var38 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_PAYMENT_TYPE]') AND [c].[name] = N'CreatedDate');
    IF @var38 IS NOT NULL EXEC(N'ALTER TABLE [RII_PAYMENT_TYPE] DROP CONSTRAINT [' + @var38 + '];');
    ALTER TABLE [RII_PAYMENT_TYPE] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var39 sysname;
    SELECT @var39 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_PASSWORD_RESET_REQUEST]') AND [c].[name] = N'CreatedDate');
    IF @var39 IS NOT NULL EXEC(N'ALTER TABLE [RII_PASSWORD_RESET_REQUEST] DROP CONSTRAINT [' + @var39 + '];');
    ALTER TABLE [RII_PASSWORD_RESET_REQUEST] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var40 sysname;
    SELECT @var40 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_OUTLOOK_INTEGRATION_LOGS]') AND [c].[name] = N'CreatedDate');
    IF @var40 IS NOT NULL EXEC(N'ALTER TABLE [RII_OUTLOOK_INTEGRATION_LOGS] DROP CONSTRAINT [' + @var40 + '];');
    ALTER TABLE [RII_OUTLOOK_INTEGRATION_LOGS] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var41 sysname;
    SELECT @var41 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_OUTLOOK_CUSTOMER_MAIL_LOGS]') AND [c].[name] = N'CreatedDate');
    IF @var41 IS NOT NULL EXEC(N'ALTER TABLE [RII_OUTLOOK_CUSTOMER_MAIL_LOGS] DROP CONSTRAINT [' + @var41 + '];');
    ALTER TABLE [RII_OUTLOOK_CUSTOMER_MAIL_LOGS] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var42 sysname;
    SELECT @var42 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_ORDER_NOTES]') AND [c].[name] = N'CreatedDate');
    IF @var42 IS NOT NULL EXEC(N'ALTER TABLE [RII_ORDER_NOTES] DROP CONSTRAINT [' + @var42 + '];');
    ALTER TABLE [RII_ORDER_NOTES] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var43 sysname;
    SELECT @var43 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_ORDER_LINE]') AND [c].[name] = N'CreatedDate');
    IF @var43 IS NOT NULL EXEC(N'ALTER TABLE [RII_ORDER_LINE] DROP CONSTRAINT [' + @var43 + '];');
    ALTER TABLE [RII_ORDER_LINE] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var44 sysname;
    SELECT @var44 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_ORDER_EXCHANGE_RATE]') AND [c].[name] = N'CreatedDate');
    IF @var44 IS NOT NULL EXEC(N'ALTER TABLE [RII_ORDER_EXCHANGE_RATE] DROP CONSTRAINT [' + @var44 + '];');
    ALTER TABLE [RII_ORDER_EXCHANGE_RATE] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var45 sysname;
    SELECT @var45 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_ORDER]') AND [c].[name] = N'CreatedDate');
    IF @var45 IS NOT NULL EXEC(N'ALTER TABLE [RII_ORDER] DROP CONSTRAINT [' + @var45 + '];');
    ALTER TABLE [RII_ORDER] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var46 sysname;
    SELECT @var46 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_JOB_FAILURE_LOG]') AND [c].[name] = N'CreatedDate');
    IF @var46 IS NOT NULL EXEC(N'ALTER TABLE [RII_JOB_FAILURE_LOG] DROP CONSTRAINT [' + @var46 + '];');
    ALTER TABLE [RII_JOB_FAILURE_LOG] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var47 sysname;
    SELECT @var47 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_GOOGLE_INTEGRATION_LOGS]') AND [c].[name] = N'CreatedDate');
    IF @var47 IS NOT NULL EXEC(N'ALTER TABLE [RII_GOOGLE_INTEGRATION_LOGS] DROP CONSTRAINT [' + @var47 + '];');
    ALTER TABLE [RII_GOOGLE_INTEGRATION_LOGS] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var48 sysname;
    SELECT @var48 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_GOOGLE_CUSTOMER_MAIL_LOGS]') AND [c].[name] = N'CreatedDate');
    IF @var48 IS NOT NULL EXEC(N'ALTER TABLE [RII_GOOGLE_CUSTOMER_MAIL_LOGS] DROP CONSTRAINT [' + @var48 + '];');
    ALTER TABLE [RII_GOOGLE_CUSTOMER_MAIL_LOGS] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var49 sysname;
    SELECT @var49 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_DOCUMENT_SERIAL_TYPE]') AND [c].[name] = N'CreatedDate');
    IF @var49 IS NOT NULL EXEC(N'ALTER TABLE [RII_DOCUMENT_SERIAL_TYPE] DROP CONSTRAINT [' + @var49 + '];');
    ALTER TABLE [RII_DOCUMENT_SERIAL_TYPE] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var50 sysname;
    SELECT @var50 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_DISTRICT]') AND [c].[name] = N'CreatedDate');
    IF @var50 IS NOT NULL EXEC(N'ALTER TABLE [RII_DISTRICT] DROP CONSTRAINT [' + @var50 + '];');
    ALTER TABLE [RII_DISTRICT] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var51 sysname;
    SELECT @var51 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_DEMAND_NOTES]') AND [c].[name] = N'CreatedDate');
    IF @var51 IS NOT NULL EXEC(N'ALTER TABLE [RII_DEMAND_NOTES] DROP CONSTRAINT [' + @var51 + '];');
    ALTER TABLE [RII_DEMAND_NOTES] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var52 sysname;
    SELECT @var52 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_DEMAND_LINE]') AND [c].[name] = N'CreatedDate');
    IF @var52 IS NOT NULL EXEC(N'ALTER TABLE [RII_DEMAND_LINE] DROP CONSTRAINT [' + @var52 + '];');
    ALTER TABLE [RII_DEMAND_LINE] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var53 sysname;
    SELECT @var53 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_DEMAND_EXCHANGE_RATE]') AND [c].[name] = N'CreatedDate');
    IF @var53 IS NOT NULL EXEC(N'ALTER TABLE [RII_DEMAND_EXCHANGE_RATE] DROP CONSTRAINT [' + @var53 + '];');
    ALTER TABLE [RII_DEMAND_EXCHANGE_RATE] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var54 sysname;
    SELECT @var54 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_DEMAND]') AND [c].[name] = N'CreatedDate');
    IF @var54 IS NOT NULL EXEC(N'ALTER TABLE [RII_DEMAND] DROP CONSTRAINT [' + @var54 + '];');
    ALTER TABLE [RII_DEMAND] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var55 sysname;
    SELECT @var55 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_CUSTOMER_TYPE]') AND [c].[name] = N'CreatedDate');
    IF @var55 IS NOT NULL EXEC(N'ALTER TABLE [RII_CUSTOMER_TYPE] DROP CONSTRAINT [' + @var55 + '];');
    ALTER TABLE [RII_CUSTOMER_TYPE] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var56 sysname;
    SELECT @var56 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_CUSTOMER_IMAGE]') AND [c].[name] = N'CreatedDate');
    IF @var56 IS NOT NULL EXEC(N'ALTER TABLE [RII_CUSTOMER_IMAGE] DROP CONSTRAINT [' + @var56 + '];');
    ALTER TABLE [RII_CUSTOMER_IMAGE] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var57 sysname;
    SELECT @var57 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_CUSTOMER]') AND [c].[name] = N'CreatedDate');
    IF @var57 IS NOT NULL EXEC(N'ALTER TABLE [RII_CUSTOMER] DROP CONSTRAINT [' + @var57 + '];');
    ALTER TABLE [RII_CUSTOMER] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var58 sysname;
    SELECT @var58 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_COUNTRY]') AND [c].[name] = N'CreatedDate');
    IF @var58 IS NOT NULL EXEC(N'ALTER TABLE [RII_COUNTRY] DROP CONSTRAINT [' + @var58 + '];');
    ALTER TABLE [RII_COUNTRY] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var59 sysname;
    SELECT @var59 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_CONTACT]') AND [c].[name] = N'CreatedDate');
    IF @var59 IS NOT NULL EXEC(N'ALTER TABLE [RII_CONTACT] DROP CONSTRAINT [' + @var59 + '];');
    ALTER TABLE [RII_CONTACT] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var60 sysname;
    SELECT @var60 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_CITY]') AND [c].[name] = N'CreatedDate');
    IF @var60 IS NOT NULL EXEC(N'ALTER TABLE [RII_CITY] DROP CONSTRAINT [' + @var60 + '];');
    ALTER TABLE [RII_CITY] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var61 sysname;
    SELECT @var61 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_APPROVAL_USER_ROLE]') AND [c].[name] = N'CreatedDate');
    IF @var61 IS NOT NULL EXEC(N'ALTER TABLE [RII_APPROVAL_USER_ROLE] DROP CONSTRAINT [' + @var61 + '];');
    ALTER TABLE [RII_APPROVAL_USER_ROLE] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var62 sysname;
    SELECT @var62 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_APPROVAL_ROLE_GROUP]') AND [c].[name] = N'CreatedDate');
    IF @var62 IS NOT NULL EXEC(N'ALTER TABLE [RII_APPROVAL_ROLE_GROUP] DROP CONSTRAINT [' + @var62 + '];');
    ALTER TABLE [RII_APPROVAL_ROLE_GROUP] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var63 sysname;
    SELECT @var63 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_APPROVAL_ROLE]') AND [c].[name] = N'CreatedDate');
    IF @var63 IS NOT NULL EXEC(N'ALTER TABLE [RII_APPROVAL_ROLE] DROP CONSTRAINT [' + @var63 + '];');
    ALTER TABLE [RII_APPROVAL_ROLE] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var64 sysname;
    SELECT @var64 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_APPROVAL_REQUEST]') AND [c].[name] = N'CreatedDate');
    IF @var64 IS NOT NULL EXEC(N'ALTER TABLE [RII_APPROVAL_REQUEST] DROP CONSTRAINT [' + @var64 + '];');
    ALTER TABLE [RII_APPROVAL_REQUEST] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var65 sysname;
    SELECT @var65 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_APPROVAL_FLOW_STEP]') AND [c].[name] = N'CreatedDate');
    IF @var65 IS NOT NULL EXEC(N'ALTER TABLE [RII_APPROVAL_FLOW_STEP] DROP CONSTRAINT [' + @var65 + '];');
    ALTER TABLE [RII_APPROVAL_FLOW_STEP] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var66 sysname;
    SELECT @var66 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_APPROVAL_FLOW]') AND [c].[name] = N'CreatedDate');
    IF @var66 IS NOT NULL EXEC(N'ALTER TABLE [RII_APPROVAL_FLOW] DROP CONSTRAINT [' + @var66 + '];');
    ALTER TABLE [RII_APPROVAL_FLOW] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var67 sysname;
    SELECT @var67 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_APPROVAL_ACTION]') AND [c].[name] = N'CreatedDate');
    IF @var67 IS NOT NULL EXEC(N'ALTER TABLE [RII_APPROVAL_ACTION] DROP CONSTRAINT [' + @var67 + '];');
    ALTER TABLE [RII_APPROVAL_ACTION] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var68 sysname;
    SELECT @var68 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_ACTIVITY_TYPE]') AND [c].[name] = N'CreatedDate');
    IF @var68 IS NOT NULL EXEC(N'ALTER TABLE [RII_ACTIVITY_TYPE] DROP CONSTRAINT [' + @var68 + '];');
    ALTER TABLE [RII_ACTIVITY_TYPE] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var69 sysname;
    SELECT @var69 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_ACTIVITY_REMINDER]') AND [c].[name] = N'CreatedDate');
    IF @var69 IS NOT NULL EXEC(N'ALTER TABLE [RII_ACTIVITY_REMINDER] DROP CONSTRAINT [' + @var69 + '];');
    ALTER TABLE [RII_ACTIVITY_REMINDER] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var70 sysname;
    SELECT @var70 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_ACTIVITY_IMAGE]') AND [c].[name] = N'CreatedDate');
    IF @var70 IS NOT NULL EXEC(N'ALTER TABLE [RII_ACTIVITY_IMAGE] DROP CONSTRAINT [' + @var70 + '];');
    ALTER TABLE [RII_ACTIVITY_IMAGE] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var71 sysname;
    SELECT @var71 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_ACTIVITY]') AND [c].[name] = N'CreatedDate');
    IF @var71 IS NOT NULL EXEC(N'ALTER TABLE [RII_ACTIVITY] DROP CONSTRAINT [' + @var71 + '];');
    ALTER TABLE [RII_ACTIVITY] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    DECLARE @var72 sysname;
    SELECT @var72 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Notifications]') AND [c].[name] = N'CreatedDate');
    IF @var72 IS NOT NULL EXEC(N'ALTER TABLE [Notifications] DROP CONSTRAINT [' + @var72 + '];');
    ALTER TABLE [Notifications] ADD DEFAULT (GETDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260312224450_UseLocalTimeForAuditDates'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260312224450_UseLocalTimeForAuditDates', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260315133023_AddTempQuotationExchangeLineUniqueIndex'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_RII_TEMP_QUOTATTION_EXCHANGE_LINE_TempQuotattionId_Currency] ON [RII_TEMP_QUOTATTION_EXCHANGE_LINE] ([TempQuotattionId], [Currency]) WHERE [IsDeleted] = 0');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260315133023_AddTempQuotationExchangeLineUniqueIndex'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260315133023_AddTempQuotationExchangeLineUniqueIndex', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260318130409_AddPdfTablePresets'
)
BEGIN
    CREATE TABLE [RII_PDF_TABLE_PRESETS] (
        [Id] bigint NOT NULL IDENTITY,
        [RuleType] int NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [Key] nvarchar(120) NOT NULL,
        [ColumnsJson] nvarchar(max) NOT NULL,
        [TableOptionsJson] nvarchar(max) NULL,
        [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedDate] datetime2 NOT NULL,
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [CreatedByUserId] bigint NULL,
        [UpdatedBy] bigint NULL,
        [UpdatedByUserId] bigint NULL,
        [DeletedBy] bigint NULL,
        [DeletedByUserId] bigint NULL,
        CONSTRAINT [PK_RII_PDF_TABLE_PRESETS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_PDF_TABLE_PRESETS_RII_USERS_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_PDF_TABLE_PRESETS_RII_USERS_DeletedByUserId] FOREIGN KEY ([DeletedByUserId]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_PDF_TABLE_PRESETS_RII_USERS_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260318130409_AddPdfTablePresets'
)
BEGIN
    CREATE INDEX [IX_RII_PDF_TABLE_PRESETS_CreatedByUserId] ON [RII_PDF_TABLE_PRESETS] ([CreatedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260318130409_AddPdfTablePresets'
)
BEGIN
    CREATE INDEX [IX_RII_PDF_TABLE_PRESETS_DeletedByUserId] ON [RII_PDF_TABLE_PRESETS] ([DeletedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260318130409_AddPdfTablePresets'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_RII_PDF_TABLE_PRESETS_Key] ON [RII_PDF_TABLE_PRESETS] ([Key]) WHERE [IsDeleted] = 0');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260318130409_AddPdfTablePresets'
)
BEGIN
    CREATE INDEX [IX_RII_PDF_TABLE_PRESETS_RuleType_IsActive] ON [RII_PDF_TABLE_PRESETS] ([RuleType], [IsActive]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260318130409_AddPdfTablePresets'
)
BEGIN
    CREATE INDEX [IX_RII_PDF_TABLE_PRESETS_UpdatedByUserId] ON [RII_PDF_TABLE_PRESETS] ([UpdatedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260318130409_AddPdfTablePresets'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260318130409_AddPdfTablePresets', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260319183045_PdfImage'
)
BEGIN
    CREATE TABLE [RII_PDF_TEMPLATE_ASSETS] (
        [Id] bigint NOT NULL IDENTITY,
        [OriginalFileName] nvarchar(260) NOT NULL,
        [StoredFileName] nvarchar(260) NOT NULL,
        [RelativeUrl] nvarchar(500) NOT NULL,
        [ContentType] nvarchar(120) NOT NULL,
        [SizeBytes] bigint NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [CreatedByUserId] bigint NULL,
        [UpdatedBy] bigint NULL,
        [UpdatedByUserId] bigint NULL,
        [DeletedBy] bigint NULL,
        [DeletedByUserId] bigint NULL,
        CONSTRAINT [PK_RII_PDF_TEMPLATE_ASSETS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_PDF_TEMPLATE_ASSETS_RII_USERS_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_PDF_TEMPLATE_ASSETS_RII_USERS_DeletedByUserId] FOREIGN KEY ([DeletedByUserId]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_PDF_TEMPLATE_ASSETS_RII_USERS_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260319183045_PdfImage'
)
BEGIN
    CREATE INDEX [IX_RII_PDF_TEMPLATE_ASSETS_CreatedBy] ON [RII_PDF_TEMPLATE_ASSETS] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260319183045_PdfImage'
)
BEGIN
    CREATE INDEX [IX_RII_PDF_TEMPLATE_ASSETS_CreatedByUserId] ON [RII_PDF_TEMPLATE_ASSETS] ([CreatedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260319183045_PdfImage'
)
BEGIN
    CREATE INDEX [IX_RII_PDF_TEMPLATE_ASSETS_DeletedByUserId] ON [RII_PDF_TEMPLATE_ASSETS] ([DeletedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260319183045_PdfImage'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_RII_PDF_TEMPLATE_ASSETS_RelativeUrl] ON [RII_PDF_TEMPLATE_ASSETS] ([RelativeUrl]) WHERE [IsDeleted] = 0');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260319183045_PdfImage'
)
BEGIN
    CREATE INDEX [IX_RII_PDF_TEMPLATE_ASSETS_UpdatedByUserId] ON [RII_PDF_TEMPLATE_ASSETS] ([UpdatedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260319183045_PdfImage'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260319183045_PdfImage', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322112106_AddFastQuotationLineImagePath'
)
BEGIN
    ALTER TABLE [RII_TEMP_QUOTATTION_LINE] ADD [ImagePath] nvarchar(500) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260322112106_AddFastQuotationLineImagePath'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260322112106_AddFastQuotationLineImagePath', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260324084027_AddReportAssignments'
)
BEGIN
    CREATE TABLE [RII_REPORT_ASSIGNMENTS] (
        [Id] bigint NOT NULL IDENTITY,
        [ReportDefinitionId] bigint NOT NULL,
        [UserId] bigint NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_REPORT_ASSIGNMENTS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_REPORT_ASSIGNMENTS_RII_REPORT_DEFINITIONS_ReportDefinitionId] FOREIGN KEY ([ReportDefinitionId]) REFERENCES [RII_REPORT_DEFINITIONS] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RII_REPORT_ASSIGNMENTS_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_REPORT_ASSIGNMENTS_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_REPORT_ASSIGNMENTS_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_REPORT_ASSIGNMENTS_RII_USERS_UserId] FOREIGN KEY ([UserId]) REFERENCES [RII_USERS] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260324084027_AddReportAssignments'
)
BEGIN
    CREATE INDEX [IX_RII_REPORT_ASSIGNMENTS_CreatedBy] ON [RII_REPORT_ASSIGNMENTS] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260324084027_AddReportAssignments'
)
BEGIN
    CREATE INDEX [IX_RII_REPORT_ASSIGNMENTS_DeletedBy] ON [RII_REPORT_ASSIGNMENTS] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260324084027_AddReportAssignments'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_RII_REPORT_ASSIGNMENTS_ReportDefinitionId_UserId] ON [RII_REPORT_ASSIGNMENTS] ([ReportDefinitionId], [UserId]) WHERE [IsDeleted] = 0');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260324084027_AddReportAssignments'
)
BEGIN
    CREATE INDEX [IX_RII_REPORT_ASSIGNMENTS_UpdatedBy] ON [RII_REPORT_ASSIGNMENTS] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260324084027_AddReportAssignments'
)
BEGIN
    CREATE INDEX [IX_RII_REPORT_ASSIGNMENTS_UserId] ON [RII_REPORT_ASSIGNMENTS] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260324084027_AddReportAssignments'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260324084027_AddReportAssignments', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    DECLARE @var73 sysname;
    SELECT @var73 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_ACTIVITY]') AND [c].[name] = N'Description');
    IF @var73 IS NOT NULL EXEC(N'ALTER TABLE [RII_ACTIVITY] DROP CONSTRAINT [' + @var73 + '];');
    ALTER TABLE [RII_ACTIVITY] ALTER COLUMN [Description] nvarchar(2000) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    ALTER TABLE [RII_ACTIVITY] ADD [ActivityMeetingTypeId] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    ALTER TABLE [RII_ACTIVITY] ADD [ActivityShippingId] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    ALTER TABLE [RII_ACTIVITY] ADD [ActivityTopicPurposeId] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    ALTER TABLE [RII_ACTIVITY] ADD [PaymentTypeId] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE TABLE [RII_ACTIVITY_MEETING_TYPE] (
        [Id] bigint NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_ACTIVITY_MEETING_TYPE] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_ACTIVITY_MEETING_TYPE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_ACTIVITY_MEETING_TYPE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_ACTIVITY_MEETING_TYPE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE TABLE [RII_ACTIVITY_SHIPPING] (
        [Id] bigint NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_ACTIVITY_SHIPPING] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_ACTIVITY_SHIPPING_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_ACTIVITY_SHIPPING_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_ACTIVITY_SHIPPING_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE TABLE [RII_ACTIVITY_TOPIC_PURPOSE] (
        [Id] bigint NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_ACTIVITY_TOPIC_PURPOSE] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_ACTIVITY_TOPIC_PURPOSE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_ACTIVITY_TOPIC_PURPOSE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_ACTIVITY_TOPIC_PURPOSE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_Activity_ActivityMeetingTypeId] ON [RII_ACTIVITY] ([ActivityMeetingTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_Activity_ActivityShippingId] ON [RII_ACTIVITY] ([ActivityShippingId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_Activity_ActivityTopicPurposeId] ON [RII_ACTIVITY] ([ActivityTopicPurposeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_Activity_PaymentTypeId] ON [RII_ACTIVITY] ([PaymentTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_ActivityMeetingType_CreatedDate] ON [RII_ACTIVITY_MEETING_TYPE] ([CreatedDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_ActivityMeetingType_IsDeleted] ON [RII_ACTIVITY_MEETING_TYPE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_ActivityMeetingType_Name] ON [RII_ACTIVITY_MEETING_TYPE] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_RII_ACTIVITY_MEETING_TYPE_CreatedBy] ON [RII_ACTIVITY_MEETING_TYPE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_RII_ACTIVITY_MEETING_TYPE_DeletedBy] ON [RII_ACTIVITY_MEETING_TYPE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_RII_ACTIVITY_MEETING_TYPE_UpdatedBy] ON [RII_ACTIVITY_MEETING_TYPE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_ActivityShipping_CreatedDate] ON [RII_ACTIVITY_SHIPPING] ([CreatedDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_ActivityShipping_IsDeleted] ON [RII_ACTIVITY_SHIPPING] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_ActivityShipping_Name] ON [RII_ACTIVITY_SHIPPING] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_RII_ACTIVITY_SHIPPING_CreatedBy] ON [RII_ACTIVITY_SHIPPING] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_RII_ACTIVITY_SHIPPING_DeletedBy] ON [RII_ACTIVITY_SHIPPING] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_RII_ACTIVITY_SHIPPING_UpdatedBy] ON [RII_ACTIVITY_SHIPPING] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_ActivityTopicPurpose_CreatedDate] ON [RII_ACTIVITY_TOPIC_PURPOSE] ([CreatedDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_ActivityTopicPurpose_IsDeleted] ON [RII_ACTIVITY_TOPIC_PURPOSE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_ActivityTopicPurpose_Name] ON [RII_ACTIVITY_TOPIC_PURPOSE] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_RII_ACTIVITY_TOPIC_PURPOSE_CreatedBy] ON [RII_ACTIVITY_TOPIC_PURPOSE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_RII_ACTIVITY_TOPIC_PURPOSE_DeletedBy] ON [RII_ACTIVITY_TOPIC_PURPOSE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    CREATE INDEX [IX_RII_ACTIVITY_TOPIC_PURPOSE_UpdatedBy] ON [RII_ACTIVITY_TOPIC_PURPOSE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    ALTER TABLE [RII_ACTIVITY] ADD CONSTRAINT [FK_RII_ACTIVITY_RII_ACTIVITY_MEETING_TYPE_ActivityMeetingTypeId] FOREIGN KEY ([ActivityMeetingTypeId]) REFERENCES [RII_ACTIVITY_MEETING_TYPE] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    ALTER TABLE [RII_ACTIVITY] ADD CONSTRAINT [FK_RII_ACTIVITY_RII_ACTIVITY_SHIPPING_ActivityShippingId] FOREIGN KEY ([ActivityShippingId]) REFERENCES [RII_ACTIVITY_SHIPPING] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    ALTER TABLE [RII_ACTIVITY] ADD CONSTRAINT [FK_RII_ACTIVITY_RII_ACTIVITY_TOPIC_PURPOSE_ActivityTopicPurposeId] FOREIGN KEY ([ActivityTopicPurposeId]) REFERENCES [RII_ACTIVITY_TOPIC_PURPOSE] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    ALTER TABLE [RII_ACTIVITY] ADD CONSTRAINT [FK_RII_ACTIVITY_RII_PAYMENT_TYPE_PaymentTypeId] FOREIGN KEY ([PaymentTypeId]) REFERENCES [RII_PAYMENT_TYPE] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071813_AddActivityPaymentType'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260325071813_AddActivityPaymentType', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071842_IncreaseActivityDescriptionLength'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260325071842_IncreaseActivityDescriptionLength', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071910_CreateActivityMeetingType'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260325071910_CreateActivityMeetingType', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325071937_AddActivityMeetingTypeIdToActivity'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260325071937_AddActivityMeetingTypeIdToActivity', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325072002_CreateActivityTopicPurpose'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260325072002_CreateActivityTopicPurpose', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325072029_AddActivityTopicPurposeIdToActivity'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260325072029_AddActivityTopicPurposeIdToActivity', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325072055_CreateActivityShipping'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260325072055_CreateActivityShipping', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260325072125_AddActivityShippingIdToActivity'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260325072125_AddActivityShippingIdToActivity', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var74 sysname;
    SELECT @var74 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_USERS]') AND [c].[name] = N'CreatedDate');
    IF @var74 IS NOT NULL EXEC(N'ALTER TABLE [RII_USERS] DROP CONSTRAINT [' + @var74 + '];');
    ALTER TABLE [RII_USERS] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var75 sysname;
    SELECT @var75 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_USER_SESSION]') AND [c].[name] = N'CreatedDate');
    IF @var75 IS NOT NULL EXEC(N'ALTER TABLE [RII_USER_SESSION] DROP CONSTRAINT [' + @var75 + '];');
    ALTER TABLE [RII_USER_SESSION] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var76 sysname;
    SELECT @var76 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_USER_POWERBI_GROUPS]') AND [c].[name] = N'CreatedDate');
    IF @var76 IS NOT NULL EXEC(N'ALTER TABLE [RII_USER_POWERBI_GROUPS] DROP CONSTRAINT [' + @var76 + '];');
    ALTER TABLE [RII_USER_POWERBI_GROUPS] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var77 sysname;
    SELECT @var77 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_USER_PERMISSION_GROUPS]') AND [c].[name] = N'CreatedDate');
    IF @var77 IS NOT NULL EXEC(N'ALTER TABLE [RII_USER_PERMISSION_GROUPS] DROP CONSTRAINT [' + @var77 + '];');
    ALTER TABLE [RII_USER_PERMISSION_GROUPS] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var78 sysname;
    SELECT @var78 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_USER_DISCOUNT_LIMIT]') AND [c].[name] = N'CreatedDate');
    IF @var78 IS NOT NULL EXEC(N'ALTER TABLE [RII_USER_DISCOUNT_LIMIT] DROP CONSTRAINT [' + @var78 + '];');
    ALTER TABLE [RII_USER_DISCOUNT_LIMIT] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var79 sysname;
    SELECT @var79 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_USER_DETAIL]') AND [c].[name] = N'CreatedDate');
    IF @var79 IS NOT NULL EXEC(N'ALTER TABLE [RII_USER_DETAIL] DROP CONSTRAINT [' + @var79 + '];');
    ALTER TABLE [RII_USER_DETAIL] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var80 sysname;
    SELECT @var80 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_USER_AUTHORITY]') AND [c].[name] = N'CreatedDate');
    IF @var80 IS NOT NULL EXEC(N'ALTER TABLE [RII_USER_AUTHORITY] DROP CONSTRAINT [' + @var80 + '];');
    ALTER TABLE [RII_USER_AUTHORITY] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var81 sysname;
    SELECT @var81 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_TITLE]') AND [c].[name] = N'CreatedDate');
    IF @var81 IS NOT NULL EXEC(N'ALTER TABLE [RII_TITLE] DROP CONSTRAINT [' + @var81 + '];');
    ALTER TABLE [RII_TITLE] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var82 sysname;
    SELECT @var82 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_TEMP_QUOTATTION_LINE]') AND [c].[name] = N'CreatedDate');
    IF @var82 IS NOT NULL EXEC(N'ALTER TABLE [RII_TEMP_QUOTATTION_LINE] DROP CONSTRAINT [' + @var82 + '];');
    ALTER TABLE [RII_TEMP_QUOTATTION_LINE] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var83 sysname;
    SELECT @var83 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_TEMP_QUOTATTION_EXCHANGE_LINE]') AND [c].[name] = N'CreatedDate');
    IF @var83 IS NOT NULL EXEC(N'ALTER TABLE [RII_TEMP_QUOTATTION_EXCHANGE_LINE] DROP CONSTRAINT [' + @var83 + '];');
    ALTER TABLE [RII_TEMP_QUOTATTION_EXCHANGE_LINE] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var84 sysname;
    SELECT @var84 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_TEMP_QUOTATTION]') AND [c].[name] = N'CreatedDate');
    IF @var84 IS NOT NULL EXEC(N'ALTER TABLE [RII_TEMP_QUOTATTION] DROP CONSTRAINT [' + @var84 + '];');
    ALTER TABLE [RII_TEMP_QUOTATTION] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var85 sysname;
    SELECT @var85 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_STOCK_RELATION]') AND [c].[name] = N'CreatedDate');
    IF @var85 IS NOT NULL EXEC(N'ALTER TABLE [RII_STOCK_RELATION] DROP CONSTRAINT [' + @var85 + '];');
    ALTER TABLE [RII_STOCK_RELATION] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var86 sysname;
    SELECT @var86 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_STOCK_IMAGE]') AND [c].[name] = N'CreatedDate');
    IF @var86 IS NOT NULL EXEC(N'ALTER TABLE [RII_STOCK_IMAGE] DROP CONSTRAINT [' + @var86 + '];');
    ALTER TABLE [RII_STOCK_IMAGE] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var87 sysname;
    SELECT @var87 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_STOCK_DETAIL]') AND [c].[name] = N'CreatedDate');
    IF @var87 IS NOT NULL EXEC(N'ALTER TABLE [RII_STOCK_DETAIL] DROP CONSTRAINT [' + @var87 + '];');
    ALTER TABLE [RII_STOCK_DETAIL] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var88 sysname;
    SELECT @var88 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_STOCK]') AND [c].[name] = N'CreatedDate');
    IF @var88 IS NOT NULL EXEC(N'ALTER TABLE [RII_STOCK] DROP CONSTRAINT [' + @var88 + '];');
    ALTER TABLE [RII_STOCK] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var89 sysname;
    SELECT @var89 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_SHIPPING_ADDRESS]') AND [c].[name] = N'CreatedDate');
    IF @var89 IS NOT NULL EXEC(N'ALTER TABLE [RII_SHIPPING_ADDRESS] DROP CONSTRAINT [' + @var89 + '];');
    ALTER TABLE [RII_SHIPPING_ADDRESS] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var90 sysname;
    SELECT @var90 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_SALES_TYPE_DEFINITION]') AND [c].[name] = N'CreatedDate');
    IF @var90 IS NOT NULL EXEC(N'ALTER TABLE [RII_SALES_TYPE_DEFINITION] DROP CONSTRAINT [' + @var90 + '];');
    ALTER TABLE [RII_SALES_TYPE_DEFINITION] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var91 sysname;
    SELECT @var91 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_REPORT_DEFINITIONS]') AND [c].[name] = N'CreatedDate');
    IF @var91 IS NOT NULL EXEC(N'ALTER TABLE [RII_REPORT_DEFINITIONS] DROP CONSTRAINT [' + @var91 + '];');
    ALTER TABLE [RII_REPORT_DEFINITIONS] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var92 sysname;
    SELECT @var92 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_REPORT_ASSIGNMENTS]') AND [c].[name] = N'CreatedDate');
    IF @var92 IS NOT NULL EXEC(N'ALTER TABLE [RII_REPORT_ASSIGNMENTS] DROP CONSTRAINT [' + @var92 + '];');
    ALTER TABLE [RII_REPORT_ASSIGNMENTS] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var93 sysname;
    SELECT @var93 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_QUOTATION_NOTES]') AND [c].[name] = N'CreatedDate');
    IF @var93 IS NOT NULL EXEC(N'ALTER TABLE [RII_QUOTATION_NOTES] DROP CONSTRAINT [' + @var93 + '];');
    ALTER TABLE [RII_QUOTATION_NOTES] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var94 sysname;
    SELECT @var94 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_QUOTATION_LINE]') AND [c].[name] = N'CreatedDate');
    IF @var94 IS NOT NULL EXEC(N'ALTER TABLE [RII_QUOTATION_LINE] DROP CONSTRAINT [' + @var94 + '];');
    ALTER TABLE [RII_QUOTATION_LINE] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var95 sysname;
    SELECT @var95 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_QUOTATION_EXCHANGE_RATE]') AND [c].[name] = N'CreatedDate');
    IF @var95 IS NOT NULL EXEC(N'ALTER TABLE [RII_QUOTATION_EXCHANGE_RATE] DROP CONSTRAINT [' + @var95 + '];');
    ALTER TABLE [RII_QUOTATION_EXCHANGE_RATE] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var96 sysname;
    SELECT @var96 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_QUOTATION]') AND [c].[name] = N'CreatedDate');
    IF @var96 IS NOT NULL EXEC(N'ALTER TABLE [RII_QUOTATION] DROP CONSTRAINT [' + @var96 + '];');
    ALTER TABLE [RII_QUOTATION] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var97 sysname;
    SELECT @var97 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_PRODUCT_PRICING_GROUP_BY]') AND [c].[name] = N'CreatedDate');
    IF @var97 IS NOT NULL EXEC(N'ALTER TABLE [RII_PRODUCT_PRICING_GROUP_BY] DROP CONSTRAINT [' + @var97 + '];');
    ALTER TABLE [RII_PRODUCT_PRICING_GROUP_BY] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var98 sysname;
    SELECT @var98 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_PRODUCT_PRICING]') AND [c].[name] = N'CreatedDate');
    IF @var98 IS NOT NULL EXEC(N'ALTER TABLE [RII_PRODUCT_PRICING] DROP CONSTRAINT [' + @var98 + '];');
    ALTER TABLE [RII_PRODUCT_PRICING] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var99 sysname;
    SELECT @var99 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_PRICING_RULE_SALESMAN]') AND [c].[name] = N'CreatedDate');
    IF @var99 IS NOT NULL EXEC(N'ALTER TABLE [RII_PRICING_RULE_SALESMAN] DROP CONSTRAINT [' + @var99 + '];');
    ALTER TABLE [RII_PRICING_RULE_SALESMAN] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var100 sysname;
    SELECT @var100 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_PRICING_RULE_LINE]') AND [c].[name] = N'CreatedDate');
    IF @var100 IS NOT NULL EXEC(N'ALTER TABLE [RII_PRICING_RULE_LINE] DROP CONSTRAINT [' + @var100 + '];');
    ALTER TABLE [RII_PRICING_RULE_LINE] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var101 sysname;
    SELECT @var101 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_PRICING_RULE_HEADER]') AND [c].[name] = N'CreatedDate');
    IF @var101 IS NOT NULL EXEC(N'ALTER TABLE [RII_PRICING_RULE_HEADER] DROP CONSTRAINT [' + @var101 + '];');
    ALTER TABLE [RII_PRICING_RULE_HEADER] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var102 sysname;
    SELECT @var102 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_POWERBI_REPORT_ROLE_MAPPINGS]') AND [c].[name] = N'CreatedDate');
    IF @var102 IS NOT NULL EXEC(N'ALTER TABLE [RII_POWERBI_REPORT_ROLE_MAPPINGS] DROP CONSTRAINT [' + @var102 + '];');
    ALTER TABLE [RII_POWERBI_REPORT_ROLE_MAPPINGS] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var103 sysname;
    SELECT @var103 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_POWERBI_REPORT_DEFINITIONS]') AND [c].[name] = N'CreatedDate');
    IF @var103 IS NOT NULL EXEC(N'ALTER TABLE [RII_POWERBI_REPORT_DEFINITIONS] DROP CONSTRAINT [' + @var103 + '];');
    ALTER TABLE [RII_POWERBI_REPORT_DEFINITIONS] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var104 sysname;
    SELECT @var104 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_POWERBI_GROUPS]') AND [c].[name] = N'CreatedDate');
    IF @var104 IS NOT NULL EXEC(N'ALTER TABLE [RII_POWERBI_GROUPS] DROP CONSTRAINT [' + @var104 + '];');
    ALTER TABLE [RII_POWERBI_GROUPS] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var105 sysname;
    SELECT @var105 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_POWERBI_GROUP_REPORT_DEFINITIONS]') AND [c].[name] = N'CreatedDate');
    IF @var105 IS NOT NULL EXEC(N'ALTER TABLE [RII_POWERBI_GROUP_REPORT_DEFINITIONS] DROP CONSTRAINT [' + @var105 + '];');
    ALTER TABLE [RII_POWERBI_GROUP_REPORT_DEFINITIONS] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var106 sysname;
    SELECT @var106 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_POWERBI_CONFIGURATION]') AND [c].[name] = N'CreatedDate');
    IF @var106 IS NOT NULL EXEC(N'ALTER TABLE [RII_POWERBI_CONFIGURATION] DROP CONSTRAINT [' + @var106 + '];');
    ALTER TABLE [RII_POWERBI_CONFIGURATION] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var107 sysname;
    SELECT @var107 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_PERMISSION_GROUPS]') AND [c].[name] = N'CreatedDate');
    IF @var107 IS NOT NULL EXEC(N'ALTER TABLE [RII_PERMISSION_GROUPS] DROP CONSTRAINT [' + @var107 + '];');
    ALTER TABLE [RII_PERMISSION_GROUPS] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var108 sysname;
    SELECT @var108 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_PERMISSION_GROUP_PERMISSIONS]') AND [c].[name] = N'CreatedDate');
    IF @var108 IS NOT NULL EXEC(N'ALTER TABLE [RII_PERMISSION_GROUP_PERMISSIONS] DROP CONSTRAINT [' + @var108 + '];');
    ALTER TABLE [RII_PERMISSION_GROUP_PERMISSIONS] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var109 sysname;
    SELECT @var109 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_PERMISSION_DEFINITIONS]') AND [c].[name] = N'CreatedDate');
    IF @var109 IS NOT NULL EXEC(N'ALTER TABLE [RII_PERMISSION_DEFINITIONS] DROP CONSTRAINT [' + @var109 + '];');
    ALTER TABLE [RII_PERMISSION_DEFINITIONS] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var110 sysname;
    SELECT @var110 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_PAYMENT_TYPE]') AND [c].[name] = N'CreatedDate');
    IF @var110 IS NOT NULL EXEC(N'ALTER TABLE [RII_PAYMENT_TYPE] DROP CONSTRAINT [' + @var110 + '];');
    ALTER TABLE [RII_PAYMENT_TYPE] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var111 sysname;
    SELECT @var111 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_PASSWORD_RESET_REQUEST]') AND [c].[name] = N'CreatedDate');
    IF @var111 IS NOT NULL EXEC(N'ALTER TABLE [RII_PASSWORD_RESET_REQUEST] DROP CONSTRAINT [' + @var111 + '];');
    ALTER TABLE [RII_PASSWORD_RESET_REQUEST] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var112 sysname;
    SELECT @var112 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_OUTLOOK_INTEGRATION_LOGS]') AND [c].[name] = N'CreatedDate');
    IF @var112 IS NOT NULL EXEC(N'ALTER TABLE [RII_OUTLOOK_INTEGRATION_LOGS] DROP CONSTRAINT [' + @var112 + '];');
    ALTER TABLE [RII_OUTLOOK_INTEGRATION_LOGS] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var113 sysname;
    SELECT @var113 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_OUTLOOK_CUSTOMER_MAIL_LOGS]') AND [c].[name] = N'CreatedDate');
    IF @var113 IS NOT NULL EXEC(N'ALTER TABLE [RII_OUTLOOK_CUSTOMER_MAIL_LOGS] DROP CONSTRAINT [' + @var113 + '];');
    ALTER TABLE [RII_OUTLOOK_CUSTOMER_MAIL_LOGS] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var114 sysname;
    SELECT @var114 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_ORDER_NOTES]') AND [c].[name] = N'CreatedDate');
    IF @var114 IS NOT NULL EXEC(N'ALTER TABLE [RII_ORDER_NOTES] DROP CONSTRAINT [' + @var114 + '];');
    ALTER TABLE [RII_ORDER_NOTES] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var115 sysname;
    SELECT @var115 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_ORDER_LINE]') AND [c].[name] = N'CreatedDate');
    IF @var115 IS NOT NULL EXEC(N'ALTER TABLE [RII_ORDER_LINE] DROP CONSTRAINT [' + @var115 + '];');
    ALTER TABLE [RII_ORDER_LINE] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var116 sysname;
    SELECT @var116 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_ORDER_EXCHANGE_RATE]') AND [c].[name] = N'CreatedDate');
    IF @var116 IS NOT NULL EXEC(N'ALTER TABLE [RII_ORDER_EXCHANGE_RATE] DROP CONSTRAINT [' + @var116 + '];');
    ALTER TABLE [RII_ORDER_EXCHANGE_RATE] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var117 sysname;
    SELECT @var117 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_ORDER]') AND [c].[name] = N'CreatedDate');
    IF @var117 IS NOT NULL EXEC(N'ALTER TABLE [RII_ORDER] DROP CONSTRAINT [' + @var117 + '];');
    ALTER TABLE [RII_ORDER] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var118 sysname;
    SELECT @var118 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_JOB_FAILURE_LOG]') AND [c].[name] = N'CreatedDate');
    IF @var118 IS NOT NULL EXEC(N'ALTER TABLE [RII_JOB_FAILURE_LOG] DROP CONSTRAINT [' + @var118 + '];');
    ALTER TABLE [RII_JOB_FAILURE_LOG] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var119 sysname;
    SELECT @var119 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_GOOGLE_INTEGRATION_LOGS]') AND [c].[name] = N'CreatedDate');
    IF @var119 IS NOT NULL EXEC(N'ALTER TABLE [RII_GOOGLE_INTEGRATION_LOGS] DROP CONSTRAINT [' + @var119 + '];');
    ALTER TABLE [RII_GOOGLE_INTEGRATION_LOGS] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var120 sysname;
    SELECT @var120 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_GOOGLE_CUSTOMER_MAIL_LOGS]') AND [c].[name] = N'CreatedDate');
    IF @var120 IS NOT NULL EXEC(N'ALTER TABLE [RII_GOOGLE_CUSTOMER_MAIL_LOGS] DROP CONSTRAINT [' + @var120 + '];');
    ALTER TABLE [RII_GOOGLE_CUSTOMER_MAIL_LOGS] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var121 sysname;
    SELECT @var121 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_DOCUMENT_SERIAL_TYPE]') AND [c].[name] = N'CreatedDate');
    IF @var121 IS NOT NULL EXEC(N'ALTER TABLE [RII_DOCUMENT_SERIAL_TYPE] DROP CONSTRAINT [' + @var121 + '];');
    ALTER TABLE [RII_DOCUMENT_SERIAL_TYPE] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var122 sysname;
    SELECT @var122 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_DISTRICT]') AND [c].[name] = N'CreatedDate');
    IF @var122 IS NOT NULL EXEC(N'ALTER TABLE [RII_DISTRICT] DROP CONSTRAINT [' + @var122 + '];');
    ALTER TABLE [RII_DISTRICT] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var123 sysname;
    SELECT @var123 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_DEMAND_NOTES]') AND [c].[name] = N'CreatedDate');
    IF @var123 IS NOT NULL EXEC(N'ALTER TABLE [RII_DEMAND_NOTES] DROP CONSTRAINT [' + @var123 + '];');
    ALTER TABLE [RII_DEMAND_NOTES] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var124 sysname;
    SELECT @var124 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_DEMAND_LINE]') AND [c].[name] = N'CreatedDate');
    IF @var124 IS NOT NULL EXEC(N'ALTER TABLE [RII_DEMAND_LINE] DROP CONSTRAINT [' + @var124 + '];');
    ALTER TABLE [RII_DEMAND_LINE] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var125 sysname;
    SELECT @var125 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_DEMAND_EXCHANGE_RATE]') AND [c].[name] = N'CreatedDate');
    IF @var125 IS NOT NULL EXEC(N'ALTER TABLE [RII_DEMAND_EXCHANGE_RATE] DROP CONSTRAINT [' + @var125 + '];');
    ALTER TABLE [RII_DEMAND_EXCHANGE_RATE] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var126 sysname;
    SELECT @var126 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_DEMAND]') AND [c].[name] = N'CreatedDate');
    IF @var126 IS NOT NULL EXEC(N'ALTER TABLE [RII_DEMAND] DROP CONSTRAINT [' + @var126 + '];');
    ALTER TABLE [RII_DEMAND] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var127 sysname;
    SELECT @var127 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_CUSTOMER_TYPE]') AND [c].[name] = N'CreatedDate');
    IF @var127 IS NOT NULL EXEC(N'ALTER TABLE [RII_CUSTOMER_TYPE] DROP CONSTRAINT [' + @var127 + '];');
    ALTER TABLE [RII_CUSTOMER_TYPE] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var128 sysname;
    SELECT @var128 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_CUSTOMER_IMAGE]') AND [c].[name] = N'CreatedDate');
    IF @var128 IS NOT NULL EXEC(N'ALTER TABLE [RII_CUSTOMER_IMAGE] DROP CONSTRAINT [' + @var128 + '];');
    ALTER TABLE [RII_CUSTOMER_IMAGE] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var129 sysname;
    SELECT @var129 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_CUSTOMER]') AND [c].[name] = N'CreatedDate');
    IF @var129 IS NOT NULL EXEC(N'ALTER TABLE [RII_CUSTOMER] DROP CONSTRAINT [' + @var129 + '];');
    ALTER TABLE [RII_CUSTOMER] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var130 sysname;
    SELECT @var130 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_COUNTRY]') AND [c].[name] = N'CreatedDate');
    IF @var130 IS NOT NULL EXEC(N'ALTER TABLE [RII_COUNTRY] DROP CONSTRAINT [' + @var130 + '];');
    ALTER TABLE [RII_COUNTRY] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var131 sysname;
    SELECT @var131 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_CONTACT]') AND [c].[name] = N'CreatedDate');
    IF @var131 IS NOT NULL EXEC(N'ALTER TABLE [RII_CONTACT] DROP CONSTRAINT [' + @var131 + '];');
    ALTER TABLE [RII_CONTACT] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var132 sysname;
    SELECT @var132 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_CITY]') AND [c].[name] = N'CreatedDate');
    IF @var132 IS NOT NULL EXEC(N'ALTER TABLE [RII_CITY] DROP CONSTRAINT [' + @var132 + '];');
    ALTER TABLE [RII_CITY] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var133 sysname;
    SELECT @var133 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_APPROVAL_USER_ROLE]') AND [c].[name] = N'CreatedDate');
    IF @var133 IS NOT NULL EXEC(N'ALTER TABLE [RII_APPROVAL_USER_ROLE] DROP CONSTRAINT [' + @var133 + '];');
    ALTER TABLE [RII_APPROVAL_USER_ROLE] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var134 sysname;
    SELECT @var134 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_APPROVAL_ROLE_GROUP]') AND [c].[name] = N'CreatedDate');
    IF @var134 IS NOT NULL EXEC(N'ALTER TABLE [RII_APPROVAL_ROLE_GROUP] DROP CONSTRAINT [' + @var134 + '];');
    ALTER TABLE [RII_APPROVAL_ROLE_GROUP] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var135 sysname;
    SELECT @var135 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_APPROVAL_ROLE]') AND [c].[name] = N'CreatedDate');
    IF @var135 IS NOT NULL EXEC(N'ALTER TABLE [RII_APPROVAL_ROLE] DROP CONSTRAINT [' + @var135 + '];');
    ALTER TABLE [RII_APPROVAL_ROLE] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var136 sysname;
    SELECT @var136 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_APPROVAL_REQUEST]') AND [c].[name] = N'CreatedDate');
    IF @var136 IS NOT NULL EXEC(N'ALTER TABLE [RII_APPROVAL_REQUEST] DROP CONSTRAINT [' + @var136 + '];');
    ALTER TABLE [RII_APPROVAL_REQUEST] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var137 sysname;
    SELECT @var137 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_APPROVAL_FLOW_STEP]') AND [c].[name] = N'CreatedDate');
    IF @var137 IS NOT NULL EXEC(N'ALTER TABLE [RII_APPROVAL_FLOW_STEP] DROP CONSTRAINT [' + @var137 + '];');
    ALTER TABLE [RII_APPROVAL_FLOW_STEP] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var138 sysname;
    SELECT @var138 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_APPROVAL_FLOW]') AND [c].[name] = N'CreatedDate');
    IF @var138 IS NOT NULL EXEC(N'ALTER TABLE [RII_APPROVAL_FLOW] DROP CONSTRAINT [' + @var138 + '];');
    ALTER TABLE [RII_APPROVAL_FLOW] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var139 sysname;
    SELECT @var139 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_APPROVAL_ACTION]') AND [c].[name] = N'CreatedDate');
    IF @var139 IS NOT NULL EXEC(N'ALTER TABLE [RII_APPROVAL_ACTION] DROP CONSTRAINT [' + @var139 + '];');
    ALTER TABLE [RII_APPROVAL_ACTION] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var140 sysname;
    SELECT @var140 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_ACTIVITY_TYPE]') AND [c].[name] = N'CreatedDate');
    IF @var140 IS NOT NULL EXEC(N'ALTER TABLE [RII_ACTIVITY_TYPE] DROP CONSTRAINT [' + @var140 + '];');
    ALTER TABLE [RII_ACTIVITY_TYPE] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var141 sysname;
    SELECT @var141 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_ACTIVITY_TOPIC_PURPOSE]') AND [c].[name] = N'CreatedDate');
    IF @var141 IS NOT NULL EXEC(N'ALTER TABLE [RII_ACTIVITY_TOPIC_PURPOSE] DROP CONSTRAINT [' + @var141 + '];');
    ALTER TABLE [RII_ACTIVITY_TOPIC_PURPOSE] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var142 sysname;
    SELECT @var142 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_ACTIVITY_SHIPPING]') AND [c].[name] = N'CreatedDate');
    IF @var142 IS NOT NULL EXEC(N'ALTER TABLE [RII_ACTIVITY_SHIPPING] DROP CONSTRAINT [' + @var142 + '];');
    ALTER TABLE [RII_ACTIVITY_SHIPPING] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var143 sysname;
    SELECT @var143 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_ACTIVITY_REMINDER]') AND [c].[name] = N'CreatedDate');
    IF @var143 IS NOT NULL EXEC(N'ALTER TABLE [RII_ACTIVITY_REMINDER] DROP CONSTRAINT [' + @var143 + '];');
    ALTER TABLE [RII_ACTIVITY_REMINDER] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var144 sysname;
    SELECT @var144 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_ACTIVITY_MEETING_TYPE]') AND [c].[name] = N'CreatedDate');
    IF @var144 IS NOT NULL EXEC(N'ALTER TABLE [RII_ACTIVITY_MEETING_TYPE] DROP CONSTRAINT [' + @var144 + '];');
    ALTER TABLE [RII_ACTIVITY_MEETING_TYPE] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var145 sysname;
    SELECT @var145 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_ACTIVITY_IMAGE]') AND [c].[name] = N'CreatedDate');
    IF @var145 IS NOT NULL EXEC(N'ALTER TABLE [RII_ACTIVITY_IMAGE] DROP CONSTRAINT [' + @var145 + '];');
    ALTER TABLE [RII_ACTIVITY_IMAGE] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var146 sysname;
    SELECT @var146 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_ACTIVITY]') AND [c].[name] = N'CreatedDate');
    IF @var146 IS NOT NULL EXEC(N'ALTER TABLE [RII_ACTIVITY] DROP CONSTRAINT [' + @var146 + '];');
    ALTER TABLE [RII_ACTIVITY] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    DECLARE @var147 sysname;
    SELECT @var147 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Notifications]') AND [c].[name] = N'CreatedDate');
    IF @var147 IS NOT NULL EXEC(N'ALTER TABLE [Notifications] DROP CONSTRAINT [' + @var147 + '];');
    ALTER TABLE [Notifications] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260326202637_RevertAuditDatesToUtc'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260326202637_RevertAuditDatesToUtc', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    DECLARE @var148 sysname;
    SELECT @var148 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_USER_DETAIL]') AND [c].[name] = N'Weight');
    IF @var148 IS NOT NULL EXEC(N'ALTER TABLE [RII_USER_DETAIL] DROP CONSTRAINT [' + @var148 + '];');
    ALTER TABLE [RII_USER_DETAIL] ALTER COLUMN [Weight] decimal(18,6) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    DECLARE @var149 sysname;
    SELECT @var149 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_USER_DETAIL]') AND [c].[name] = N'Height');
    IF @var149 IS NOT NULL EXEC(N'ALTER TABLE [RII_USER_DETAIL] DROP CONSTRAINT [' + @var149 + '];');
    ALTER TABLE [RII_USER_DETAIL] ALTER COLUMN [Height] decimal(18,6) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    DROP INDEX [IX_ShippingAddress_Latitude_Longitude] ON [RII_SHIPPING_ADDRESS];
    DECLARE @var150 sysname;
    SELECT @var150 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_SHIPPING_ADDRESS]') AND [c].[name] = N'Longitude');
    IF @var150 IS NOT NULL EXEC(N'ALTER TABLE [RII_SHIPPING_ADDRESS] DROP CONSTRAINT [' + @var150 + '];');
    ALTER TABLE [RII_SHIPPING_ADDRESS] ALTER COLUMN [Longitude] decimal(18,6) NULL;
    CREATE INDEX [IX_ShippingAddress_Latitude_Longitude] ON [RII_SHIPPING_ADDRESS] ([Latitude], [Longitude]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    DROP INDEX [IX_ShippingAddress_Latitude_Longitude] ON [RII_SHIPPING_ADDRESS];
    DECLARE @var151 sysname;
    SELECT @var151 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_SHIPPING_ADDRESS]') AND [c].[name] = N'Latitude');
    IF @var151 IS NOT NULL EXEC(N'ALTER TABLE [RII_SHIPPING_ADDRESS] DROP CONSTRAINT [' + @var151 + '];');
    ALTER TABLE [RII_SHIPPING_ADDRESS] ALTER COLUMN [Latitude] decimal(18,6) NULL;
    CREATE INDEX [IX_ShippingAddress_Latitude_Longitude] ON [RII_SHIPPING_ADDRESS] ([Latitude], [Longitude]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    DROP INDEX [IX_Customer_Latitude_Longitude] ON [RII_CUSTOMER];
    DECLARE @var152 sysname;
    SELECT @var152 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_CUSTOMER]') AND [c].[name] = N'Longitude');
    IF @var152 IS NOT NULL EXEC(N'ALTER TABLE [RII_CUSTOMER] DROP CONSTRAINT [' + @var152 + '];');
    ALTER TABLE [RII_CUSTOMER] ALTER COLUMN [Longitude] decimal(18,6) NULL;
    CREATE INDEX [IX_Customer_Latitude_Longitude] ON [RII_CUSTOMER] ([Latitude], [Longitude]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    DROP INDEX [IX_Customer_Latitude_Longitude] ON [RII_CUSTOMER];
    DECLARE @var153 sysname;
    SELECT @var153 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RII_CUSTOMER]') AND [c].[name] = N'Latitude');
    IF @var153 IS NOT NULL EXEC(N'ALTER TABLE [RII_CUSTOMER] DROP CONSTRAINT [' + @var153 + '];');
    ALTER TABLE [RII_CUSTOMER] ALTER COLUMN [Latitude] decimal(18,6) NULL;
    CREATE INDEX [IX_Customer_Latitude_Longitude] ON [RII_CUSTOMER] ([Latitude], [Longitude]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE TABLE [RII_PRODUCT_CATALOG] (
        [Id] bigint NOT NULL IDENTITY,
        [Name] nvarchar(150) NOT NULL,
        [Code] nvarchar(50) NOT NULL,
        [Description] nvarchar(500) NULL,
        [CatalogType] int NOT NULL,
        [BranchCode] int NULL,
        [SortOrder] int NOT NULL DEFAULT 0,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_PRODUCT_CATALOG] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_PRODUCT_CATALOG_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_PRODUCT_CATALOG_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_PRODUCT_CATALOG_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE TABLE [RII_PRODUCT_CATEGORY] (
        [Id] bigint NOT NULL IDENTITY,
        [ParentCategoryId] bigint NULL,
        [Name] nvarchar(150) NOT NULL,
        [Code] nvarchar(50) NOT NULL,
        [Description] nvarchar(500) NULL,
        [Level] int NOT NULL DEFAULT 1,
        [FullPath] nvarchar(1000) NULL,
        [SortOrder] int NOT NULL DEFAULT 0,
        [ImageUrl] nvarchar(500) NULL,
        [IconName] nvarchar(100) NULL,
        [ColorHex] nvarchar(20) NULL,
        [IsLeaf] bit NOT NULL DEFAULT CAST(0 AS bit),
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_PRODUCT_CATEGORY] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_PRODUCT_CATEGORY_RII_PRODUCT_CATEGORY_ParentCategoryId] FOREIGN KEY ([ParentCategoryId]) REFERENCES [RII_PRODUCT_CATEGORY] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RII_PRODUCT_CATEGORY_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_PRODUCT_CATEGORY_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_PRODUCT_CATEGORY_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE TABLE [RII_CATALOG_CATEGORY] (
        [Id] bigint NOT NULL IDENTITY,
        [CatalogId] bigint NOT NULL,
        [CategoryId] bigint NOT NULL,
        [ParentCatalogCategoryId] bigint NULL,
        [SortOrder] int NOT NULL DEFAULT 0,
        [IsRoot] bit NOT NULL DEFAULT CAST(0 AS bit),
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_CATALOG_CATEGORY] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_CATALOG_CATEGORY_RII_CATALOG_CATEGORY_ParentCatalogCategoryId] FOREIGN KEY ([ParentCatalogCategoryId]) REFERENCES [RII_CATALOG_CATEGORY] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RII_CATALOG_CATEGORY_RII_PRODUCT_CATALOG_CatalogId] FOREIGN KEY ([CatalogId]) REFERENCES [RII_PRODUCT_CATALOG] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RII_CATALOG_CATEGORY_RII_PRODUCT_CATEGORY_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [RII_PRODUCT_CATEGORY] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RII_CATALOG_CATEGORY_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_CATALOG_CATEGORY_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_CATALOG_CATEGORY_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE TABLE [RII_PRODUCT_CATEGORY_RULE] (
        [Id] bigint NOT NULL IDENTITY,
        [CategoryId] bigint NOT NULL,
        [RuleName] nvarchar(150) NOT NULL,
        [RuleCode] nvarchar(50) NULL,
        [StockAttributeType] int NOT NULL,
        [OperatorType] int NOT NULL,
        [Value] nvarchar(250) NOT NULL,
        [Priority] int NOT NULL DEFAULT 0,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_PRODUCT_CATEGORY_RULE] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_PRODUCT_CATEGORY_RULE_RII_PRODUCT_CATEGORY_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [RII_PRODUCT_CATEGORY] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RII_PRODUCT_CATEGORY_RULE_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_PRODUCT_CATEGORY_RULE_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_PRODUCT_CATEGORY_RULE_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE TABLE [RII_STOCK_CATEGORY] (
        [Id] bigint NOT NULL IDENTITY,
        [StockId] bigint NOT NULL,
        [CategoryId] bigint NOT NULL,
        [AssignmentType] int NOT NULL,
        [RuleId] bigint NULL,
        [IsPrimary] bit NOT NULL DEFAULT CAST(1 AS bit),
        [SortOrder] int NOT NULL DEFAULT 0,
        [Note] nvarchar(500) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_STOCK_CATEGORY] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_STOCK_CATEGORY_RII_PRODUCT_CATEGORY_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [RII_PRODUCT_CATEGORY] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RII_STOCK_CATEGORY_RII_PRODUCT_CATEGORY_RULE_RuleId] FOREIGN KEY ([RuleId]) REFERENCES [RII_PRODUCT_CATEGORY_RULE] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RII_STOCK_CATEGORY_RII_STOCK_StockId] FOREIGN KEY ([StockId]) REFERENCES [RII_STOCK] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RII_STOCK_CATEGORY_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_STOCK_CATEGORY_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_STOCK_CATEGORY_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_CatalogCategory_IsDeleted] ON [RII_CATALOG_CATEGORY] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_CatalogCategory_ParentCatalogCategoryId] ON [RII_CATALOG_CATEGORY] ([ParentCatalogCategoryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_RII_CATALOG_CATEGORY_CategoryId] ON [RII_CATALOG_CATEGORY] ([CategoryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_RII_CATALOG_CATEGORY_CreatedBy] ON [RII_CATALOG_CATEGORY] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_RII_CATALOG_CATEGORY_DeletedBy] ON [RII_CATALOG_CATEGORY] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_RII_CATALOG_CATEGORY_UpdatedBy] ON [RII_CATALOG_CATEGORY] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE UNIQUE INDEX [UX_CatalogCategory_CatalogId_CategoryId] ON [RII_CATALOG_CATEGORY] ([CatalogId], [CategoryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_ProductCatalog_IsDeleted] ON [RII_PRODUCT_CATALOG] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_RII_PRODUCT_CATALOG_CreatedBy] ON [RII_PRODUCT_CATALOG] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_RII_PRODUCT_CATALOG_DeletedBy] ON [RII_PRODUCT_CATALOG] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_RII_PRODUCT_CATALOG_UpdatedBy] ON [RII_PRODUCT_CATALOG] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE UNIQUE INDEX [UX_ProductCatalog_Code] ON [RII_PRODUCT_CATALOG] ([Code]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_ProductCategory_IsDeleted] ON [RII_PRODUCT_CATEGORY] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_ProductCategory_ParentCategoryId] ON [RII_PRODUCT_CATEGORY] ([ParentCategoryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_RII_PRODUCT_CATEGORY_CreatedBy] ON [RII_PRODUCT_CATEGORY] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_RII_PRODUCT_CATEGORY_DeletedBy] ON [RII_PRODUCT_CATEGORY] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_RII_PRODUCT_CATEGORY_UpdatedBy] ON [RII_PRODUCT_CATEGORY] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE UNIQUE INDEX [UX_ProductCategory_Code] ON [RII_PRODUCT_CATEGORY] ([Code]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_ProductCategoryRule_CategoryId] ON [RII_PRODUCT_CATEGORY_RULE] ([CategoryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_ProductCategoryRule_IsDeleted] ON [RII_PRODUCT_CATEGORY_RULE] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_ProductCategoryRule_Priority] ON [RII_PRODUCT_CATEGORY_RULE] ([Priority]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_RII_PRODUCT_CATEGORY_RULE_CreatedBy] ON [RII_PRODUCT_CATEGORY_RULE] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_RII_PRODUCT_CATEGORY_RULE_DeletedBy] ON [RII_PRODUCT_CATEGORY_RULE] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_RII_PRODUCT_CATEGORY_RULE_UpdatedBy] ON [RII_PRODUCT_CATEGORY_RULE] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_RII_STOCK_CATEGORY_CreatedBy] ON [RII_STOCK_CATEGORY] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_RII_STOCK_CATEGORY_DeletedBy] ON [RII_STOCK_CATEGORY] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_RII_STOCK_CATEGORY_RuleId] ON [RII_STOCK_CATEGORY] ([RuleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_RII_STOCK_CATEGORY_UpdatedBy] ON [RII_STOCK_CATEGORY] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_StockCategory_CategoryId] ON [RII_STOCK_CATEGORY] ([CategoryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE INDEX [IX_StockCategory_IsDeleted] ON [RII_STOCK_CATEGORY] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    CREATE UNIQUE INDEX [UX_StockCategory_StockId_CategoryId] ON [RII_STOCK_CATEGORY] ([StockId], [CategoryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410063138_AddCatalogModuleInitial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260410063138_AddCatalogModuleInitial', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410092031_AddCategoryVisualPreset'
)
BEGIN
    ALTER TABLE [RII_PRODUCT_CATEGORY] ADD [VisualPreset] int NOT NULL DEFAULT 1;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410092031_AddCategoryVisualPreset'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260410092031_AddCategoryVisualPreset', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410191345_CreatePdfImageTables'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260410191345_CreatePdfImageTables', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410201617_AddPdfImageUsageLinks'
)
BEGIN
    CREATE TABLE [RII_PDF_IMAGE_USAGES] (
        [Id] bigint NOT NULL IDENTITY,
        [PdfTemplateAssetId] bigint NOT NULL,
        [ReportTemplateId] bigint NOT NULL,
        [UsageType] int NOT NULL,
        [ElementId] nvarchar(120) NOT NULL,
        [PageNumber] int NOT NULL,
        [RuleType] int NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [CreatedByUserId] bigint NULL,
        [UpdatedBy] bigint NULL,
        [UpdatedByUserId] bigint NULL,
        [DeletedBy] bigint NULL,
        [DeletedByUserId] bigint NULL,
        CONSTRAINT [PK_RII_PDF_IMAGE_USAGES] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_PDF_IMAGE_USAGES_RII_PDF_IMAGES_PdfTemplateAssetId] FOREIGN KEY ([PdfTemplateAssetId]) REFERENCES [RII_PDF_IMAGES] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RII_PDF_IMAGE_USAGES_RII_REPORT_TEMPLATES_ReportTemplateId] FOREIGN KEY ([ReportTemplateId]) REFERENCES [RII_REPORT_TEMPLATES] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RII_PDF_IMAGE_USAGES_RII_USERS_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_PDF_IMAGE_USAGES_RII_USERS_DeletedByUserId] FOREIGN KEY ([DeletedByUserId]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_PDF_IMAGE_USAGES_RII_USERS_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410201617_AddPdfImageUsageLinks'
)
BEGIN
    CREATE TABLE [RII_QUICK_QUOTATION_IMAGE_USAGES] (
        [Id] bigint NOT NULL IDENTITY,
        [QuickQuotationImageId] bigint NOT NULL,
        [TempQuotattionId] bigint NOT NULL,
        [TempQuotattionLineId] bigint NOT NULL,
        [ProductCode] nvarchar(100) NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [CreatedByUserId] bigint NULL,
        [UpdatedBy] bigint NULL,
        [UpdatedByUserId] bigint NULL,
        [DeletedBy] bigint NULL,
        [DeletedByUserId] bigint NULL,
        CONSTRAINT [PK_RII_QUICK_QUOTATION_IMAGE_USAGES] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_QUICK_QUOTATION_IMAGE_USAGES_RII_QUICK_QUOTATION_IMAGES_QuickQuotationImageId] FOREIGN KEY ([QuickQuotationImageId]) REFERENCES [RII_QUICK_QUOTATION_IMAGES] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RII_QUICK_QUOTATION_IMAGE_USAGES_RII_TEMP_QUOTATTION_LINE_TempQuotattionLineId] FOREIGN KEY ([TempQuotattionLineId]) REFERENCES [RII_TEMP_QUOTATTION_LINE] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RII_QUICK_QUOTATION_IMAGE_USAGES_RII_TEMP_QUOTATTION_TempQuotattionId] FOREIGN KEY ([TempQuotattionId]) REFERENCES [RII_TEMP_QUOTATTION] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RII_QUICK_QUOTATION_IMAGE_USAGES_RII_USERS_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_QUICK_QUOTATION_IMAGE_USAGES_RII_USERS_DeletedByUserId] FOREIGN KEY ([DeletedByUserId]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_QUICK_QUOTATION_IMAGE_USAGES_RII_USERS_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410201617_AddPdfImageUsageLinks'
)
BEGIN
    CREATE INDEX [IX_RII_PDF_IMAGE_USAGES_CreatedByUserId] ON [RII_PDF_IMAGE_USAGES] ([CreatedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410201617_AddPdfImageUsageLinks'
)
BEGIN
    CREATE INDEX [IX_RII_PDF_IMAGE_USAGES_DeletedByUserId] ON [RII_PDF_IMAGE_USAGES] ([DeletedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410201617_AddPdfImageUsageLinks'
)
BEGIN
    CREATE INDEX [IX_RII_PDF_IMAGE_USAGES_ImageId] ON [RII_PDF_IMAGE_USAGES] ([PdfTemplateAssetId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410201617_AddPdfImageUsageLinks'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_RII_PDF_IMAGE_USAGES_Template_Element_Page] ON [RII_PDF_IMAGE_USAGES] ([ReportTemplateId], [ElementId], [PageNumber]) WHERE [IsDeleted] = 0');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410201617_AddPdfImageUsageLinks'
)
BEGIN
    CREATE INDEX [IX_RII_PDF_IMAGE_USAGES_UpdatedByUserId] ON [RII_PDF_IMAGE_USAGES] ([UpdatedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410201617_AddPdfImageUsageLinks'
)
BEGIN
    CREATE INDEX [IX_RII_QUICK_QUOTATION_IMAGE_USAGES_CreatedByUserId] ON [RII_QUICK_QUOTATION_IMAGE_USAGES] ([CreatedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410201617_AddPdfImageUsageLinks'
)
BEGIN
    CREATE INDEX [IX_RII_QUICK_QUOTATION_IMAGE_USAGES_DeletedByUserId] ON [RII_QUICK_QUOTATION_IMAGE_USAGES] ([DeletedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410201617_AddPdfImageUsageLinks'
)
BEGIN
    CREATE INDEX [IX_RII_QUICK_QUOTATION_IMAGE_USAGES_ImageId] ON [RII_QUICK_QUOTATION_IMAGE_USAGES] ([QuickQuotationImageId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410201617_AddPdfImageUsageLinks'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_RII_QUICK_QUOTATION_IMAGE_USAGES_LineId] ON [RII_QUICK_QUOTATION_IMAGE_USAGES] ([TempQuotattionLineId]) WHERE [IsDeleted] = 0');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410201617_AddPdfImageUsageLinks'
)
BEGIN
    CREATE INDEX [IX_RII_QUICK_QUOTATION_IMAGE_USAGES_TempQuotattionId] ON [RII_QUICK_QUOTATION_IMAGE_USAGES] ([TempQuotattionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410201617_AddPdfImageUsageLinks'
)
BEGIN
    CREATE INDEX [IX_RII_QUICK_QUOTATION_IMAGE_USAGES_UpdatedByUserId] ON [RII_QUICK_QUOTATION_IMAGE_USAGES] ([UpdatedByUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410201617_AddPdfImageUsageLinks'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260410201617_AddPdfImageUsageLinks', N'8.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260415152553_AddSalesRepTables'
)
BEGIN
    CREATE TABLE [RII_SalepRep_Codes] (
        [Id] bigint NOT NULL IDENTITY,
        [BranchCode] smallint NOT NULL,
        [SalesRepCode] nvarchar(8) NOT NULL,
        [SalesRepDescription] nvarchar(30) NULL,
        [Name] nvarchar(35) NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_SalepRep_Codes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_SalepRep_Codes_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_SalepRep_Codes_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_SalepRep_Codes_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260415152553_AddSalesRepTables'
)
BEGIN
    CREATE TABLE [RII_SalepRep_Code_User_Matches] (
        [Id] bigint NOT NULL IDENTITY,
        [SalesRepCodeId] bigint NOT NULL,
        [UserId] bigint NOT NULL,
        [CreatedDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_SalepRep_Code_User_Matches] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RII_SalepRep_Code_User_Matches_RII_SalepRep_Codes_SalesRepCodeId] FOREIGN KEY ([SalesRepCodeId]) REFERENCES [RII_SalepRep_Codes] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RII_SalepRep_Code_User_Matches_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_SalepRep_Code_User_Matches_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_SalepRep_Code_User_Matches_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS] ([Id]),
        CONSTRAINT [FK_RII_SalepRep_Code_User_Matches_RII_USERS_UserId] FOREIGN KEY ([UserId]) REFERENCES [RII_USERS] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260415152553_AddSalesRepTables'
)
BEGIN
    CREATE INDEX [IX_RII_SalepRep_Code_User_Matches_CreatedBy] ON [RII_SalepRep_Code_User_Matches] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260415152553_AddSalesRepTables'
)
BEGIN
    CREATE INDEX [IX_RII_SalepRep_Code_User_Matches_DeletedBy] ON [RII_SalepRep_Code_User_Matches] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260415152553_AddSalesRepTables'
)
BEGIN
    CREATE INDEX [IX_RII_SalepRep_Code_User_Matches_UpdatedBy] ON [RII_SalepRep_Code_User_Matches] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260415152553_AddSalesRepTables'
)
BEGIN
    CREATE INDEX [IX_SalesRepCodeUserMatch_IsDeleted] ON [RII_SalepRep_Code_User_Matches] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260415152553_AddSalesRepTables'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_SalesRepCodeUserMatch_SalesRepCodeId_UserId] ON [RII_SalepRep_Code_User_Matches] ([SalesRepCodeId], [UserId]) WHERE [IsDeleted] = 0');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260415152553_AddSalesRepTables'
)
BEGIN
    CREATE INDEX [IX_SalesRepCodeUserMatch_UserId] ON [RII_SalepRep_Code_User_Matches] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260415152553_AddSalesRepTables'
)
BEGIN
    CREATE INDEX [IX_RII_SalepRep_Codes_CreatedBy] ON [RII_SalepRep_Codes] ([CreatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260415152553_AddSalesRepTables'
)
BEGIN
    CREATE INDEX [IX_RII_SalepRep_Codes_DeletedBy] ON [RII_SalepRep_Codes] ([DeletedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260415152553_AddSalesRepTables'
)
BEGIN
    CREATE INDEX [IX_RII_SalepRep_Codes_UpdatedBy] ON [RII_SalepRep_Codes] ([UpdatedBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260415152553_AddSalesRepTables'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_SalesRepCode_BranchCode_SalesRepCode] ON [RII_SalepRep_Codes] ([BranchCode], [SalesRepCode]) WHERE [IsDeleted] = 0');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260415152553_AddSalesRepTables'
)
BEGIN
    CREATE INDEX [IX_SalesRepCode_IsDeleted] ON [RII_SalepRep_Codes] ([IsDeleted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260415152553_AddSalesRepTables'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260415152553_AddSalesRepTables', N'8.0.11');
END;
GO

COMMIT;
GO

