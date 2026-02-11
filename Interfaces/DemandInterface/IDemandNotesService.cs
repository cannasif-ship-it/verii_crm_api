using crm_api.DTOs;

namespace crm_api.Interfaces
{
    public interface IDemandNotesService
    {
        Task<ApiResponse<PagedResponse<DemandNotesGetDto>>> GetAllDemandNotesAsync(PagedRequest request);
        Task<ApiResponse<DemandNotesGetDto>> GetDemandNotesByIdAsync(long id);
        Task<ApiResponse<DemandNotesGetDto>> GetNotesByDemandIdAsync(long demandId);
        Task<ApiResponse<DemandNotesDto>> CreateDemandNotesAsync(CreateDemandNotesDto createDemandNotesDto);
        Task<ApiResponse<DemandNotesDto>> UpdateDemandNotesAsync(long id, UpdateDemandNotesDto updateDemandNotesDto);
        Task<ApiResponse<DemandNotesGetDto>> UpdateNotesListByDemandIdAsync(long demandId, UpdateDemandNotesListDto request);
        Task<ApiResponse<object>> DeleteDemandNotesAsync(long id);
    }
}
