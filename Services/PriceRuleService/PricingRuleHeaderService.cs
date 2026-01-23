using AutoMapper;
using crm_api.DTOs;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.UnitOfWork;
using crm_api.Helpers;
using Microsoft.EntityFrameworkCore;

namespace crm_api.Services
{
    public class PricingRuleHeaderService : IPricingRuleHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public PricingRuleHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<PricingRuleHeaderGetDto>>> GetAllPricingRuleHeadersAsync(PagedRequest request)
        {
            try
            {
                if (request == null)
                {
                    request = new PagedRequest();
                }

                if (request.Filters == null)
                {
                    request.Filters = new List<Filter>();
                }

                var query = _unitOfWork.PricingRuleHeaders
                    .Query()
                    .Where(h => !h.IsDeleted)
                    .Include(h => h.Customer)
                    .Include(h => h.Lines.Where(l => !l.IsDeleted))
                    .Include(h => h.Salesmen.Where(s => !s.IsDeleted))
                    .Include(h => h.CreatedByUser)
                    .Include(h => h.UpdatedByUser)
                    .Include(h => h.DeletedByUser)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(PricingRuleHeader.Id);
                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<PricingRuleHeaderGetDto>(x)).ToList();

                var pagedResponse = new PagedResponse<PricingRuleHeaderGetDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<PricingRuleHeaderGetDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("PricingRuleHeaderService.HeadersRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<PricingRuleHeaderGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("PricingRuleHeaderService.InternalServerError"),
                    _localizationService.GetLocalizedString("PricingRuleHeaderService.GetAllHeadersExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<PricingRuleHeaderGetDto>> GetPricingRuleHeaderByIdAsync(long id)
        {
            try
            {
                var header = await _unitOfWork.PricingRuleHeaders
                    .Query()
                    .Include(h => h.Customer)
                    .Include(h => h.Lines.Where(l => !l.IsDeleted))
                    .Include(h => h.Salesmen.Where(s => !s.IsDeleted))
                    .Include(h => h.CreatedByUser)
                    .Include(h => h.UpdatedByUser)
                    .Include(h => h.DeletedByUser)
                    .FirstOrDefaultAsync(h => h.Id == id && !h.IsDeleted);

                if (header == null)
                {
                    return ApiResponse<PricingRuleHeaderGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("PricingRuleHeaderService.HeaderNotFound"),
                        _localizationService.GetLocalizedString("PricingRuleHeaderService.HeaderNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var headerDto = _mapper.Map<PricingRuleHeaderGetDto>(header);
                return ApiResponse<PricingRuleHeaderGetDto>.SuccessResult(headerDto, _localizationService.GetLocalizedString("PricingRuleHeaderService.HeaderRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PricingRuleHeaderGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("PricingRuleHeaderService.InternalServerError"),
                    _localizationService.GetLocalizedString("PricingRuleHeaderService.GetHeaderByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<PricingRuleHeaderGetDto>> CreatePricingRuleHeaderAsync(PricingRuleHeaderCreateDto createDto)
        {
            try
            {
                var header = _mapper.Map<PricingRuleHeader>(createDto);
                await _unitOfWork.PricingRuleHeaders.AddAsync(header);
                await _unitOfWork.SaveChangesAsync();

                // Handle Lines
                if (createDto.Lines != null && createDto.Lines.Any())
                {
                    foreach (var lineDto in createDto.Lines)
                    {
                        var line = _mapper.Map<PricingRuleLine>(lineDto);
                        line.PricingRuleHeaderId = header.Id;
                        await _unitOfWork.PricingRuleLines.AddAsync(line);
                    }
                }

                // Handle Salesmen
                if (createDto.Salesmen != null && createDto.Salesmen.Any())
                {
                    foreach (var salesmanDto in createDto.Salesmen)
                    {
                        var salesman = _mapper.Map<PricingRuleSalesman>(salesmanDto);
                        salesman.PricingRuleHeaderId = header.Id;
                        await _unitOfWork.PricingRuleSalesmen.AddAsync(salesman);
                    }
                }

                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties
                var headerWithNav = await _unitOfWork.PricingRuleHeaders
                    .Query()
                    .Include(h => h.Customer)
                    .Include(h => h.Lines.Where(l => !l.IsDeleted))
                    .Include(h => h.Salesmen.Where(s => !s.IsDeleted))
                    .Include(h => h.CreatedByUser)
                    .Include(h => h.UpdatedByUser)
                    .Include(h => h.DeletedByUser)
                    .FirstOrDefaultAsync(h => h.Id == header.Id && !h.IsDeleted);

                if (headerWithNav == null)
                {
                    return ApiResponse<PricingRuleHeaderGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("PricingRuleHeaderService.HeaderNotFound"),
                        _localizationService.GetLocalizedString("PricingRuleHeaderService.HeaderNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var headerDto = _mapper.Map<PricingRuleHeaderGetDto>(headerWithNav);
                return ApiResponse<PricingRuleHeaderGetDto>.SuccessResult(headerDto, _localizationService.GetLocalizedString("PricingRuleHeaderService.HeaderCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PricingRuleHeaderGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("PricingRuleHeaderService.InternalServerError"),
                    _localizationService.GetLocalizedString("PricingRuleHeaderService.CreateHeaderExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<PricingRuleHeaderGetDto>> UpdatePricingRuleHeaderAsync(long id, PricingRuleHeaderUpdateDto updateDto)
        {
            try
            {
                var header = await _unitOfWork.PricingRuleHeaders.GetByIdForUpdateAsync(id);
                if (header == null)
                {
                    return ApiResponse<PricingRuleHeaderGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("PricingRuleHeaderService.HeaderNotFound"),
                        _localizationService.GetLocalizedString("PricingRuleHeaderService.HeaderNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(updateDto, header);
                await _unitOfWork.PricingRuleHeaders.UpdateAsync(header);

                // Handle Lines update (delete existing and add new)
                if (updateDto.Lines != null)
                {
                    var existingLines = await _unitOfWork.PricingRuleLines
                        .Query()
                        .Where(l => l.PricingRuleHeaderId == id && !l.IsDeleted)
                        .ToListAsync();

                    foreach (var existingLine in existingLines)
                    {
                        await _unitOfWork.PricingRuleLines.SoftDeleteAsync(existingLine.Id);
                    }

                    foreach (var lineDto in updateDto.Lines)
                    {
                        var line = _mapper.Map<PricingRuleLine>(lineDto);
                        line.PricingRuleHeaderId = id;
                        await _unitOfWork.PricingRuleLines.AddAsync(line);
                    }
                }

                // Handle Salesmen update (delete existing and add new)
                if (updateDto.Salesmen != null)
                {
                    var existingSalesmen = await _unitOfWork.PricingRuleSalesmen
                        .Query()
                        .Where(s => s.PricingRuleHeaderId == id && !s.IsDeleted)
                        .ToListAsync();

                    foreach (var existingSalesman in existingSalesmen)
                    {
                        await _unitOfWork.PricingRuleSalesmen.SoftDeleteAsync(existingSalesman.Id);
                    }

                    foreach (var salesmanDto in updateDto.Salesmen)
                    {
                        var salesman = _mapper.Map<PricingRuleSalesman>(salesmanDto);
                        salesman.PricingRuleHeaderId = id;
                        await _unitOfWork.PricingRuleSalesmen.AddAsync(salesman);
                    }
                }

                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties
                var headerWithNav = await _unitOfWork.PricingRuleHeaders
                    .Query()
                    .Include(h => h.Customer)
                    .Include(h => h.Lines.Where(l => !l.IsDeleted))
                    .Include(h => h.Salesmen.Where(s => !s.IsDeleted))
                    .Include(h => h.CreatedByUser)
                    .Include(h => h.UpdatedByUser)
                    .Include(h => h.DeletedByUser)
                    .FirstOrDefaultAsync(h => h.Id == header.Id && !h.IsDeleted);

                if (headerWithNav == null)
                {
                    return ApiResponse<PricingRuleHeaderGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("PricingRuleHeaderService.HeaderNotFound"),
                        _localizationService.GetLocalizedString("PricingRuleHeaderService.HeaderNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var headerDto = _mapper.Map<PricingRuleHeaderGetDto>(headerWithNav);
                return ApiResponse<PricingRuleHeaderGetDto>.SuccessResult(headerDto, _localizationService.GetLocalizedString("PricingRuleHeaderService.HeaderUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PricingRuleHeaderGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("PricingRuleHeaderService.InternalServerError"),
                    _localizationService.GetLocalizedString("PricingRuleHeaderService.UpdateHeaderExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeletePricingRuleHeaderAsync(long id)
        {
            try
            {
                var header = await _unitOfWork.PricingRuleHeaders.GetByIdAsync(id);
                if (header == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("PricingRuleHeaderService.HeaderNotFound"),
                        _localizationService.GetLocalizedString("PricingRuleHeaderService.HeaderNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.PricingRuleHeaders.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("PricingRuleHeaderService.HeaderDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("PricingRuleHeaderService.InternalServerError"),
                    _localizationService.GetLocalizedString("PricingRuleHeaderService.DeleteHeaderExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
