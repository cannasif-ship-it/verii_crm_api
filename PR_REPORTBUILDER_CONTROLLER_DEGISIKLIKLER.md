# Report Builder Controller Değişiklikleri – Frontend Uyum Özeti

Aşağıdaki değişiklikler backend’de yapıldı. API route’ları aynı kaldı; yalnızca **response formatı** ve **HTTP status code** kullanımı DocumentSerialTypeController ile uyumlu hale getirildi.

---

## 1. Dosya yapısı

| Önce | Sonra |
|------|--------|
| `Controllers/ReportBuilderController/ReportingController.cs` (ayrı dosya) | Tüm endpoint’ler `Controllers/ReportBuilderController/ReportBuilderController.cs` içinde (tek dosyada iki controller sınıfı: `ReportingController` ve `ReportsController`) |
| `Controllers/ReportBuilderController/ReportsController.cs` (ayrı dosya) | (silindi, yukarıdaki dosyaya taşındı) |

**Frontend etkisi:** Yok. URL’ler değişmedi: `api/reporting/*` ve `api/reports/*` aynı.

---

## 2. `api/reporting/connections` – Bağlantı listesi

| Önce | Sonra |
|------|--------|
| Başarı: `200 OK`, body doğrudan **data** (bağlantı listesi). | Başarı: `result.StatusCode` (genelde 200), body **tüm ApiResponse** (`success`, `data`, `message`, vb.). |
| Hata: `result.StatusCode` ile **result** objesi dönüyordu. | Hata: Aynı – `StatusCode(result.StatusCode, result)`. |

**Frontend:** Artık her durumda body’de **ApiResponse** bekleyin; listeyi `response.data` içinden alın.

---

## 3. `api/reporting/datasources/check` – Data source kontrol

| Önce | Sonra |
|------|--------|
| `200 OK`, body: `{ exists, message, schema }` (DataSourceCheckResponseDto). | **Değişmedi.** Aynı response. |

**Frontend:** Değişiklik yok.

---

## 4. `api/reports` – Rapor CRUD ve liste

### POST (yeni rapor oluşturma)

| Önce | Sonra |
|------|--------|
| Başarı: `201 Created`, `CreatedAtAction` ile **result** (içinde `Data` ile oluşan rapor). | Başarı: `result.StatusCode` (genelde 201), body **tüm ApiResponse** (`success`, `data`, `message`, vb.). |
| Hata: `StatusCode(result.StatusCode, result)`. | Hata: Aynı. |

**Frontend:** Oluşan rapor bilgisi artık her zaman `response.data` içinde; `response` tam ApiResponse.

### GET (rapor listesi)

| Önce | Sonra |
|------|--------|
| Başarı: `200 OK`, body **result** (ApiResponse / paged result). | Başarı: `StatusCode(result.StatusCode, result)` – body aynı result, status code artık her zaman `result.StatusCode`. |
| Hata: `StatusCode(result.StatusCode, result)`. | Hata: Aynı. |

**Frontend:** Body formatı aynı; status code’u backend’in döndüğü `result.StatusCode` ile kullanın.

### GET `api/reports/{id}` (tek rapor)

| Önce | Sonra |
|------|--------|
| Başarı: `200 OK`, body **result** (ApiResponse). | Başarı: `StatusCode(result.StatusCode, result)`. |
| Hata: `StatusCode(result.StatusCode, result)`. | Hata: Aynı. |

**Frontend:** Body aynı; status code artık her zaman `result.StatusCode`.

### PUT `api/reports/{id}` (güncelleme)

| Önce | Sonra |
|------|--------|
| Başarı: `200 OK`, body **result**. | Başarı: `StatusCode(result.StatusCode, result)`. |
| Hata: `StatusCode(result.StatusCode, result)`. | Hata: Aynı. |

**Frontend:** Body aynı; status code `result.StatusCode`.

### DELETE `api/reports/{id}`

| Önce | Sonra |
|------|--------|
| Başarı: `200 OK`, body **result**. | Başarı: `StatusCode(result.StatusCode, result)`. |
| Hata: Aynı. | Hata: Aynı. |

**Frontend:** Body aynı; status code `result.StatusCode`.

---

## 5. `api/reports/preview` – Önizleme

| Önce | Sonra |
|------|--------|
| Başarı: `200 OK`, body **sadece preview data** (`result.Data`). | Başarı: `result.StatusCode` (genelde 200), body **tüm ApiResponse** (`success`, `data`, `message`, vb.). |
| Hata: `StatusCode(result.StatusCode, result)`. | Hata: Aynı. |

**Frontend:** Önizleme verisini artık `response.data` içinden alın; tüm cevap ApiResponse formatında.

---

## 6. Özet – Frontend’de yapılacaklar

1. **`api/reporting/connections`**  
   - Cevabı her zaman ApiResponse gibi işleyin; listeyi **`response.data`** üzerinden alın.

2. **`api/reports` (POST)**  
   - Oluşan raporu **`response.data`** içinden alın; status code’u response’un status’üne göre kullanın.

3. **`api/reports/preview`**  
   - Önizleme verisini **`response.data`** içinden alın; artık doğrudan body değil, sarmalı ApiResponse geliyor.

4. **Diğer endpoint’ler** (`api/reports` GET, GET by id, PUT, DELETE)  
   - Body formatı aynı; sadece HTTP status code artık her zaman backend’in döndüğü `result.StatusCode` ile uyumlu. Mevcut `result`/ApiResponse kullanımınız uyumluysa ek değişiklik gerekmez.

5. **`api/reporting/datasources/check`**  
   - Değişiklik yok; mevcut kullanım aynen devam edebilir.

Bu özeti PR açıklamasında veya frontend ekibine ileteceğiniz prompt olarak kullanabilirsiniz.
