using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface ISmtpSettingsService
    {
        Task<ApiResponse<SmtpSettingsDto>> GetAsync();
        Task<ApiResponse<SmtpSettingsDto>> UpdateAsync(UpdateSmtpSettingsDto dto, long userId);

        // internal kullanım (mail sender)
        Task<SmtpSettingsRuntimeDto> GetRuntimeAsync();

        // update sonrası bu instance cache temizliği
        void InvalidateCache();
    }
}
