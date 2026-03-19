using System.Threading.Tasks;
using crm_api.DTOs;
using crm_api.Models;

namespace crm_api.Interfaces
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
