using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IUserAuthorityService
    {
        Task<ApiResponse<PagedResponse<UserAuthorityDto>>> GetAllAsync(PagedRequest request);
        Task<ApiResponse<UserAuthorityDto>> GetByIdAsync(long id);
        Task<ApiResponse<UserAuthorityDto>> CreateAsync(CreateUserAuthorityDto createDto);
        Task<ApiResponse<UserAuthorityDto>> UpdateAsync(long id, UpdateUserAuthorityDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<bool>> ExistsAsync(long id);
    }
}
