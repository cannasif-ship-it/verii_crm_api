# Revenue Quality ve NBA Workflow (v1)

Bu dokuman, Customer360 ve Salesmen360 icin gelir kalitesi metriklerinin:
- tanimini
- formullerini
- veri kaynaklarini
- kod izlenebilirligini
ve Next-Best-Action (NBA) kural + execute hattini aciklar.

## 1) Metrikler

### 1.1 CohortKey
- Tanim: Ilk temas ayi (`yyyy-MM`).
- Veri kaynagi:
  - Customer icin: `Orders`, `Quotations`, `Demands`
  - Salesman icin: `Orders`, `Quotations`, `Activities`
- Hesap:
  - Ilk temas tarihi = ilgili kaynaklardaki en erken tarih
  - `CohortKey = firstTouchDate.ToString("yyyy-MM")`
- Kod:
  - `Services/RevenueQualityService.cs` -> `CalculateCustomerRevenueQualityAsync`
  - `Services/RevenueQualityService.cs` -> `CalculateSalesmanRevenueQualityAsync`

### 1.2 RetentionRate
- Tanim: Ilk temastan bugune kadar aktif olunan aylarin orani (%).
- Veri kaynagi: `Orders` tarihleri.
- Formul:
  - `monthsSinceFirstTouch = max(1, monthDiff(firstTouchDate, now) + 1)`
  - `activeMonths = distinct(orderDate yyyy-MM).Count`
  - `RetentionRate = round(activeMonths / monthsSinceFirstTouch * 100, 2)`
- Kod:
  - `Services/RevenueQualityService.cs` -> `CalculateRetentionRate`

### 1.3 RfmSegment
- Tanim: Recency/Frequency/Monetary tabanli segment.
- Veri kaynagi: Son 12 ay `Orders`.
- Kural:
  - `Champions`: `recency <= 30` ve `frequency12 >= 6` ve `monetary12 >= 100000`
  - `Loyal`: `recency <= 60` ve `frequency12 >= 3`
  - `AtRisk`: `recency > 120` ve `frequency12 <= 1`
  - aksi: `Potential`
- Kod:
  - `Services/RevenueQualityService.cs` -> `CalculateRfmSegment`

### 1.4 Ltv
- Tanim: Kayitta kalan siparislerin toplami.
- Veri kaynagi: `Orders.GrandTotal`
- Formul: `Ltv = Sum(Orders.GrandTotal)`
- Kod:
  - `Services/RevenueQualityService.cs`

### 1.5 ChurnRiskScore
- Tanim: Musteriyi kaybetme riski (0-100).
- Veri kaynagi:
  - Customer: son siparis gunu, son aktivite gunu, son 12 ay siparis adedi
  - Salesman: portfoyde 90 gun dormant musteri oranı
- Formul (Customer):
  - Siparis recency skor katkisi: `+10/+20/+35/+45`
  - Aktivite recency katkisi: `+10/+20`
  - Dusuk frekans katkisi: `+10/+25`
  - Sonuc: `Clamp(0,100)`
- Formul (Salesman):
  - `churn = dormantCustomers90 / activeCustomers12 * 100` (aktif musteri 0 ise baz 50)
  - Sonuc: `Clamp(0,100)`
- Kod:
  - `Services/RevenueQualityService.cs` -> `CalculateCustomerChurnRisk`
  - `Services/RevenueQualityService.cs` -> `CalculateSalesmanRevenueQualityAsync`

### 1.6 UpsellPropensityScore
- Tanim: Ust urun/ek urun alma potansiyeli (0-100).
- Veri kaynagi:
  - Customer: siparis frekansi, teklif donusumu, ortalama siparis tutari, recency
  - Salesman: siparis, approved teklif, aktivite adedi
- Formul (Customer):
  - Baz `20`
  - Frekans, conversion, ortalama siparis tutari ve recency katkilarinin toplami
  - Sonuc: `Clamp(0,100)`
- Formul (Salesman):
  - Baz `25`
  - Siparis/approved teklif/aktivite esik puanlari
  - Sonuc: `Clamp(0,100)`
- Kod:
  - `Services/RevenueQualityService.cs` -> `CalculateCustomerUpsellPropensity`
  - `Services/RevenueQualityService.cs` -> `CalculateSalesmanRevenueQualityAsync`

### 1.7 PaymentBehaviorScore
- Tanim: Odeme davranisinin proxy skoru (0-100). (v1: ERP tahsilat gecmisi yok)
- Veri kaynagi: siparis adedi, teklif adedi, rejected teklif orani, recency.
- Formul:
  - Baz `50`
  - Siparis adedi ve recency pozitif katkilar
  - Rejected teklif orani negatif katkilar
  - Sonuc: `Clamp(0,100)`
- Kod:
  - `Services/RevenueQualityService.cs` -> `CalculatePaymentBehaviorScore`

## 2) NBA Kural Motoru

### 2.1 Kural degerlendirme
- Musteri NBA: `Services/NextBestActionService.cs` -> `GetCustomerActionsAsync`
- Salesman NBA: `Services/NextBestActionService.cs` -> `GetSalesmanActionsAsync`
- Cikti: `RecommendedActionDto[]` (en yuksek oncelikli ilk 5)

Her aksiyonda su alanlar dondurulur:
- `ActionCode`
- `Title`
- `Priority`
- `Reason`
- `DueDate`
- `TargetEntityType`
- `TargetEntityId`
- `SourceRuleCode`

### 2.2 Rule catalog (tek kaynak)
- Kod: `Services/NbaActionCatalog.cs`
- Amac:
  - Desteklenen `ActionCode` listesini sabitlemek
  - Varsayilan `Title`, `DefaultPriority`, `DefaultDueInDays`, `TargetEntityType` tanimi

## 3) Execute Hatti (Deterministic)

### 3.1 Endpointler
- Customer:
  - `POST /api/customer360/{id}/actions/execute`
  - Kod: `Services/CustomerService/Customer360Service.cs` -> `ExecuteRecommendedActionAsync`
- Salesman:
  - `POST /api/salesmen360/{userId}/actions/execute`
  - Kod: `Services/UserService/Salesmen360Service.cs` -> `ExecuteRecommendedActionAsync`

### 3.2 Davranis
- `ActionCode` bos ise `400`.
- `ActionCode` `NbaActionCatalog` icinde yoksa `400`.
- Entity tipi uyusmazsa (`Customer` vs `User`) `400`.
- `dueInDays`:
  - request varsa onu kullanir
  - yoksa katalog varsayilani
  - her durumda `0..30` araligina clamp edilir.
- `title/priority`:
  - request override eder
  - yoksa katalog varsayilani kullanilir.
- Sonuc:
  - `Activity` kaydi olusturulur ve `ActivityDto` doner.

## 4) Izlenebilirlik (Traceability)

Metrik veya aksiyonun nereden geldigini su sekilde takip edebilirsin:
- Metrik kaynagi:
  - `RevenueQualityDto` alanlari -> `Services/RevenueQualityService.cs`
- Kural kaynagi:
  - `SourceRuleCode` -> `Services/NextBestActionService.cs`
- Execute kaynagi:
  - `ActionCode` -> `Services/NbaActionCatalog.cs`
  - `Activity` olusumu -> `Customer360Service` / `Salesmen360Service`

## 5) Bilinen kisitlar (v1)

- PaymentBehavior su an proxy hesap; ERP tahsilat/gecikme satirlari modele dahil degil.
- NBA execute adimi su an aktivite olusturma odakli; farkli aksiyon tipleri (mail, webhook, kampanya tetikleme) sonraki fazda ayrilabilir.
- Kural agirliklari deterministic; ML/LLM tabanli skorlar eklenmemis.
