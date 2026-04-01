EXEC(N'
CREATE OR ALTER FUNCTION dbo.RII_FN_USER_DAILY_CARD_PERFORMANCE
(
    @StartDate DATE = NULL,
    @EndDate DATE = NULL,
    @UserId BIGINT = NULL,
    @UserEmail NVARCHAR(100) = NULL
)
RETURNS TABLE
AS
RETURN
(
    WITH DateBounds AS
    (
        SELECT
            CAST(COALESCE(@StartDate, DATEADD(DAY, -29, CAST(GETDATE() AS DATE))) AS DATE) AS StartDate,
            CAST(COALESCE(@EndDate, CAST(GETDATE() AS DATE)) AS DATE) AS EndDate,
            @UserId AS UserId,
            NULLIF(LTRIM(RTRIM(@UserEmail)), N'''') AS UserEmail
    ),
    NormalizedBounds AS
    (
        SELECT
            CASE WHEN EndDate < StartDate THEN EndDate ELSE StartDate END AS StartDate,
            CASE WHEN EndDate < StartDate THEN StartDate ELSE EndDate END AS EndDate,
            UserId,
            UserEmail
        FROM DateBounds
    ),
    DateSeries AS
    (
        SELECT DATEADD(DAY, NumberSeries.RowNumber, Bounds.StartDate) AS Tarih
        FROM NormalizedBounds Bounds
        CROSS APPLY
        (
            SELECT TOP (DATEDIFF(DAY, Bounds.StartDate, Bounds.EndDate) + 1)
                ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1 AS RowNumber
            FROM sys.all_objects ObjectA
            CROSS JOIN sys.all_objects ObjectB
        ) NumberSeries
    ),
    FilteredUsers AS
    (
        SELECT
            UserTable.Id AS UserId,
            UserTable.Email AS UserEmail,
            LTRIM(RTRIM(
                COALESCE(NULLIF(UserTable.Username, N''''), UserTable.Email, N''Kullanıcı'')
                + CASE
                    WHEN NULLIF(UserTable.LastName, N'''') IS NOT NULL THEN N'' '' + UserTable.LastName
                    WHEN NULLIF(UserTable.FirstName, N'''') IS NOT NULL THEN N'' '' + UserTable.FirstName
                    ELSE N''''
                  END
            )) AS Plasiyer
        FROM RII_USERS UserTable
        CROSS JOIN NormalizedBounds Bounds
        WHERE UserTable.IsDeleted = 0
          AND UserTable.IsActive = 1
          AND (Bounds.UserId IS NULL OR UserTable.Id = Bounds.UserId)
          AND (Bounds.UserEmail IS NULL OR LOWER(UserTable.Email) = LOWER(Bounds.UserEmail))
    ),
    CardScanSummary AS
    (
        SELECT
            CustomerImage.CreatedBy AS UserId,
            CAST(CustomerImage.CreatedDate AS DATE) AS Tarih,
            COUNT_BIG(1) AS OkutulanKartSayisi
        FROM RII_CUSTOMER_IMAGE CustomerImage
        CROSS JOIN NormalizedBounds Bounds
        WHERE CustomerImage.IsDeleted = 0
          AND CustomerImage.CreatedBy IS NOT NULL
          AND CAST(CustomerImage.CreatedDate AS DATE) BETWEEN Bounds.StartDate AND Bounds.EndDate
        GROUP BY CustomerImage.CreatedBy, CAST(CustomerImage.CreatedDate AS DATE)
    ),
    ContactSummary AS
    (
        SELECT
            ContactTable.CreatedBy AS UserId,
            CAST(ContactTable.CreatedDate AS DATE) AS Tarih,
            COUNT_BIG(1) AS OlusturulanKisiSayisi
        FROM RII_CONTACT ContactTable
        CROSS JOIN NormalizedBounds Bounds
        WHERE ContactTable.IsDeleted = 0
          AND ContactTable.CreatedBy IS NOT NULL
          AND CAST(ContactTable.CreatedDate AS DATE) BETWEEN Bounds.StartDate AND Bounds.EndDate
        GROUP BY ContactTable.CreatedBy, CAST(ContactTable.CreatedDate AS DATE)
    ),
    CustomerSummary AS
    (
        SELECT
            CustomerTable.CreatedBy AS UserId,
            CAST(CustomerTable.CreatedDate AS DATE) AS Tarih,
            COUNT_BIG(1) AS OlusturulanCariSayisi
        FROM RII_CUSTOMER CustomerTable
        CROSS JOIN NormalizedBounds Bounds
        WHERE CustomerTable.IsDeleted = 0
          AND CustomerTable.CreatedBy IS NOT NULL
          AND CAST(CustomerTable.CreatedDate AS DATE) BETWEEN Bounds.StartDate AND Bounds.EndDate
        GROUP BY CustomerTable.CreatedBy, CAST(CustomerTable.CreatedDate AS DATE)
    )
    SELECT
        DateTable.Tarih AS tarih,
        UserTable.Plasiyer AS plasiyer,
        UserTable.UserId AS user_id,
        UserTable.UserEmail AS user_email,
        CAST(ISNULL(CardSummary.OkutulanKartSayisi, 0) AS INT) AS okutulan_kart_sayisi,
        CAST(ISNULL(ContactDailySummary.OlusturulanKisiSayisi, 0) AS INT) AS olusturulan_kisi_sayisi,
        CAST(ISNULL(CustomerDailySummary.OlusturulanCariSayisi, 0) AS INT) AS olusturulan_cari_sayisi
    FROM DateSeries DateTable
    CROSS JOIN FilteredUsers UserTable
    LEFT JOIN CardScanSummary CardSummary
        ON CardSummary.UserId = UserTable.UserId
       AND CardSummary.Tarih = DateTable.Tarih
    LEFT JOIN ContactSummary ContactDailySummary
        ON ContactDailySummary.UserId = UserTable.UserId
       AND ContactDailySummary.Tarih = DateTable.Tarih
    LEFT JOIN CustomerSummary CustomerDailySummary
        ON CustomerDailySummary.UserId = UserTable.UserId
       AND CustomerDailySummary.Tarih = DateTable.Tarih
);');

DECLARE @ReportName NVARCHAR(200) = N'Günlük Kullanıcı Kart Performans Raporu';
DECLARE @ReportDescription NVARCHAR(500) = N'Başlangıç ve bitiş tarihine göre plasiyer bazlı günlük performans trendlerini ve seçilen aralık için kümüle özeti gösterir.';
DECLARE @ConfigJson NVARCHAR(MAX) = N'{
  "chartType": "line",
  "axis": { "field": "tarih", "dateGrouping": "day" },
  "values": [
    { "field": "okutulan_kart_sayisi", "aggregation": "sum" }
  ],
  "legend": { "field": "plasiyer" },
  "sorting": { "by": "axis", "direction": "desc" },
  "filters": [],
  "datasetParameters": [
    { "name": "StartDate", "source": "literal", "value": "", "allowViewerOverride": true, "viewerLabel": "Başlangıç Tarihi" },
    { "name": "EndDate", "source": "literal", "value": "", "allowViewerOverride": true, "viewerLabel": "Bitiş Tarihi" }
  ],
  "lifecycle": { "status": "published", "version": 1 },
  "governance": {
    "audience": "private",
    "category": "Saha Performansı",
    "tags": ["kart", "ocr", "plasiyer", "gunluk"],
    "refreshCadence": "daily",
    "owner": "CRM",
    "certified": true
  },
  "widgets": [
    {
      "id": "daily-card-scan-trend",
      "title": "Günlük Kart Okutma Trendi",
      "size": "half",
      "height": "md",
      "appearance": {
        "themePreset": "performance",
        "tone": "soft",
        "accentColor": "#7c3aed",
        "backgroundStyle": "gradient",
        "sectionLabel": "Kart Performansı",
        "sectionDescription": "Seçilen tarih aralığında plasiyer bazlı günlük okutulan kart trendi."
      },
      "chartType": "line",
      "axis": { "field": "tarih", "dateGrouping": "day" },
      "legend": { "field": "plasiyer" },
      "sorting": { "by": "axis", "direction": "desc" },
      "filters": [],
      "values": [{ "field": "okutulan_kart_sayisi", "aggregation": "sum" }]
    },
    {
      "id": "daily-contact-creation-trend",
      "title": "Günlük Kişi Oluşturma Trendi",
      "size": "half",
      "height": "md",
      "appearance": {
        "themePreset": "operations",
        "tone": "soft",
        "accentColor": "#f59e0b",
        "backgroundStyle": "card",
        "sectionLabel": "Günlük Kişi Performansı",
        "sectionDescription": "Seçilen tarih aralığında plasiyer bazlı günlük kişi oluşturma trendi."
      },
      "chartType": "line",
      "axis": { "field": "tarih", "dateGrouping": "day" },
      "legend": { "field": "plasiyer" },
      "sorting": { "by": "axis", "direction": "desc" },
      "filters": [],
      "values": [{ "field": "olusturulan_kisi_sayisi", "aggregation": "sum" }]
    },
    {
      "id": "daily-card-performance-table",
      "title": "Plasiyer Bazlı Kümüle Özet",
      "size": "full",
      "height": "lg",
      "appearance": {
        "themePreset": "operations",
        "tableDensity": "comfortable",
        "accentColor": "#0f766e",
        "sectionLabel": "Kümüle Liste",
        "sectionDescription": "Seçilen tarih aralığında her plasiyerin toplam okutulan kart, toplam kişi ve toplam cari adetlerini gösterir."
      },
      "chartType": "table",
      "axis": { "field": "plasiyer" },
      "sorting": { "by": "value", "direction": "desc", "valueField": "okutulan_kart_sayisi" },
      "filters": [],
      "values": [
        { "field": "okutulan_kart_sayisi", "aggregation": "sum" },
        { "field": "olusturulan_kisi_sayisi", "aggregation": "sum" },
        { "field": "olusturulan_cari_sayisi", "aggregation": "sum" }
      ]
    }
  ],
  "activeWidgetId": "daily-card-scan-trend"
}';

DECLARE @ExistingReportId BIGINT;

SELECT TOP (1) @ExistingReportId = ReportDefinition.Id
FROM RII_REPORT_DEFINITIONS ReportDefinition
WHERE ReportDefinition.Name = @ReportName
ORDER BY ReportDefinition.Id DESC;

IF @ExistingReportId IS NULL
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
        IsDeleted,
        CreatedBy
    )
    VALUES
    (
        @ReportName,
        @ReportDescription,
        N'CRM',
        N'function',
        N'dbo.RII_FN_USER_DAILY_CARD_PERFORMANCE',
        @ConfigJson,
        GETDATE(),
        0,
        1
    );

    SET @ExistingReportId = SCOPE_IDENTITY();
END
ELSE
BEGIN
    UPDATE RII_REPORT_DEFINITIONS
    SET
        Description = @ReportDescription,
        ConnectionKey = N'CRM',
        DataSourceType = N'function',
        DataSourceName = N'dbo.RII_FN_USER_DAILY_CARD_PERFORMANCE',
        ConfigJson = @ConfigJson,
        UpdatedDate = GETDATE(),
        UpdatedBy = 1,
        IsDeleted = 0,
        DeletedDate = NULL,
        DeletedBy = NULL
    WHERE Id = @ExistingReportId;
END;

UPDATE RII_REPORT_DEFINITIONS
SET
    IsDeleted = 1,
    DeletedDate = GETDATE(),
    DeletedBy = 1,
    UpdatedDate = GETDATE(),
    UpdatedBy = 1
WHERE Name = @ReportName
  AND Id <> @ExistingReportId
  AND IsDeleted = 0;

SELECT
    ReportDefinition.Id,
    ReportDefinition.Name,
    ReportDefinition.DataSourceType,
    ReportDefinition.DataSourceName
FROM RII_REPORT_DEFINITIONS ReportDefinition
WHERE ReportDefinition.Name = @ReportName
  AND ReportDefinition.IsDeleted = 0
ORDER BY ReportDefinition.Id DESC;
