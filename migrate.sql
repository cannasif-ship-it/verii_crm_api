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

