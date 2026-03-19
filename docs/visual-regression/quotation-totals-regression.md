# Quotation Totals Visual Regression

Bu akısin amacı, builder preview semantiğini temsil eden referans PNG ile aday PDF sayfasını aynı anchor bölgeleri üzerinden kıyaslamaktır.

## 1. Referans PNG üret

```bash
python3 /Users/cannasif/Documents/V3rii/verii_crm_api/scripts/render_quotation_totals_reference.py \
  /Users/cannasif/Documents/V3rii/pdf-samples/quotation-totals-regression-sample.json \
  /tmp/quotation-totals-reference.png
```

## 2. Aday PDF üret

İstersen gerçek builder çıktını kullan, istersen aynı generic renderer semantiğinden otomatik aday PDF üret:

```bash
dotnet run --project /Users/cannasif/Documents/V3rii/verii_crm_api/scripts/PdfRegressionSampleRunner/PdfRegressionSampleRunner.csproj -- \
  /Users/cannasif/Documents/V3rii/pdf-samples/quotation-totals-regression-sample.json \
  /tmp/quotation-totals-candidate.pdf
```

## 3. Görsel diff al

```bash
python3 /Users/cannasif/Documents/V3rii/verii_crm_api/scripts/pdf_visual_diff.py \
  /tmp/quotation-totals-reference.png \
  /tmp/quotation-totals-candidate.pdf \
  --layout-spec /Users/cannasif/Documents/V3rii/pdf-samples/quotation-totals-layout-spec.json \
  --output-dir /tmp/quotation-totals-diff
```

## Beklenen çıktı

- `report.json`
- `diff-page-001.png`
- anchor bazlı:
  - `quotation_totals_block`
  - `quotation_totals_note`

Bu sayede totals grid ve totals altı note kompozisyonu preview/PDF parity açısından ölçülebilir olur.
