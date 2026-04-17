using crm_api.Shared.Common.Application.Common;

namespace crm_api.Modules.System.Localization;

public sealed class SystemSettingsLocalizationResource : ILocalizationResource
{
    public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> MessagesByCulture { get; } =
        new Dictionary<string, IReadOnlyDictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["tr"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["SystemSettingsService.SystemSettingsRetrieved"] = "Sistem ayarları başarıyla getirildi.",
                ["SystemSettingsService.DefaultSystemSettingsReturned"] = "Sistem ayarları bulunamadı, varsayılan değerler getirildi.",
                ["SystemSettingsService.SystemSettingsCreated"] = "Sistem ayarları başarıyla oluşturuldu.",
                ["SystemSettingsService.SystemSettingsUpdated"] = "Sistem ayarları başarıyla güncellendi.",
                ["SystemSettingsService.InternalServerError"] = "Sunucu tarafında bir hata oluştu.",
                ["SystemSettingsService.GetExceptionMessage"] = "Sistem ayarları alınırken bir hata oluştu: {0}",
                ["SystemSettingsService.UpdateExceptionMessage"] = "Sistem ayarları güncellenirken bir hata oluştu: {0}",
            },
            ["en"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["SystemSettingsService.SystemSettingsRetrieved"] = "System settings retrieved successfully.",
                ["SystemSettingsService.DefaultSystemSettingsReturned"] = "System settings were not found, default values were returned.",
                ["SystemSettingsService.SystemSettingsCreated"] = "System settings created successfully.",
                ["SystemSettingsService.SystemSettingsUpdated"] = "System settings updated successfully.",
                ["SystemSettingsService.InternalServerError"] = "An internal server error occurred.",
                ["SystemSettingsService.GetExceptionMessage"] = "An error occurred while retrieving system settings: {0}",
                ["SystemSettingsService.UpdateExceptionMessage"] = "An error occurred while updating system settings: {0}",
            },
        };
}
