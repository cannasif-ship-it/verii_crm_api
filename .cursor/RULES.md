# CRM Backend – .NET Core & Entity Framework Core
## Architecture, EF Core, Localization & Cursor Rules

> Bu doküman bağlayıcıdır.  
> Kurallara uymayan kod KABUL EDİLMEZ.

---

## 1. GENEL MİMARİ

Katmanlı mimari zorunludur:

Controllers  
→ Services  
→ UnitOfWork / Repositories  
→ Domain  
→ Data (EF Core)

### Controller
- Business logic içermez
- DbContext / Repository kullanmaz
- Transaction yönetmez
- Mapping yapmaz

### Service
- Tüm iş kuralları burada yazılır
- Transaction burada başlatılır
- SaveChanges sadece burada çağrılır

### Repository
- Sadece veri erişimi yapar
- Business rule, validation içermez

---

## 2. ENTITY FRAMEWORK CORE KURALLARI

- DbContext controller’da ASLA kullanılmaz
- Read-only sorgularda AsNoTracking ZORUNLU
- Entity doğrudan client’a dönülmez (DTO zorunlu)
- Gereksiz Include YASAK
- Projection tercih edilir

### Fluent Configuration
- DataAnnotation YASAK
- IEntityTypeConfiguration<T> zorunlu
- OnModelCreating içinde logic yazılmaz

---

## 3. BASE ENTITY & DTO

Yeni entity/DTO yazmadan önce kontrol edilir:

- BaseEntity
- BaseEntityDto
- BaseCreateDto
- BaseUpdateDto

Kurallar:
- Base class olmadan alan eklenemez
- Create/Update DTO’larda ERP açıklamaları yer alamaz

---

## 4. CRUD FELSEFESİ

- CRUD endpoint olabilir
- Operasyonel süreçlerde kullanılmaz
- Generate / Bulk / Route bazlı akışlar tercih edilir

---

## 5. TRANSACTION & UNIT OF WORK

- Transaction sadece Service katmanında
- Controller içinde transaction YASAK
- External API çağrıları transaction DIŞINDA yapılır

---

## 6. SOFT DELETE

- Hard delete KESİNLİKLE YASAK
- Tüm silmeler soft delete
- Global Query Filter kullanılır

---

## 7. PAGINATION / FILTER / SORT

- Tüm listeler PagedRequest alır
- Filtering JSON tabanlıdır
- Pagination / filter / sort tek helper üzerinden yapılır

---

## 8. VALIDATION vs BUSINESS RULE

- Validation: DTO seviyesinde
- Business rule: Service seviyesinde

HTTP Codes:
- Validation → 400
- Business rule → 400
- Sistem hatası → 500

---

## 9. AUTH & CONTEXT

- UserId, CompanyId, BranchCode client’tan alınmaz
- HttpContext üzerinden okunur
- Tüm endpoint’ler [Authorize] ile korunur

---

## 10. LOGGING & AUDIT

Otomatik alanlar:
- CreatedBy / CreatedAt
- UpdatedBy / UpdatedAt
- DeletedBy / DeletedAt

Client’a sadece localized mesaj döner

---

## 11. LOCALIZATION (ZORUNLU)

- Hardcoded string YASAK
- Tüm mesajlar Resources üzerinden gelir

Zorunlu dosyalar:
- Messages.tr-TR.resx
- Messages.en-US.resx

---

## 11.1 LOCALIZATION STRUCTURE (ZORUNLU FORMAT)

- Localization CLASS bazlıdır
- Her Service kendi region’ına sahiptir

ÖRNEK:

<!-- #region AuthService -->
<data name="AuthService.LoginFailed" xml:space="preserve">
  <value>Giriş işlemi başarısız.</value>
</data>

<data name="AuthService.LoginSuccess" xml:space="preserve">
  <value>Giriş başarılı.</value>
</data>
<!-- #endregion AuthService -->

Kurallar:
- Region adı Service adıyla birebir
- Key formatı: ServiceName.ActionOrRule

---

## 11.2 LOCALIZATION KULLANIMI

_localizationService.GetLocalizedString("AuthService.LoginFailed")

- ApiResponse.Message mutlaka localized olmalıdır
- Validation mesajları da localization kullanır

---

## 11.3 FALLBACK

- Key yoksa İngilizce döner
- Eksik key loglanır
- Uygulama çalışmaya devam eder

---

## 12. CURSOR (AI) RULES

Cursor:
- Bu dokümana %100 uyar
- Hardcoded string yazamaz
- Entity döndüren endpoint yazamaz
- Localization region açmadan Service oluşturamaz
- Soft delete kuralını bypass edemez
- Aynı helper’ı tekrar yazamaz

---

# ---------------------------------------------------------
# 12.4 PARAMETRELİ LOCALIZATION (ZORUNLU KURAL)
# ---------------------------------------------------------

- Localization mesajları DİNAMİK olarak string birleştirilerek üretilemez.
- Hardcoded string, interpolation veya string.Format kullanımı YASAKTIR.
- Değişken değerler localization key’ine PARAMETRE olarak gönderilir.

YANLIŞ:
ApiResponse.Error("File size exceeds 10 MB")

DOĞRU:
_localizationService.GetLocalizedString(
    "FileService.FileSizeExceeded",
    maxSizeMb
)

- Resource value içerisinde {0}, {1} ... placeholder kullanılır.
- Tüm dillerde placeholder sayısı birebir aynı olmalıdır.
- ExceptionMessage alanı localization içermez.

# ---------------------------------------------------------
# 12.5 DEBUG / EXCEPTION MESSAGE LOCALIZATION (ZORUNLU)
# ---------------------------------------------------------

- ApiResponse içinde dönen:
  - Message
  - Error
  - ExceptionMessage
  alanlarının TAMAMI localization üzerinden gelmelidir.

- ExceptionMessage için:
  - Hardcoded string
  - String interpolation
  - string.Format
  KESİNLİKLE YASAKTIR.

- Değişken bilgiler:
  - Localization key’ine PARAMETRE olarak gönderilir.

- Gerekirse:
  - Message
  - ExceptionMessage
  için AYRI localization key’leri tanımlanır.

- Debug amaçlı bile olsa localization dışı mesaj yazılamaz.

BU DOKÜMAN REFERANS ALINMADAN YAZILAN KOD KABUL EDİLMEZ.
