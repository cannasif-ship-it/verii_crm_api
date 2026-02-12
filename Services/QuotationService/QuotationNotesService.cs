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
    public class QuotationNotesService : IQuotationNotesService
    {
        private const int MaxNoteCount = 15;
        private const int MaxNoteLength = 100;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public QuotationNotesService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<QuotationNotesGetDto>>> GetAllQuotationNotesAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                request.Filters ??= new List<Filter>();

                var query = _unitOfWork.QuotationNotes.Query()
                    .AsNoTracking()
                    .Where(x => !x.IsDeleted)
                    .ApplyFilters(request.Filters, request.FilterLogic)
                    .ApplySorting(request.SortBy ?? nameof(QuotationNotes.Id), request.SortDirection);

                var totalCount = await query.CountAsync();
                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .Select(x => _mapper.Map<QuotationNotesGetDto>(x))
                    .ToListAsync();

                var pagedResponse = new PagedResponse<QuotationNotesGetDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<QuotationNotesGetDto>>.SuccessResult(pagedResponse, "Quotation notes retrieved.");
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<QuotationNotesGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<QuotationNotesGetDto>> GetQuotationNotesByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.QuotationNotes.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<QuotationNotesGetDto>.ErrorResult("Quotation notes not found.", "Quotation notes not found.", StatusCodes.Status404NotFound);
                }

                var dto = _mapper.Map<QuotationNotesGetDto>(entity);
                return ApiResponse<QuotationNotesGetDto>.SuccessResult(dto, "Quotation notes retrieved.");
            }
            catch (Exception ex)
            {
                return ApiResponse<QuotationNotesGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<QuotationNotesGetDto>> GetNotesByQuotationIdAsync(long quotationId)
        {
            try
            {
                var quotationExists = await _unitOfWork.Quotations.Query()
                    .AnyAsync(x => x.Id == quotationId && !x.IsDeleted);

                if (!quotationExists)
                {
                    return ApiResponse<QuotationNotesGetDto>.ErrorResult("Quotation not found.", "Quotation not found.", StatusCodes.Status404NotFound);
                }

                var entity = await _unitOfWork.QuotationNotes.Query()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.QuotationId == quotationId && !x.IsDeleted);

                if (entity == null)
                {
                    var empty = new QuotationNotesGetDto { QuotationId = quotationId };
                    return ApiResponse<QuotationNotesGetDto>.SuccessResult(empty, "Quotation notes retrieved.");
                }

                var dto = _mapper.Map<QuotationNotesGetDto>(entity);
                return ApiResponse<QuotationNotesGetDto>.SuccessResult(dto, "Quotation notes retrieved.");
            }
            catch (Exception ex)
            {
                return ApiResponse<QuotationNotesGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<QuotationNotesGetDto>> GetByQuotationIdAsync(long quotationId)
        {
            return await GetNotesByQuotationIdAsync(quotationId);
        }

        public async Task<ApiResponse<QuotationNotesDto>> CreateQuotationNotesAsync(CreateQuotationNotesDto createQuotationNotesDto)
        {
            try
            {
                var quotationExists = await _unitOfWork.Quotations.Query()
                    .AnyAsync(x => x.Id == createQuotationNotesDto.QuotationId && !x.IsDeleted);

                if (!quotationExists)
                {
                    return ApiResponse<QuotationNotesDto>.ErrorResult("Quotation not found.", "Quotation not found.", StatusCodes.Status404NotFound);
                }

                var alreadyExists = await _unitOfWork.QuotationNotes.Query()
                    .AnyAsync(x => x.QuotationId == createQuotationNotesDto.QuotationId && !x.IsDeleted);

                if (alreadyExists)
                {
                    return ApiResponse<QuotationNotesDto>.ErrorResult("Quotation notes already exist for this quotation.", "Quotation notes already exist for this quotation.", StatusCodes.Status400BadRequest);
                }

                var entity = _mapper.Map<QuotationNotes>(createQuotationNotesDto);
                entity.CreatedDate = DateTime.UtcNow;

                await _unitOfWork.QuotationNotes.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<QuotationNotesDto>(entity);
                return ApiResponse<QuotationNotesDto>.SuccessResult(dto, "Quotation notes created.");
            }
            catch (Exception ex)
            {
                return ApiResponse<QuotationNotesDto>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<QuotationNotesDto>> UpdateQuotationNotesAsync(long id, UpdateQuotationNotesDto updateQuotationNotesDto)
        {
            try
            {
                var existing = await _unitOfWork.QuotationNotes.GetByIdAsync(id);
                if (existing == null || existing.IsDeleted)
                {
                    return ApiResponse<QuotationNotesDto>.ErrorResult("Quotation notes not found.", "Quotation notes not found.", StatusCodes.Status404NotFound);
                }

                _mapper.Map(updateQuotationNotesDto, existing);
                existing.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.QuotationNotes.UpdateAsync(existing);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<QuotationNotesDto>(existing);
                return ApiResponse<QuotationNotesDto>.SuccessResult(dto, "Quotation notes updated.");
            }
            catch (Exception ex)
            {
                return ApiResponse<QuotationNotesDto>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<QuotationNotesGetDto>> UpdateNotesListByQuotationIdAsync(long quotationId, UpdateQuotationNotesListDto request)
        {
            try
            {
                var quotationExists = await _unitOfWork.Quotations.Query()
                    .AnyAsync(x => x.Id == quotationId && !x.IsDeleted);

                if (!quotationExists)
                {
                    return ApiResponse<QuotationNotesGetDto>.ErrorResult("Quotation not found.", "Quotation not found.", StatusCodes.Status404NotFound);
                }

                var normalizedNotes = NormalizeNotes(request?.Notes);

                if (normalizedNotes.Count > MaxNoteCount)
                {
                    return ApiResponse<QuotationNotesGetDto>.ErrorResult(
                        $"A maximum of {MaxNoteCount} notes is allowed.",
                        $"A maximum of {MaxNoteCount} notes is allowed.",
                        StatusCodes.Status400BadRequest);
                }

                var tooLong = normalizedNotes.FirstOrDefault(n => n.Length > MaxNoteLength);
                if (tooLong != null)
                {
                    return ApiResponse<QuotationNotesGetDto>.ErrorResult(
                        $"Each note can be at most {MaxNoteLength} characters.",
                        $"Each note can be at most {MaxNoteLength} characters.",
                        StatusCodes.Status400BadRequest);
                }

                var entity = await _unitOfWork.QuotationNotes.Query()
                    .FirstOrDefaultAsync(x => x.QuotationId == quotationId && !x.IsDeleted);

                if (entity == null)
                {
                    entity = new QuotationNotes
                    {
                        QuotationId = quotationId,
                        CreatedDate = DateTime.UtcNow
                    };

                    ApplyNotesToEntity(entity, normalizedNotes);
                    await _unitOfWork.QuotationNotes.AddAsync(entity);
                }
                else
                {
                    ApplyNotesToEntity(entity, normalizedNotes);
                    entity.UpdatedDate = DateTime.UtcNow;
                    await _unitOfWork.QuotationNotes.UpdateAsync(entity);
                }

                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<QuotationNotesGetDto>(entity);
                return ApiResponse<QuotationNotesGetDto>.SuccessResult(dto, "Quotation notes updated.");
            }
            catch (Exception ex)
            {
                return ApiResponse<QuotationNotesGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteQuotationNotesAsync(long id)
        {
            try
            {
                var existing = await _unitOfWork.QuotationNotes.GetByIdAsync(id);
                if (existing == null || existing.IsDeleted)
                {
                    return ApiResponse<object>.ErrorResult("Quotation notes not found.", "Quotation notes not found.", StatusCodes.Status404NotFound);
                }

                await _unitOfWork.QuotationNotes.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, "Quotation notes deleted.");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationService.InternalServerError"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        private static List<string> NormalizeNotes(List<string>? notes)
        {
            if (notes == null)
            {
                return new List<string>();
            }

            return notes
                .Where(x => x != null)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
        }

        private static void ApplyNotesToEntity(QuotationNotes entity, IReadOnlyList<string> notes)
        {
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
        }
    }
}
