using System.Collections.Generic;

namespace crm_api.DTOs
{
    /// <summary>
    /// Frontend için kullanılabilir tüm field'ları içeren DTO
    /// Bu DTO template designer'da field palette'ini oluşturmak için kullanılır
    /// </summary>
    public class ReportTemplateFieldsDto
    {
        /// <summary>
        /// Header/Root seviye field'lar (Demand, Quotation, Order için ortak)
        /// </summary>
        public List<FieldDefinition> HeaderFields { get; set; } = new List<FieldDefinition>();

        /// <summary>
        /// Line (satır) seviye field'lar - Tablolarda kullanılır
        /// </summary>
        public List<FieldDefinition> LineFields { get; set; } = new List<FieldDefinition>();
    }

    /// <summary>
    /// Tek bir field tanımı
    /// </summary>
    public class FieldDefinition
    {
        /// <summary>
        /// Field'ın görünen adı (Türkçe)
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Field'ın DTO'daki path'i (örn: "OfferNo", "Lines.ProductName")
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Field'ın veri tipi (string, number, date, boolean)
        /// </summary>
        public string DataType { get; set; } = "string";

        /// <summary>
        /// Field açıklaması
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Örnek değer
        /// </summary>
        public string? ExampleValue { get; set; }
    }

    /// <summary>
    /// Demand için field'lar
    /// </summary>
    public static class DemandFields
    {
        public static ReportTemplateFieldsDto GetFields()
        {
            return new ReportTemplateFieldsDto
            {
                HeaderFields = new List<FieldDefinition>
                {
                    // Temel Bilgiler
                    new FieldDefinition { Label = "Talep No", Path = "OfferNo", DataType = "string", ExampleValue = "TLP-2024-001" },
                    new FieldDefinition { Label = "Talep Tarihi", Path = "OfferDate", DataType = "date", ExampleValue = "2024-01-15" },
                    new FieldDefinition { Label = "Talep Tipi", Path = "OfferType", DataType = "string", ExampleValue = "Yurtiçi" },
                    new FieldDefinition { Label = "Revizyon No", Path = "RevisionNo", DataType = "string", ExampleValue = "REV-001" },
                    
                    // Müşteri Bilgileri
                    new FieldDefinition { Label = "Müşteri Adı", Path = "PotentialCustomerName", DataType = "string", ExampleValue = "ABC Şirketi" },
                    new FieldDefinition { Label = "Müşteri Kodu (ERP)", Path = "ErpCustomerCode", DataType = "string", ExampleValue = "CUST001" },
                    
                    // Teslimat Bilgileri
                    new FieldDefinition { Label = "Teslimat Tarihi", Path = "DeliveryDate", DataType = "date", ExampleValue = "2024-02-15" },
                    new FieldDefinition { Label = "Teslimat Adresi", Path = "ShippingAddressText", DataType = "string", ExampleValue = "İstanbul, Türkiye" },
                    
                    // Diğer Bilgiler
                    new FieldDefinition { Label = "Satış Temsilcisi", Path = "RepresentativeName", DataType = "string", ExampleValue = "Ahmet Yılmaz" },
                    new FieldDefinition { Label = "Ödeme Tipi", Path = "PaymentTypeName", DataType = "string", ExampleValue = "Vadeli" },
                    new FieldDefinition { Label = "Para Birimi", Path = "Currency", DataType = "string", ExampleValue = "TRY" },
                    new FieldDefinition { Label = "Açıklama", Path = "Description", DataType = "string", ExampleValue = "Acil talep" },
                    new FieldDefinition { Label = "Belge Seri Tipi", Path = "DocumentSerialTypeName", DataType = "string", ExampleValue = "A Serisi" },
                    
                    // Oluşturma Bilgileri
                    new FieldDefinition { Label = "Oluşturan", Path = "CreatedBy", DataType = "string", ExampleValue = "Mehmet Demir" },
                    new FieldDefinition { Label = "Güncelleyen", Path = "UpdatedBy", DataType = "string", ExampleValue = "Ayşe Kaya" },
                },
                LineFields = new List<FieldDefinition>
                {
                    // Ürün Bilgileri
                    new FieldDefinition { Label = "Ürün Kodu", Path = "Lines.ProductCode", DataType = "string", ExampleValue = "PRD-001" },
                    new FieldDefinition { Label = "Ürün Adı", Path = "Lines.ProductName", DataType = "string", ExampleValue = "Laptop" },
                    new FieldDefinition { Label = "Grup Kodu", Path = "Lines.GroupCode", DataType = "string", ExampleValue = "GRP-001" },
                    
                    // Miktar ve Fiyat
                    new FieldDefinition { Label = "Miktar", Path = "Lines.Quantity", DataType = "number", ExampleValue = "10" },
                    new FieldDefinition { Label = "Birim Fiyat", Path = "Lines.UnitPrice", DataType = "number", ExampleValue = "5000.00" },
                    
                    // İndirimler
                    new FieldDefinition { Label = "İndirim Oranı 1 (%)", Path = "Lines.DiscountRate1", DataType = "number", ExampleValue = "10" },
                    new FieldDefinition { Label = "İndirim Tutarı 1", Path = "Lines.DiscountAmount1", DataType = "number", ExampleValue = "500.00" },
                    new FieldDefinition { Label = "İndirim Oranı 2 (%)", Path = "Lines.DiscountRate2", DataType = "number", ExampleValue = "5" },
                    new FieldDefinition { Label = "İndirim Tutarı 2", Path = "Lines.DiscountAmount2", DataType = "number", ExampleValue = "250.00" },
                    new FieldDefinition { Label = "İndirim Oranı 3 (%)", Path = "Lines.DiscountRate3", DataType = "number", ExampleValue = "2" },
                    new FieldDefinition { Label = "İndirim Tutarı 3", Path = "Lines.DiscountAmount3", DataType = "number", ExampleValue = "100.00" },
                    
                    // KDV
                    new FieldDefinition { Label = "KDV Oranı (%)", Path = "Lines.VatRate", DataType = "number", ExampleValue = "18" },
                    new FieldDefinition { Label = "KDV Tutarı", Path = "Lines.VatAmount", DataType = "number", ExampleValue = "900.00" },
                    
                    // Toplamlar
                    new FieldDefinition { Label = "Satır Toplamı (KDV Hariç)", Path = "Lines.LineTotal", DataType = "number", ExampleValue = "45000.00" },
                    new FieldDefinition { Label = "Satır Genel Toplamı (KDV Dahil)", Path = "Lines.LineGrandTotal", DataType = "number", ExampleValue = "53100.00" },
                    
                    // Diğer
                    new FieldDefinition { Label = "Açıklama", Path = "Lines.Description", DataType = "string", ExampleValue = "Özel not" },
                }
            };
        }
    }

    /// <summary>
    /// Quotation için field'lar
    /// </summary>
    public static class QuotationFields
    {
        public static ReportTemplateFieldsDto GetFields()
        {
            return new ReportTemplateFieldsDto
            {
                HeaderFields = new List<FieldDefinition>
                {
                    // Temel Bilgiler
                    new FieldDefinition { Label = "Teklif No", Path = "OfferNo", DataType = "string", ExampleValue = "TKL-2024-001" },
                    new FieldDefinition { Label = "Teklif Tarihi", Path = "OfferDate", DataType = "date", ExampleValue = "2024-01-15" },
                    new FieldDefinition { Label = "Teklif Tipi", Path = "OfferType", DataType = "string", ExampleValue = "Yurtiçi" },
                    new FieldDefinition { Label = "Revizyon No", Path = "RevisionNo", DataType = "string", ExampleValue = "REV-001" },
                    
                    // Müşteri Bilgileri
                    new FieldDefinition { Label = "Müşteri Adı", Path = "PotentialCustomerName", DataType = "string", ExampleValue = "ABC Şirketi" },
                    new FieldDefinition { Label = "Müşteri Kodu (ERP)", Path = "ErpCustomerCode", DataType = "string", ExampleValue = "CUST001" },
                    
                    // Teslimat Bilgileri
                    new FieldDefinition { Label = "Teslimat Tarihi", Path = "DeliveryDate", DataType = "date", ExampleValue = "2024-02-15" },
                    new FieldDefinition { Label = "Teslimat Adresi", Path = "ShippingAddressText", DataType = "string", ExampleValue = "İstanbul, Türkiye" },
                    
                    // Diğer Bilgiler
                    new FieldDefinition { Label = "Satış Temsilcisi", Path = "RepresentativeName", DataType = "string", ExampleValue = "Ahmet Yılmaz" },
                    new FieldDefinition { Label = "Ödeme Tipi", Path = "PaymentTypeName", DataType = "string", ExampleValue = "Vadeli" },
                    new FieldDefinition { Label = "Para Birimi", Path = "Currency", DataType = "string", ExampleValue = "TRY" },
                    new FieldDefinition { Label = "Açıklama", Path = "Description", DataType = "string", ExampleValue = "Özel teklif" },
                    new FieldDefinition { Label = "Belge Seri Tipi", Path = "DocumentSerialTypeName", DataType = "string", ExampleValue = "A Serisi" },
                    
                    // Oluşturma Bilgileri
                    new FieldDefinition { Label = "Oluşturan", Path = "CreatedBy", DataType = "string", ExampleValue = "Mehmet Demir" },
                    new FieldDefinition { Label = "Güncelleyen", Path = "UpdatedBy", DataType = "string", ExampleValue = "Ayşe Kaya" },
                },
                LineFields = new List<FieldDefinition>
                {
                    // Ürün Bilgileri
                    new FieldDefinition { Label = "Ürün Kodu", Path = "Lines.ProductCode", DataType = "string", ExampleValue = "PRD-001" },
                    new FieldDefinition { Label = "Ürün Adı", Path = "Lines.ProductName", DataType = "string", ExampleValue = "Laptop" },
                    new FieldDefinition { Label = "Grup Kodu", Path = "Lines.GroupCode", DataType = "string", ExampleValue = "GRP-001" },
                    
                    // Miktar ve Fiyat
                    new FieldDefinition { Label = "Miktar", Path = "Lines.Quantity", DataType = "number", ExampleValue = "10" },
                    new FieldDefinition { Label = "Birim Fiyat", Path = "Lines.UnitPrice", DataType = "number", ExampleValue = "5000.00" },
                    
                    // İndirimler
                    new FieldDefinition { Label = "İndirim Oranı 1 (%)", Path = "Lines.DiscountRate1", DataType = "number", ExampleValue = "10" },
                    new FieldDefinition { Label = "İndirim Tutarı 1", Path = "Lines.DiscountAmount1", DataType = "number", ExampleValue = "500.00" },
                    new FieldDefinition { Label = "İndirim Oranı 2 (%)", Path = "Lines.DiscountRate2", DataType = "number", ExampleValue = "5" },
                    new FieldDefinition { Label = "İndirim Tutarı 2", Path = "Lines.DiscountAmount2", DataType = "number", ExampleValue = "250.00" },
                    new FieldDefinition { Label = "İndirim Oranı 3 (%)", Path = "Lines.DiscountRate3", DataType = "number", ExampleValue = "2" },
                    new FieldDefinition { Label = "İndirim Tutarı 3", Path = "Lines.DiscountAmount3", DataType = "number", ExampleValue = "100.00" },
                    
                    // KDV
                    new FieldDefinition { Label = "KDV Oranı (%)", Path = "Lines.VatRate", DataType = "number", ExampleValue = "18" },
                    new FieldDefinition { Label = "KDV Tutarı", Path = "Lines.VatAmount", DataType = "number", ExampleValue = "900.00" },
                    
                    // Toplamlar
                    new FieldDefinition { Label = "Satır Toplamı (KDV Hariç)", Path = "Lines.LineTotal", DataType = "number", ExampleValue = "45000.00" },
                    new FieldDefinition { Label = "Satır Genel Toplamı (KDV Dahil)", Path = "Lines.LineGrandTotal", DataType = "number", ExampleValue = "53100.00" },
                    
                    // Diğer
                    new FieldDefinition { Label = "Açıklama", Path = "Lines.Description", DataType = "string", ExampleValue = "Özel not" },
                }
            };
        }
    }

    /// <summary>
    /// Order için field'lar
    /// </summary>
    public static class OrderFields
    {
        public static ReportTemplateFieldsDto GetFields()
        {
            return new ReportTemplateFieldsDto
            {
                HeaderFields = new List<FieldDefinition>
                {
                    // Temel Bilgiler
                    new FieldDefinition { Label = "Sipariş No", Path = "OfferNo", DataType = "string", ExampleValue = "SIP-2024-001" },
                    new FieldDefinition { Label = "Sipariş Tarihi", Path = "OfferDate", DataType = "date", ExampleValue = "2024-01-15" },
                    new FieldDefinition { Label = "Sipariş Tipi", Path = "OfferType", DataType = "string", ExampleValue = "Yurtiçi" },
                    new FieldDefinition { Label = "Revizyon No", Path = "RevisionNo", DataType = "string", ExampleValue = "REV-001" },
                    
                    // Müşteri Bilgileri
                    new FieldDefinition { Label = "Müşteri Adı", Path = "PotentialCustomerName", DataType = "string", ExampleValue = "ABC Şirketi" },
                    new FieldDefinition { Label = "Müşteri Kodu (ERP)", Path = "ErpCustomerCode", DataType = "string", ExampleValue = "CUST001" },
                    
                    // Teslimat Bilgileri
                    new FieldDefinition { Label = "Teslimat Tarihi", Path = "DeliveryDate", DataType = "date", ExampleValue = "2024-02-15" },
                    new FieldDefinition { Label = "Teslimat Adresi", Path = "ShippingAddressText", DataType = "string", ExampleValue = "İstanbul, Türkiye" },
                    
                    // Diğer Bilgiler
                    new FieldDefinition { Label = "Satış Temsilcisi", Path = "RepresentativeName", DataType = "string", ExampleValue = "Ahmet Yılmaz" },
                    new FieldDefinition { Label = "Ödeme Tipi", Path = "PaymentTypeName", DataType = "string", ExampleValue = "Vadeli" },
                    new FieldDefinition { Label = "Para Birimi", Path = "Currency", DataType = "string", ExampleValue = "TRY" },
                    new FieldDefinition { Label = "Açıklama", Path = "Description", DataType = "string", ExampleValue = "Acil sipariş" },
                    new FieldDefinition { Label = "Belge Seri Tipi", Path = "DocumentSerialTypeName", DataType = "string", ExampleValue = "A Serisi" },
                    
                    // Oluşturma Bilgileri
                    new FieldDefinition { Label = "Oluşturan", Path = "CreatedBy", DataType = "string", ExampleValue = "Mehmet Demir" },
                    new FieldDefinition { Label = "Güncelleyen", Path = "UpdatedBy", DataType = "string", ExampleValue = "Ayşe Kaya" },
                },
                LineFields = new List<FieldDefinition>
                {
                    // Ürün Bilgileri
                    new FieldDefinition { Label = "Ürün Kodu", Path = "Lines.ProductCode", DataType = "string", ExampleValue = "PRD-001" },
                    new FieldDefinition { Label = "Ürün Adı", Path = "Lines.ProductName", DataType = "string", ExampleValue = "Laptop" },
                    new FieldDefinition { Label = "Grup Kodu", Path = "Lines.GroupCode", DataType = "string", ExampleValue = "GRP-001" },
                    
                    // Miktar ve Fiyat
                    new FieldDefinition { Label = "Miktar", Path = "Lines.Quantity", DataType = "number", ExampleValue = "10" },
                    new FieldDefinition { Label = "Birim Fiyat", Path = "Lines.UnitPrice", DataType = "number", ExampleValue = "5000.00" },
                    
                    // İndirimler
                    new FieldDefinition { Label = "İndirim Oranı 1 (%)", Path = "Lines.DiscountRate1", DataType = "number", ExampleValue = "10" },
                    new FieldDefinition { Label = "İndirim Tutarı 1", Path = "Lines.DiscountAmount1", DataType = "number", ExampleValue = "500.00" },
                    new FieldDefinition { Label = "İndirim Oranı 2 (%)", Path = "Lines.DiscountRate2", DataType = "number", ExampleValue = "5" },
                    new FieldDefinition { Label = "İndirim Tutarı 2", Path = "Lines.DiscountAmount2", DataType = "number", ExampleValue = "250.00" },
                    new FieldDefinition { Label = "İndirim Oranı 3 (%)", Path = "Lines.DiscountRate3", DataType = "number", ExampleValue = "2" },
                    new FieldDefinition { Label = "İndirim Tutarı 3", Path = "Lines.DiscountAmount3", DataType = "number", ExampleValue = "100.00" },
                    
                    // KDV
                    new FieldDefinition { Label = "KDV Oranı (%)", Path = "Lines.VatRate", DataType = "number", ExampleValue = "18" },
                    new FieldDefinition { Label = "KDV Tutarı", Path = "Lines.VatAmount", DataType = "number", ExampleValue = "900.00" },
                    
                    // Toplamlar
                    new FieldDefinition { Label = "Satır Toplamı (KDV Hariç)", Path = "Lines.LineTotal", DataType = "number", ExampleValue = "45000.00" },
                    new FieldDefinition { Label = "Satır Genel Toplamı (KDV Dahil)", Path = "Lines.LineGrandTotal", DataType = "number", ExampleValue = "53100.00" },
                    
                    // Diğer
                    new FieldDefinition { Label = "Açıklama", Path = "Lines.Description", DataType = "string", ExampleValue = "Özel not" },
                }
            };
        }
    }
}
