# PDF Report Template API — Production Ready

## Değişen endpoint tablosu

| Method | Endpoint | Değişiklik |
|--------|----------|------------|
| GET | `/api/pdf-report-templates` | +`search` query; `pageNumber`, `pageSize`, `ruleType`, `isActive` |
| GET | `/api/pdf-report-templates/{id}` | — |
| POST | `/api/pdf-report-templates` | — |
| PUT | `/api/pdf-report-templates/{id}` | — |
| DELETE | `/api/pdf-report-templates/{id}` | — |
| POST | `/api/pdf-report-templates/generate-document` | — |
| GET | `/api/pdf-report-templates/fields/{ruleType}` | — |

**GET list query parametreleri:**
- `search` (string, opsiyonel): Title üzerinde case-insensitive contains
- `pageNumber` (int, default: 1)
- `pageSize` (int, default: 10)
- `ruleType` (0|1|2, opsiyonel): Demand=0, Quotation=1, Order=2
- `isActive` (bool, opsiyonel)

**Response shape:** Tüm endpointler `ApiResponse<T>` + `Data` yapısında tutarlı.

---

## Eski → Yeni davranış farkları

| Alan | Eski (/api/ReportTemplate) | Yeni (/api/pdf-report-templates) |
|------|----------------------------|-----------------------------------|
| Deprecation | — | `X-API-Deprecated: true`, `X-API-Replacement: /api/pdf-report-templates` |
| List search | Yok | `search` query ile Title filtresi |
| List request | PagedRequest | PdfReportTemplateListRequest (search + page + filters) |
| PDF layout | Column/cumulativeY (akış tabanlı) | Layers ile gerçek absolute (x,y,w,h) |
| Element render | Flow + padding simülasyonu | zIndex, rotation, opacity, background, border, padding, borderRadius |
| Text | wrap only | wrap, ellipsis, clip; lineHeight, letterSpacing, textAlign, verticalAlign |
| Image fetch | Sync-over-async (GetAwaiter) | Tam async pre-fetch; Content-Type magic bytes doğrulaması |
| SSRF log | Plain warning | Structured: Reason, Url, SourceLength |
| Validator | Temel | +columnWidths range, +opacity/rotation/lineHeight range, +overlap warning |
| Table | Tek sayfa | repeatHeader, pageBreak (TableOptions) |

---

## Kalan teknik borç listesi (max 10)

1. **Golden PDF snapshot test** — Temel layout değişikliklerini tespit etmek için baseline PDF karşılaştırması henüz yok.
2. **Table page-break + header repeat** — QuestPDF API ile tam entegrasyon; bazı tablo ayarları render'da henüz kullanılmıyor.
3. **Integration test (generate-document)** — Gerçek DB + HTTP context ile end-to-end test eksik.
4. **Role-based access** — Generate PDF için yalnızca auth kontrolü var; entity (Quotation/Demand/Order) sahipliği/rol kontrolü yok.
5. **Rate limiting** — PDF generate endpoint için throttle/limit uygulanmıyor.
6. **Caching** — Sık kullanılan template'ler için deserialization cache yok.
7. **async RenderImage** — Document.Create senkron olduğu için image'lar pre-fetch ediliyor; uzun listelerde memory tüketimi gözden geçirilmeli.
8. **Content-Type HTTP header** — URL ile alınan image’larda response Content-Type doğrulaması sadece magic bytes ile; header kontrolü eklenebilir.
9. **Placement warnings API** — Validator.GetPlacementWarnings Create/Update pipeline’da çağrılmıyor; opsiyonel uyarı olarak response’a eklenebilir.
10. **Table columnWidths birimi** — Şu an page unit kullanılıyor; column bazlı unit desteği yok.

---

## Yapılan iyileştirmeler özeti

- **API:** search + tutarlı query parametreleri
- **Legacy:** Deprecation header
- **PDF render:** Gerçek absolute layout (Layers), zIndex, rotation, opacity, background, border, padding, borderRadius
- **Text:** wrap/ellipsis/clip, lineHeight, letterSpacing, textAlign, verticalAlign
- **Image:** Full async pre-fetch, Content-Type magic-bytes doğrulaması, structured SSRF logs
- **Validator:** columnWidths range, style numeric ranges, overlap warning
- **Tests:** Unit (validator, image URL validator, unit conversion)
