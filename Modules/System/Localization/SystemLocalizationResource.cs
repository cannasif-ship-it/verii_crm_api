using crm_api.Shared.Common.Application.Common;

namespace crm_api.Modules.System.Localization;

public sealed class SystemLocalizationResource : ILocalizationResource
{
    public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> MessagesByCulture { get; } =
        new Dictionary<string, IReadOnlyDictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["en"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["CustomerSyncJob.Completed"] = "Customer sync job completed.",
                ["CustomerSyncJob.Failed"] = "Customer sync job failed.",
                ["CustomerSyncJob.Started"] = "Customer sync job started.",
                ["DashboardController.CurrencyRatesNotImplemented"] = "Currency rates endpoint is not implemented.",
                ["DashboardController.DashboardRetrieved"] = "Dashboard data retrieved successfully.",
                ["StockSyncJob.Completed"] = "ERP stock synchronization completed.",
                ["StockSyncJob.Failed"] = "ERP stock synchronization failed.",
                ["StockSyncJob.Started"] = "ERP stock synchronization started.",
            },
            ["tr"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["CustomerSyncJob.Completed"] = "Cari senkronizasyonu tamamland\u0131.",
                ["CustomerSyncJob.ErpFetchFailed"] = "ERP m\u00fc\u015fteri verisi al\u0131namad\u0131.",
                ["CustomerSyncJob.Failed"] = "Cari senkronizasyonu ba\u015far\u0131s\u0131z oldu.",
                ["CustomerSyncJob.Started"] = "Cari senkronizasyonu ba\u015flad\u0131.",
                ["DashboardController.CurrencyRatesNotImplemented"] = "Kur bilgileri ucu hen\u00fcz uygulanmad\u0131.",
                ["DashboardController.DashboardRetrieved"] = "Dashboard verileri ba\u015far\u0131yla getirildi.",
                ["MailJob.EmailSendFailed"] = "E-posta g\u00f6nderilemedi: {0}",
                ["StockSyncJob.Completed"] = "ERP stok senkronizasyonu tamamland\u0131.",
                ["StockSyncJob.ErpFetchFailed"] = "ERP stok verisi al\u0131namad\u0131.",
                ["StockSyncJob.Failed"] = "ERP stok senkronizasyonu ba\u015far\u0131s\u0131z oldu.",
                ["StockSyncJob.Started"] = "ERP stok senkronizasyonu ba\u015flat\u0131ld\u0131.",
            },
            ["de"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["DashboardController.CurrencyRatesNotImplemented"] = "Kur bilgileri ucu hen\u00fcz uygulanmad\u0131.",
                ["DashboardController.DashboardRetrieved"] = "Dashboard verileri ba\u015far\u0131yla getirildi.",
                ["StockSyncJob.Completed"] = "ERP stock synchronization completed.",
                ["StockSyncJob.Failed"] = "ERP stock synchronization failed.",
                ["StockSyncJob.Started"] = "ERP stock synchronization started.",
            },
            ["fr"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["DashboardController.CurrencyRatesNotImplemented"] = "Kur bilgileri ucu hen\u00fcz uygulanmad\u0131.",
                ["DashboardController.DashboardRetrieved"] = "Dashboard verileri ba\u015far\u0131yla getirildi.",
                ["StockSyncJob.Completed"] = "ERP stock synchronization completed.",
                ["StockSyncJob.Failed"] = "ERP stock synchronization failed.",
                ["StockSyncJob.Started"] = "ERP stock synchronization started.",
            },
            ["es"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["DashboardController.CurrencyRatesNotImplemented"] = "Kur bilgileri ucu hen\u00fcz uygulanmad\u0131.",
                ["DashboardController.DashboardRetrieved"] = "Dashboard verileri ba\u015far\u0131yla getirildi.",
                ["StockSyncJob.Completed"] = "ERP stock synchronization completed.",
                ["StockSyncJob.Failed"] = "ERP stock synchronization failed.",
                ["StockSyncJob.Started"] = "ERP stock synchronization started.",
            },
            ["it"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["DashboardController.CurrencyRatesNotImplemented"] = "Kur bilgileri ucu hen\u00fcz uygulanmad\u0131.",
                ["DashboardController.DashboardRetrieved"] = "Dashboard verileri ba\u015far\u0131yla getirildi.",
                ["StockSyncJob.Completed"] = "ERP stock synchronization completed.",
                ["StockSyncJob.Failed"] = "ERP stock synchronization failed.",
                ["StockSyncJob.Started"] = "ERP stock synchronization started.",
            },
            ["ar"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["DashboardController.CurrencyRatesNotImplemented"] = "Kur bilgileri ucu hen\u00fcz uygulanmad\u0131.",
                ["DashboardController.DashboardRetrieved"] = "Dashboard verileri ba\u015far\u0131yla getirildi.",
                ["StockSyncJob.Completed"] = "ERP stock synchronization completed.",
                ["StockSyncJob.Failed"] = "ERP stock synchronization failed.",
                ["StockSyncJob.Started"] = "ERP stock synchronization started.",
            },
        };
}
