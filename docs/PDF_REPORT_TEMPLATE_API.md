# PDF Report Template API (Report-Builder Discipline)

## Yeni endpoint listesi (frontend `/report-designer` için)

| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | `/api/pdf-report-templates` | Sayfalı liste (query: search, pageNumber, pageSize, ruleType, isActive) |
| GET | `/api/pdf-report-templates/{id}` | Tek şablon |
| POST | `/api/pdf-report-templates` | Yeni şablon oluştur |
| PUT | `/api/pdf-report-templates/{id}` | Şablon güncelle |
| DELETE | `/api/pdf-report-templates/{id}` | Şablon sil (soft delete) |
| POST | `/api/pdf-report-templates/generate-document` | PDF üret |
| GET | `/api/pdf-report-templates/fields/{ruleType}` | ruleType için kullanılabilir alanlar (0=Demand, 1=Quotation, 2=Order) |

## Eski → Yeni mapping (backward compatibility)

| Eski (deprecated) | Yeni |
|-------------------|------|
| `GET /api/ReportTemplate` | `GET /api/pdf-report-templates` |
| `GET /api/ReportTemplate/{id}` | `GET /api/pdf-report-templates/{id}` |
| `POST /api/ReportTemplate` | `POST /api/pdf-report-templates` |
| `PUT /api/ReportTemplate/{id}` | `PUT /api/pdf-report-templates/{id}` |
| `DELETE /api/ReportTemplate/{id}` | `DELETE /api/pdf-report-templates/{id}` |
| `POST /api/ReportTemplate/generate-pdf` | `POST /api/pdf-report-templates/generate-document` |
| `GET /api/ReportTemplate/fields/{ruleType}` | `GET /api/pdf-report-templates/fields/{ruleType}` |

Eski endpointler çalışır durumda bırakıldı; yeni entegrasyonlar için `/api/pdf-report-templates` kullanılmalıdır.

## Request / Response örnekleri

### GET /api/pdf-report-templates?pageNumber=1&pageSize=10&ruleType=1

**Response 200:**
```json
{
  "success": true,
  "message": "...",
  "data": {
    "items": [
      {
        "id": 1,
        "ruleType": 1,
        "title": "Teklif şablonu",
        "templateData": {
          "page": { "width": 794, "height": 1123, "unit": "px" },
          "elements": [
            { "id": "el1", "type": "text", "x": 10, "y": 20, "width": 200, "height": 24, "zIndex": 0, "text": "Başlık", "fontSize": 14 }
          ]
        },
        "isActive": true,
        "default": true,
        "createdByUserId": 1,
        "updatedByUserId": null,
        "createdDate": "2025-02-10T12:00:00Z",
        "updatedDate": null
      }
    ],
    "totalCount": 1,
    "pageNumber": 1,
    "pageSize": 10
  },
  "statusCode": 200
}
```

### POST /api/pdf-report-templates (Create)

**Request body:**
```json
{
  "ruleType": 1,
  "title": "Yeni teklif şablonu",
  "templateData": {
    "page": { "width": 794, "height": 1123, "unit": "px" },
    "elements": [
      { "id": "title", "type": "text", "x": 0, "y": 0, "width": 300, "height": 30, "zIndex": 0, "text": "Teklif", "fontSize": 18 }
    ]
  },
  "isActive": true,
  "default": false
}
```

### POST /api/pdf-report-templates/generate-document

**Request body:**
```json
{
  "templateId": 1,
  "entityId": 42
}
```

**Response:** `200 OK` with `Content-Type: application/pdf` and binary PDF body.

### Template data contract (ReportTemplateData)

- **page**: `width`, `height`, `unit` (px, pt, in, mm)
- **elements**: array of:
  - `id`, `type` (text | field | image | table), `x`, `y`, `width`, `height`, `zIndex`, `rotation`, `style`, `binding`
  - text: `text`; field: `path`; image: `value` (data: URI veya allowlist’teki URL); table: `columns`, `headerStyle`, `rowStyle`, `alternateRowStyle`, `columnWidths`
  - `textOverflow`: wrap | ellipsis | clip | autoHeight
- **table**: `columns` (label, path), `columnWidths` (opsiyonel)

JSON serializer: camelCase (sabit).

## Güvenlik ve doğrulama

- **User claim:** `TryParse` ile güvenli parse; geçersiz/eksik ise 401.
- **Template update default:** `currentWasDefault` atanmadan önce okunur (invariant korunur).
- **Generate PDF:** Kimlik doğrulama zorunlu (yeni API); 401 döner.
- **SSRF (görsel):** Varsayılan olarak sadece `data:` image kabul edilir. Harici URL için `PdfBuilder:AllowlistedImageHosts` ile allowlist; localhost/private IP engelli; timeout ve max boyut `PdfBuilder` ile yapılandırılır.

## Appsettings (opsiyonel)

```json
"PdfBuilder": {
  "AllowlistedImageHosts": [],
  "ImageFetchTimeoutSeconds": 5,
  "MaxImageSizeBytes": 5242880,
  "AllowedImageContentTypes": ["image/png", "image/jpeg", "image/gif", "image/webp"]
}
```

Bölüm yoksa varsayılan değerler kullanılır; sadece `data:` görseller kabul edilir.

## Sınıf / arayüz eşlemesi

| Eski | Yeni (PDF prefix) |
|------|-------------------|
| ReportTemplateController | PdfReportTemplateController |
| IReportTemplateService | IPdfReportTemplateService |
| ReportTemplateService | PdfReportTemplateService (+ legacy adapter) |
| IReportPdfGeneratorService | IPdfReportDocumentGeneratorService |
| ReportPdfGeneratorService | PdfReportDocumentGeneratorService (legacy → delegate) |
| ReportTemplateDto / Create / Update | PdfReportTemplateDto, CreatePdfReportTemplateDto, UpdatePdfReportTemplateDto |
| GeneratePdfRequest | Aynı (GeneratePdfRequest) |

Validator: `IPdfReportTemplateValidator` / `PdfReportTemplateValidator` (Create, Update, Generate için merkezi doğrulama).
