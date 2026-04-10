using System.Threading.Tasks;

namespace crm_api.Modules.PdfBuilder.Application.Services
{
    public interface IPdfTablePresetService
    {
        Task<ApiResponse<PagedResponse<PdfTablePresetDto>>> GetAllAsync(PdfTablePresetListRequest request);
        Task<ApiResponse<PdfTablePresetDto>> GetByIdAsync(long id);
        Task<ApiResponse<PdfTablePresetDto>> CreateAsync(CreatePdfTablePresetDto dto, long userId);
        Task<ApiResponse<PdfTablePresetDto>> UpdateAsync(long id, UpdatePdfTablePresetDto dto, long userId);
        Task<ApiResponse<bool>> DeleteAsync(long id);
    }
}
