# PDF Image Tasima Rehberi

Bu rehber sadece su hedef icin hazirlandi:

- `RII_REPORT_TEMPLATES` tablosu oldugu gibi kalacak
- PDF builder ve report builder image metadata kayitlari `RII_PDF_IMAGES` tablosuna gidecek
- Hizli teklif image metadata kayitlari `RII_QUICK_QUOTATION_IMAGES` tablosuna gidecek

Asagidaki adimlari sirayla uygula.

## Adim 0 - Once migration'i uygula

Asagidaki komutu calistir:

```bash
cd /Users/cannasif/Documents/V3rii/verii_crm_api
dotnet ef database update --context CmsDbContext --project crm_api.csproj
```

Bu komut su tablolari olusturur:

- `RII_PDF_IMAGES`
- `RII_QUICK_QUOTATION_IMAGES`

---

## Adim 1 - PDF builder path'lerini guncelle

Bunu komple kopyala yapistir:

```sql
UPDATE RII_REPORT_TEMPLATES
SET TemplateJson = REPLACE(
    TemplateJson,
    '/uploads/pdf-template-assets/templates/',
    '/uploads/pdf-designer/'
)
WHERE RuleType <> 3
  AND IsDeleted = 0
  AND TemplateJson LIKE '%/uploads/pdf-template-assets/templates/%';

UPDATE RII_REPORT_TEMPLATES
SET TemplateJson = REPLACE(
    TemplateJson,
    '/uploads/pdf-template-assets/',
    '/uploads/pdf-designer/'
)
WHERE RuleType <> 3
  AND IsDeleted = 0
  AND TemplateJson LIKE '%/uploads/pdf-template-assets/%'
  AND TemplateJson NOT LIKE '%/uploads/pdf-template-assets/templates/%'
  AND TemplateJson NOT LIKE '%/uploads/pdf-template-assets/quick-quotation/%';
```

Bu adim ne yapar:

- normal PDF builder kayitlarindaki eski image path'lerini
- yeni `/uploads/pdf-designer/{id}/...` formatina cevirir

---

## Adim 2 - Hizli teklif path'lerini guncelle

Bunu komple kopyala yapistir:

```sql
UPDATE RII_REPORT_TEMPLATES
SET TemplateJson = REPLACE(
    TemplateJson,
    '/uploads/pdf-template-assets/templates/',
    '/uploads/quick-quotation/'
)
WHERE RuleType = 3
  AND IsDeleted = 0
  AND TemplateJson LIKE '%/uploads/pdf-template-assets/templates/%';

UPDATE RII_REPORT_TEMPLATES
SET TemplateJson = REPLACE(
    TemplateJson,
    '/uploads/pdf-template-assets/quick-quotation/',
    '/uploads/quick-quotation/'
)
WHERE RuleType = 3
  AND IsDeleted = 0
  AND TemplateJson LIKE '%/uploads/pdf-template-assets/quick-quotation/%';
```

Bu adim ne yapar:

- quick quotation kayitlarindaki eski image path'lerini
- yeni `/uploads/quick-quotation/{id}/...` formatina cevirir

---

## Adim 3 - PDF builder image metadata'larini yeni tabloya bas

Bunu komple kopyala yapistir:

```sql
;WITH ImageElements AS
(
    SELECT
        rt.Id AS ReportTemplateId,
        rt.CreatedBy,
        rt.CreatedDate,
        JSON_VALUE(j.[value], '$.value') AS RelativeUrl
    FROM RII_REPORT_TEMPLATES rt
    CROSS APPLY OPENJSON(rt.TemplateJson, '$.elements') j
    WHERE rt.IsDeleted = 0
      AND rt.RuleType <> 3
      AND JSON_VALUE(j.[value], '$.type') = 'image'
      AND JSON_VALUE(j.[value], '$.value') LIKE '/uploads/%'
)
INSERT INTO RII_PDF_IMAGES
(
    OriginalFileName,
    StoredFileName,
    RelativeUrl,
    ContentType,
    SizeBytes,
    CreatedDate,
    IsDeleted,
    CreatedBy
)
SELECT
    RIGHT(RelativeUrl, CHARINDEX('/', REVERSE(RelativeUrl)) - 1) AS OriginalFileName,
    RIGHT(RelativeUrl, CHARINDEX('/', REVERSE(RelativeUrl)) - 1) AS StoredFileName,
    RelativeUrl,
    CASE
        WHEN RelativeUrl LIKE '%.png'  THEN 'image/png'
        WHEN RelativeUrl LIKE '%.gif'  THEN 'image/gif'
        WHEN RelativeUrl LIKE '%.webp' THEN 'image/webp'
        WHEN RelativeUrl LIKE '%.jpeg' THEN 'image/jpeg'
        WHEN RelativeUrl LIKE '%.jpg'  THEN 'image/jpeg'
        ELSE 'application/octet-stream'
    END AS ContentType,
    0 AS SizeBytes,
    CreatedDate,
    0 AS IsDeleted,
    CreatedBy
FROM ImageElements src
WHERE src.RelativeUrl IS NOT NULL
  AND NOT EXISTS
  (
      SELECT 1
      FROM RII_PDF_IMAGES tgt
      WHERE tgt.RelativeUrl = src.RelativeUrl
        AND tgt.IsDeleted = 0
  );
```

Bu adim ne yapar:

- `RII_REPORT_TEMPLATES` icindeki image elementlerini parse eder
- normal PDF builder ve report builder image metadata'sini
- `RII_PDF_IMAGES` tablosuna yazar

---

## Adim 4 - Hizli teklif image metadata'larini yeni tabloya bas

Bunu komple kopyala yapistir:

```sql
;WITH ImageElements AS
(
    SELECT
        rt.Id AS ReportTemplateId,
        rt.CreatedBy,
        rt.CreatedDate,
        JSON_VALUE(j.[value], '$.value') AS RelativeUrl
    FROM RII_REPORT_TEMPLATES rt
    CROSS APPLY OPENJSON(rt.TemplateJson, '$.elements') j
    WHERE rt.IsDeleted = 0
      AND rt.RuleType = 3
      AND JSON_VALUE(j.[value], '$.type') = 'image'
      AND JSON_VALUE(j.[value], '$.value') LIKE '/uploads/%'
)
INSERT INTO RII_QUICK_QUOTATION_IMAGES
(
    OriginalFileName,
    StoredFileName,
    RelativeUrl,
    ContentType,
    SizeBytes,
    CreatedDate,
    IsDeleted,
    CreatedBy
)
SELECT
    RIGHT(RelativeUrl, CHARINDEX('/', REVERSE(RelativeUrl)) - 1) AS OriginalFileName,
    RIGHT(RelativeUrl, CHARINDEX('/', REVERSE(RelativeUrl)) - 1) AS StoredFileName,
    RelativeUrl,
    CASE
        WHEN RelativeUrl LIKE '%.png'  THEN 'image/png'
        WHEN RelativeUrl LIKE '%.gif'  THEN 'image/gif'
        WHEN RelativeUrl LIKE '%.webp' THEN 'image/webp'
        WHEN RelativeUrl LIKE '%.jpeg' THEN 'image/jpeg'
        WHEN RelativeUrl LIKE '%.jpg'  THEN 'image/jpeg'
        ELSE 'application/octet-stream'
    END AS ContentType,
    0 AS SizeBytes,
    CreatedDate,
    0 AS IsDeleted,
    CreatedBy
FROM ImageElements src
WHERE src.RelativeUrl IS NOT NULL
  AND NOT EXISTS
  (
      SELECT 1
      FROM RII_QUICK_QUOTATION_IMAGES tgt
      WHERE tgt.RelativeUrl = src.RelativeUrl
        AND tgt.IsDeleted = 0
  );
```

Bu adim ne yapar:

- `RII_REPORT_TEMPLATES` icindeki quick quotation image elementlerini parse eder
- `RII_QUICK_QUOTATION_IMAGES` tablosuna metadata yazar

---

## Adim 5 - Kontrol sorgulari

PDF builder image sayisi:

```sql
SELECT COUNT(*) AS PdfImageCount
FROM RII_PDF_IMAGES
WHERE IsDeleted = 0;
```

Quick quotation image sayisi:

```sql
SELECT COUNT(*) AS QuickQuotationImageCount
FROM RII_QUICK_QUOTATION_IMAGES
WHERE IsDeleted = 0;
```

Ornek PDF builder kayitlari:

```sql
SELECT TOP 20 Id, RelativeUrl, OriginalFileName, CreatedDate
FROM RII_PDF_IMAGES
WHERE IsDeleted = 0
ORDER BY Id DESC;
```

Ornek quick quotation kayitlari:

```sql
SELECT TOP 20 Id, RelativeUrl, OriginalFileName, CreatedDate
FROM RII_QUICK_QUOTATION_IMAGES
WHERE IsDeleted = 0
ORDER BY Id DESC;
```

---

## Adim 6 - Fiziksel dosya kontrolu

Bu SQL'leri calistirdiktan sonra fiziksel dosyalari da su klasorlerde tutman gerekir:

- PDF builder: `/uploads/pdf-designer/{id}/...`
- Hizli teklif: `/uploads/quick-quotation/{id}/...`

Eger metadata var ama fiziksel dosya yoksa resim yine gozukmez.

---

## Kisa Ozet

Sirayla sadece sunlari yap:

1. migration uygula
2. PDF builder path update SQL
3. quick quotation path update SQL
4. `RII_PDF_IMAGES` insert-select
5. `RII_QUICK_QUOTATION_IMAGES` insert-select
6. kontrol sorgulari
