/*
  Daily user card performance report
  Scope:
  - Counts processed / scanned cards from RII_CUSTOMER_IMAGE
  - Counts created contacts from RII_CONTACT
  - Counts created companies from RII_CUSTOMER
  - Creates / updates a Report Builder report definition
  - Assigns the report to a target user in RII_REPORT_ASSIGNMENTS

  Preconditions:
  1. RII_REPORT_ASSIGNMENTS migration must already be applied.
  2. Creator and target viewer users must exist in RII_USERS.
*/

SET NOCOUNT ON;

DECLARE @CreatorEmail NVARCHAR(256) = N'can.nasif@v3rii.com';
DECLARE @ViewerEmail NVARCHAR(256) = N'can.nasif@v3rii.com';
DECLARE @ReportName NVARCHAR(200) = N'Günlük Kullanıcı Kart Performans Raporu';
DECLARE @ReportDescription NVARCHAR(500) = N'Hangi kullanıcının hangi gün kaç kart işlediğini, kaç contact oluşturduğunu ve kaç firma oluşturduğunu gösterir.';
DECLARE @DataSourceName NVARCHAR(128) = N'dbo.RII_FN_USER_DAILY_CARD_PERFORMANCE';

DECLARE @CreatorUserId BIGINT;
DECLARE @ViewerUserId BIGINT;
DECLARE @ReportId BIGINT;

SELECT @CreatorUserId = Id
FROM RII_USERS
WHERE IsDeleted = 0
  AND Email = @CreatorEmail;

SELECT @ViewerUserId = Id
FROM RII_USERS
WHERE IsDeleted = 0
  AND Email = @ViewerEmail;

IF @CreatorUserId IS NULL
BEGIN
    THROW 50001, 'Creator user was not found in RII_USERS.', 1;
END;

IF @ViewerUserId IS NULL
BEGIN
    THROW 50002, 'Viewer user was not found in RII_USERS.', 1;
END;

EXEC('
CREATE OR ALTER FUNCTION dbo.RII_FN_USER_DAILY_CARD_PERFORMANCE
(
    @p_start_date DATE = NULL,
    @p_end_date DATE = NULL,
    @p_user_id BIGINT = NULL,
    @p_user_email NVARCHAR(256) = NULL
)
RETURNS TABLE
AS
RETURN
WITH scanned_cards AS
(
    SELECT
        CAST(ci.CreatedDate AS DATE) AS report_date,
        ci.CreatedBy AS user_id,
        COUNT(*) AS scanned_card_count
    FROM RII_CUSTOMER_IMAGE ci
    WHERE ci.IsDeleted = 0
      AND ci.CreatedBy IS NOT NULL
      AND (@p_start_date IS NULL OR CAST(ci.CreatedDate AS DATE) >= @p_start_date)
      AND (@p_end_date IS NULL OR CAST(ci.CreatedDate AS DATE) <= @p_end_date)
    GROUP BY CAST(ci.CreatedDate AS DATE), ci.CreatedBy
),
created_contacts AS
(
    SELECT
        CAST(c.CreatedDate AS DATE) AS report_date,
        c.CreatedBy AS user_id,
        COUNT(*) AS created_contact_count
    FROM RII_CONTACT c
    WHERE c.IsDeleted = 0
      AND c.CreatedBy IS NOT NULL
      AND (@p_start_date IS NULL OR CAST(c.CreatedDate AS DATE) >= @p_start_date)
      AND (@p_end_date IS NULL OR CAST(c.CreatedDate AS DATE) <= @p_end_date)
    GROUP BY CAST(c.CreatedDate AS DATE), c.CreatedBy
),
created_companies AS
(
    SELECT
        CAST(c.CreatedDate AS DATE) AS report_date,
        c.CreatedBy AS user_id,
        COUNT(*) AS created_company_count
    FROM RII_CUSTOMER c
    WHERE c.IsDeleted = 0
      AND c.CreatedBy IS NOT NULL
      AND (@p_start_date IS NULL OR CAST(c.CreatedDate AS DATE) >= @p_start_date)
      AND (@p_end_date IS NULL OR CAST(c.CreatedDate AS DATE) <= @p_end_date)
    GROUP BY CAST(c.CreatedDate AS DATE), c.CreatedBy
),
usage_keys AS
(
    SELECT report_date, user_id FROM scanned_cards
    UNION
    SELECT report_date, user_id FROM created_contacts
    UNION
    SELECT report_date, user_id FROM created_companies
)
SELECT
    uk.report_date,
    u.Id AS user_id,
    COALESCE(NULLIF(LTRIM(RTRIM(CONCAT(ISNULL(u.FirstName, ''''), '' '', ISNULL(u.LastName, '''')))), ''''), u.Username, u.Email) AS user_name,
    u.Email AS user_email,
    ISNULL(sc.scanned_card_count, 0) AS scanned_card_count,
    ISNULL(cc.created_contact_count, 0) AS created_contact_count,
    ISNULL(cp.created_company_count, 0) AS created_company_count
FROM usage_keys uk
INNER JOIN RII_USERS u
    ON u.Id = uk.user_id
   AND u.IsDeleted = 0
LEFT JOIN scanned_cards sc
    ON sc.report_date = uk.report_date
   AND sc.user_id = uk.user_id
LEFT JOIN created_contacts cc
    ON cc.report_date = uk.report_date
   AND cc.user_id = uk.user_id
LEFT JOIN created_companies cp
    ON cp.report_date = uk.report_date
   AND cp.user_id = uk.user_id
WHERE (@p_user_id IS NULL OR u.Id = @p_user_id)
  AND (@p_user_email IS NULL OR u.Email = @p_user_email)
');

DECLARE @ConfigJson NVARCHAR(MAX) = N'{
  "chartType": "line",
  "axis": { "field": "report_date", "dateGrouping": "day" },
  "values": [
    { "field": "scanned_card_count", "aggregation": "sum" }
  ],
  "legend": { "field": "user_name" },
  "sorting": { "by": "axis", "direction": "asc" },
  "filters": [],
  "datasetParameters": [],
  "calculatedFields": [],
  "lifecycle": {
    "status": "published",
    "version": 1,
    "releaseNote": "Gunluk kullanici kart performans raporu"
  },
  "governance": {
    "category": "Satis Operasyon",
    "tags": ["kart", "contact", "firma", "gunluk-performans"],
    "audience": "private",
    "refreshCadence": "daily",
    "favorite": false,
    "sharedWith": ["can.nasif@v3rii.com"],
    "owner": "Veri Admin",
    "certified": true
  },
  "widgets": [
    {
      "id": "daily-card-trend",
      "title": "Gunluk kart isleme trendi",
      "size": "full",
      "height": "md",
      "chartType": "line",
      "axis": { "field": "report_date", "dateGrouping": "day" },
      "values": [
        { "field": "scanned_card_count", "aggregation": "sum" }
      ],
      "legend": { "field": "user_name" },
      "filters": []
    },
    {
      "id": "user-contact-company-comparison",
      "title": "Kullanici bazli contact ve firma karsilastirmasi",
      "size": "full",
      "height": "md",
      "chartType": "bar",
      "axis": { "field": "user_name" },
      "values": [
        { "field": "created_contact_count", "aggregation": "sum" },
        { "field": "created_company_count", "aggregation": "sum" }
      ],
      "filters": []
    },
    {
      "id": "daily-card-performance-table",
      "title": "Gunluk detay tablo",
      "size": "full",
      "height": "lg",
      "chartType": "table",
      "axis": { "field": "report_date", "dateGrouping": "day" },
      "values": [
        { "field": "scanned_card_count", "aggregation": "sum" },
        { "field": "created_contact_count", "aggregation": "sum" },
        { "field": "created_company_count", "aggregation": "sum" }
      ],
      "legend": { "field": "user_name" },
      "filters": []
    }
  ],
  "activeWidgetId": "daily-card-trend"
}';

SELECT @ReportId = Id
FROM RII_REPORT_DEFINITIONS
WHERE IsDeleted = 0
  AND Name = @ReportName
  AND ConnectionKey = N'CRM'
  AND DataSourceType = N'function'
  AND DataSourceName = @DataSourceName;

IF @ReportId IS NULL
BEGIN
    INSERT INTO RII_REPORT_DEFINITIONS
    (
        Name,
        Description,
        ConnectionKey,
        DataSourceType,
        DataSourceName,
        ConfigJson,
        CreatedDate,
        UpdatedDate,
        DeletedDate,
        IsDeleted,
        CreatedBy,
        UpdatedBy,
        DeletedBy
    )
    VALUES
    (
        @ReportName,
        @ReportDescription,
        N'CRM',
        N'function',
        @DataSourceName,
        @ConfigJson,
        GETDATE(),
        NULL,
        NULL,
        0,
        @CreatorUserId,
        NULL,
        NULL
    );

    SET @ReportId = SCOPE_IDENTITY();
END
ELSE
BEGIN
    UPDATE RII_REPORT_DEFINITIONS
    SET
        Description = @ReportDescription,
        ConfigJson = @ConfigJson,
        UpdatedDate = GETDATE(),
        UpdatedBy = @CreatorUserId
    WHERE Id = @ReportId;
END;

UPDATE RII_REPORT_ASSIGNMENTS
SET
    IsDeleted = 1,
    DeletedDate = GETDATE(),
    DeletedBy = @CreatorUserId,
    UpdatedDate = GETDATE(),
    UpdatedBy = @CreatorUserId
WHERE ReportDefinitionId = @ReportId
  AND IsDeleted = 0
  AND UserId <> @ViewerUserId;

IF EXISTS
(
    SELECT 1
    FROM RII_REPORT_ASSIGNMENTS
    WHERE ReportDefinitionId = @ReportId
      AND UserId = @ViewerUserId
)
BEGIN
    UPDATE RII_REPORT_ASSIGNMENTS
    SET
        IsDeleted = 0,
        DeletedDate = NULL,
        DeletedBy = NULL,
        UpdatedDate = GETDATE(),
        UpdatedBy = @CreatorUserId
    WHERE ReportDefinitionId = @ReportId
      AND UserId = @ViewerUserId;
END
ELSE
BEGIN
    INSERT INTO RII_REPORT_ASSIGNMENTS
    (
        ReportDefinitionId,
        UserId,
        CreatedDate,
        UpdatedDate,
        DeletedDate,
        IsDeleted,
        CreatedBy,
        UpdatedBy,
        DeletedBy
    )
    VALUES
    (
        @ReportId,
        @ViewerUserId,
        GETDATE(),
        NULL,
        NULL,
        0,
        @CreatorUserId,
        NULL,
        NULL
    );
END;

SELECT
    @ReportId AS ReportId,
    @CreatorUserId AS CreatorUserId,
    @ViewerUserId AS ViewerUserId,
    @DataSourceName AS DataSourceName;
