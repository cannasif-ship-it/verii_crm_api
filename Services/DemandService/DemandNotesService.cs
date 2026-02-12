using AutoMapper;
using crm_api.DTOs;
using crm_api.Helpers;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace crm_api.Services
{
    public class DemandNotesService : IDemandNotesService
    {
        private const int MaxNoteCount = 15;
        private const int MaxNoteLength = 100;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public DemandNotesService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<DemandNotesGetDto>>> GetAllDemandNotesAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                request.Filters ??= new List<Filter>();

                var query = _unitOfWork.DemandNotes.Query()
                    .AsNoTracking()
                    .Where(x => !x.IsDeleted)
                    .ApplyFilters(request.Filters, request.FilterLogic)
                    .ApplySorting(request.SortBy ?? nameof(DemandNotes.Id), request.SortDirection);

                var totalCount = await query.CountAsync();
                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .Select(x => _mapper.Map<DemandNotesGetDto>(x))
                    .ToListAsync();

                var pagedResponse = new PagedResponse<DemandNotesGetDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<DemandNotesGetDto>>.SuccessResult(pagedResponse, "Demand notes retrieved.");
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<DemandNotesGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("DemandService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<DemandNotesGetDto>> GetDemandNotesByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.DemandNotes.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<DemandNotesGetDto>.ErrorResult("Demand notes not found.", "Demand notes not found.", StatusCodes.Status404NotFound);
                }

                var dto = _mapper.Map<DemandNotesGetDto>(entity);
                return ApiResponse<DemandNotesGetDto>.SuccessResult(dto, "Demand notes retrieved.");
            }
            catch (Exception ex)
            {
                return ApiResponse<DemandNotesGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("DemandService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<DemandNotesGetDto>> GetNotesByDemandIdAsync(long demandId)
        {
            try
            {
                var demandExists = await _unitOfWork.Demands.Query().AnyAsync(x => x.Id == demandId && !x.IsDeleted);
                if (!demandExists)
                {
                    return ApiResponse<DemandNotesGetDto>.ErrorResult("Demand not found.", "Demand not found.", StatusCodes.Status404NotFound);
                }

                var entity = await _unitOfWork.DemandNotes.Query().AsNoTracking().FirstOrDefaultAsync(x => x.DemandId == demandId && !x.IsDeleted);
                if (entity == null)
                {
                    return ApiResponse<DemandNotesGetDto>.SuccessResult(new DemandNotesGetDto { DemandId = demandId }, "Demand notes retrieved.");
                }

                return ApiResponse<DemandNotesGetDto>.SuccessResult(_mapper.Map<DemandNotesGetDto>(entity), "Demand notes retrieved.");
            }
            catch (Exception ex)
            {
                return ApiResponse<DemandNotesGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("DemandService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<DemandNotesDto>> CreateDemandNotesAsync(CreateDemandNotesDto createDemandNotesDto)
        {
            try
            {
                var demandExists = await _unitOfWork.Demands.Query().AnyAsync(x => x.Id == createDemandNotesDto.DemandId && !x.IsDeleted);
                if (!demandExists)
                {
                    return ApiResponse<DemandNotesDto>.ErrorResult("Demand not found.", "Demand not found.", StatusCodes.Status404NotFound);
                }

                var exists = await _unitOfWork.DemandNotes.Query().AnyAsync(x => x.DemandId == createDemandNotesDto.DemandId && !x.IsDeleted);
                if (exists)
                {
                    return ApiResponse<DemandNotesDto>.ErrorResult("Demand notes already exist for this demand.", "Demand notes already exist for this demand.", StatusCodes.Status400BadRequest);
                }

                var entity = _mapper.Map<DemandNotes>(createDemandNotesDto);
                entity.CreatedDate = DateTime.UtcNow;
                await _unitOfWork.DemandNotes.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<DemandNotesDto>.SuccessResult(_mapper.Map<DemandNotesDto>(entity), "Demand notes created.");
            }
            catch (Exception ex)
            {
                return ApiResponse<DemandNotesDto>.ErrorResult(
                    _localizationService.GetLocalizedString("DemandService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<DemandNotesDto>> UpdateDemandNotesAsync(long id, UpdateDemandNotesDto updateDemandNotesDto)
        {
            try
            {
                var existing = await _unitOfWork.DemandNotes.GetByIdAsync(id);
                if (existing == null || existing.IsDeleted)
                {
                    return ApiResponse<DemandNotesDto>.ErrorResult("Demand notes not found.", "Demand notes not found.", StatusCodes.Status404NotFound);
                }

                _mapper.Map(updateDemandNotesDto, existing);
                existing.UpdatedDate = DateTime.UtcNow;
                await _unitOfWork.DemandNotes.UpdateAsync(existing);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<DemandNotesDto>.SuccessResult(_mapper.Map<DemandNotesDto>(existing), "Demand notes updated.");
            }
            catch (Exception ex)
            {
                return ApiResponse<DemandNotesDto>.ErrorResult(
                    _localizationService.GetLocalizedString("DemandService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<DemandNotesGetDto>> UpdateNotesListByDemandIdAsync(long demandId, UpdateDemandNotesListDto request)
        {
            try
            {
                var demandExists = await _unitOfWork.Demands.Query().AnyAsync(x => x.Id == demandId && !x.IsDeleted);
                if (!demandExists)
                {
                    return ApiResponse<DemandNotesGetDto>.ErrorResult("Demand not found.", "Demand not found.", StatusCodes.Status404NotFound);
                }

                var notes = (request?.Notes ?? new List<string>())
                    .Where(x => x != null)
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToList();

                if (notes.Count > MaxNoteCount)
                {
                    return ApiResponse<DemandNotesGetDto>.ErrorResult($"A maximum of {MaxNoteCount} notes is allowed.", $"A maximum of {MaxNoteCount} notes is allowed.", StatusCodes.Status400BadRequest);
                }

                if (notes.Any(x => x.Length > MaxNoteLength))
                {
                    return ApiResponse<DemandNotesGetDto>.ErrorResult($"Each note can be at most {MaxNoteLength} characters.", $"Each note can be at most {MaxNoteLength} characters.", StatusCodes.Status400BadRequest);
                }

                var entity = await _unitOfWork.DemandNotes.Query().FirstOrDefaultAsync(x => x.DemandId == demandId && !x.IsDeleted);
                if (entity == null)
                {
                    entity = new DemandNotes { DemandId = demandId, CreatedDate = DateTime.UtcNow };
                    await _unitOfWork.DemandNotes.AddAsync(entity);
                }
                else
                {
                    entity.UpdatedDate = DateTime.UtcNow;
                    await _unitOfWork.DemandNotes.UpdateAsync(entity);
                }

                entity.Note1 = notes.ElementAtOrDefault(0);
                entity.Note2 = notes.ElementAtOrDefault(1);
                entity.Note3 = notes.ElementAtOrDefault(2);
                entity.Note4 = notes.ElementAtOrDefault(3);
                entity.Note5 = notes.ElementAtOrDefault(4);
                entity.Note6 = notes.ElementAtOrDefault(5);
                entity.Note7 = notes.ElementAtOrDefault(6);
                entity.Note8 = notes.ElementAtOrDefault(7);
                entity.Note9 = notes.ElementAtOrDefault(8);
                entity.Note10 = notes.ElementAtOrDefault(9);
                entity.Note11 = notes.ElementAtOrDefault(10);
                entity.Note12 = notes.ElementAtOrDefault(11);
                entity.Note13 = notes.ElementAtOrDefault(12);
                entity.Note14 = notes.ElementAtOrDefault(13);
                entity.Note15 = notes.ElementAtOrDefault(14);

                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<DemandNotesGetDto>.SuccessResult(_mapper.Map<DemandNotesGetDto>(entity), "Demand notes updated.");
            }
            catch (Exception ex)
            {
                return ApiResponse<DemandNotesGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("DemandService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteDemandNotesAsync(long id)
        {
            try
            {
                var existing = await _unitOfWork.DemandNotes.GetByIdAsync(id);
                if (existing == null || existing.IsDeleted)
                {
                    return ApiResponse<object>.ErrorResult("Demand notes not found.", "Demand notes not found.", StatusCodes.Status404NotFound);
                }

                await _unitOfWork.DemandNotes.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<object>.SuccessResult(null, "Demand notes deleted.");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("DemandService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
