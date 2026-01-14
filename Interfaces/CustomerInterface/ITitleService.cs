using depoWebAPI.Models;
using cms_webapi.DTOs;
using cms_webapi.Data;

namespace cms_webapi.Interfaces
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
