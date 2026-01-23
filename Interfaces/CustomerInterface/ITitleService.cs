using depoWebAPI.Models;
using crm_api.DTOs;
using crm_api.Data;

namespace crm_api.Interfaces
{
    public interface ITitleService
    {
        Task<ApiResponse<PagedResponse<TitleDto>>> GetAllTitlesAsync(PagedRequest request);
        Task<ApiResponse<TitleDto>> GetTitleByIdAsync(long id);
        Task<ApiResponse<TitleDto>> CreateTitleAsync(CreateTitleDto createTitleDto);
        Task<ApiResponse<TitleDto>> UpdateTitleAsync(long id, UpdateTitleDto updateTitleDto);
        Task<ApiResponse<object>> DeleteTitleAsync(long id);
    }
}
