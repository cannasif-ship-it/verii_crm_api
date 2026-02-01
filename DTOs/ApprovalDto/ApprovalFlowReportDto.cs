namespace crm_api.DTOs
{
    /// <summary>
    /// Teklif onay akışı raporu - Teklif ID ile tüm onay sürecinin detaylı özeti
    /// </summary>
    public class QuotationApprovalFlowReportDto
    {
        /// <summary>Teklif ID</summary>
        public long QuotationId { get; set; }

        /// <summary>Teklif numarası / revizyon (Örn: OFF-2025-001)</summary>
        public string? QuotationOfferNo { get; set; }

        /// <summary>Onay süreci başlatılmış mı?</summary>
        public bool HasApprovalRequest { get; set; }

        /// <summary>Genel durum: HavenotStarted, Waiting, Approved, Rejected</summary>
        public int? OverallStatus { get; set; }

        /// <summary>Genel durum açıklaması</summary>
        public string? OverallStatusName { get; set; }

        /// <summary>Mevcut adım numarası</summary>
        public int CurrentStep { get; set; }

        /// <summary>Onay akışı açıklaması (Örn: 100K Üstü Teklif)</summary>
        public string? FlowDescription { get; set; }

        /// <summary>Red sebebi (reddedildiyse, Quotation.RejectedReason)</summary>
        public string? RejectedReason { get; set; }

        /// <summary>Aşama bazlı detaylar</summary>
        public List<ApprovalFlowStepReportDto> Steps { get; set; } = new();
    }

    /// <summary>
    /// Her onay aşamasının detaylı raporu
    /// </summary>
    public class ApprovalFlowStepReportDto
    {
        /// <summary>Adım sırası (1, 2, 3...)</summary>
        public int StepOrder { get; set; }

        /// <summary>Adım adı (Rol grubu adı - Örn: Satış Müdürü, Genel Müdür)</summary>
        public string StepName { get; set; } = null!;

        /// <summary>Bu aşamadaki durum: NotStarted, InProgress, Completed, Rejected</summary>
        public string StepStatus { get; set; } = null!;

        /// <summary>Bu aşamadaki tüm kullanıcıların işlem detayları</summary>
        public List<ApprovalActionDetailDto> Actions { get; set; } = new();
    }

    /// <summary>
    /// Her kullanıcının onay aşamasındaki işlem detayı
    /// </summary>
    public class ApprovalActionDetailDto
    {
        /// <summary>Kullanıcı ID</summary>
        public long UserId { get; set; }

        /// <summary>Kullanıcı adı soyadı</summary>
        public string? UserFullName { get; set; }

        /// <summary>Kullanıcı e-posta</summary>
        public string? UserEmail { get; set; }

        /// <summary>İşlem durumu: Waiting (Beklemede), Approved (Onayladı), Rejected (Reddetti)</summary>
        public int Status { get; set; }

        /// <summary>İşlem durumu açıklaması</summary>
        public string StatusName { get; set; } = null!;

        /// <summary>İşlem tarihi (onay/red anı, Waiting ise null)</summary>
        public DateTime? ActionDate { get; set; }

        /// <summary>Red sebebi (sadece Rejected ise dolu)</summary>
        public string? RejectedReason { get; set; }
    }

    /// <summary>
    /// Talep onay akışı raporu - Talep ID ile tüm onay sürecinin detaylı özeti
    /// </summary>
    public class DemandApprovalFlowReportDto
    {
        /// <summary>Talep ID</summary>
        public long DemandId { get; set; }

        /// <summary>Talep numarası / revizyon</summary>
        public string? DemandOfferNo { get; set; }

        /// <summary>Onay süreci başlatılmış mı?</summary>
        public bool HasApprovalRequest { get; set; }

        /// <summary>Genel durum: HavenotStarted, Waiting, Approved, Rejected</summary>
        public int? OverallStatus { get; set; }

        /// <summary>Genel durum açıklaması</summary>
        public string? OverallStatusName { get; set; }

        /// <summary>Mevcut adım numarası</summary>
        public int CurrentStep { get; set; }

        /// <summary>Onay akışı açıklaması</summary>
        public string? FlowDescription { get; set; }

        /// <summary>Red sebebi (reddedildiyse)</summary>
        public string? RejectedReason { get; set; }

        /// <summary>Aşama bazlı detaylar</summary>
        public List<ApprovalFlowStepReportDto> Steps { get; set; } = new();
    }

    /// <summary>
    /// Sipariş onay akışı raporu - Sipariş ID ile tüm onay sürecinin detaylı özeti
    /// </summary>
    public class OrderApprovalFlowReportDto
    {
        /// <summary>Sipariş ID</summary>
        public long OrderId { get; set; }

        /// <summary>Sipariş numarası / revizyon</summary>
        public string? OrderOfferNo { get; set; }

        /// <summary>Onay süreci başlatılmış mı?</summary>
        public bool HasApprovalRequest { get; set; }

        /// <summary>Genel durum: HavenotStarted, Waiting, Approved, Rejected</summary>
        public int? OverallStatus { get; set; }

        /// <summary>Genel durum açıklaması</summary>
        public string? OverallStatusName { get; set; }

        /// <summary>Mevcut adım numarası</summary>
        public int CurrentStep { get; set; }

        /// <summary>Onay akışı açıklaması</summary>
        public string? FlowDescription { get; set; }

        /// <summary>Red sebebi (reddedildiyse)</summary>
        public string? RejectedReason { get; set; }

        /// <summary>Aşama bazlı detaylar</summary>
        public List<ApprovalFlowStepReportDto> Steps { get; set; } = new();
    }
}
