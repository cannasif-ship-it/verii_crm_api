using AutoMapper;
using cms_webapi.DTOs;
using cms_webapi.Interfaces;
using cms_webapi.Models;
using cms_webapi.UnitOfWork;
using cms_webapi.Data;
using Microsoft.AspNetCore.Http;
using cms_webapi.Helpers;
using Microsoft.EntityFrameworkCore;
using System;

namespace cms_webapi.Services
{
    public class QuotationLineService : IQuotationLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly CmsDbContext _context;

        public QuotationLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, CmsDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _context = context;
        }

        public async Task<ApiResponse<PagedResponse<QuotationLineGetDto>>> GetAllQuotationLinesAsync(PagedRequest request)
        {
            try
            {
                var query = _context.QuotationLines
                    .AsNoTracking()
                    .Where(ql => !ql.IsDeleted)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(QuotationLine.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .Select(x => _mapper.Map<QuotationLineGetDto>(x))
                    .ToListAsync();

                var pagedResponse = new PagedResponse<QuotationLineGetDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<QuotationLineGetDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("QuotationLineService.QuotationLinesRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<QuotationLineGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationLineService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationLineService.GetAllExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<QuotationLineGetDto>> GetQuotationLineByIdAsync(long id)
        {
            try
            {
                var line = await _unitOfWork.QuotationLines.GetByIdAsync(id);
                if (line == null)
                {
                    return ApiResponse<QuotationLineGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("QuotationLineService.QuotationLineNotFound"),
                        _localizationService.GetLocalizedString("QuotationLineService.QuotationLineNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var dto = _mapper.Map<QuotationLineGetDto>(line);
                return ApiResponse<QuotationLineGetDto>.SuccessResult(dto, _localizationService.GetLocalizedString("QuotationLineService.QuotationLineRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<QuotationLineGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationLineService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationLineService.GetByIdExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<QuotationLineDto>> CreateQuotationLineAsync(CreateQuotationLineDto createQuotationLineDto)
        {
            try
            {
                var entity = _mapper.Map<QuotationLine>(createQuotationLineDto);
                entity.CreatedDate = DateTime.UtcNow;

                await _unitOfWork.QuotationLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<QuotationLineDto>(entity);
                return ApiResponse<QuotationLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("QuotationLineService.QuotationLineCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<QuotationLineDto>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationLineService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationLineService.CreateExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<QuotationLineDto>> UpdateQuotationLineAsync(long id, UpdateQuotationLineDto updateQuotationLineDto)
        {
            try
            {
                var existing = await _unitOfWork.QuotationLines.GetByIdAsync(id);
                if (existing == null)
                {
                    return ApiResponse<QuotationLineDto>.ErrorResult(
                        _localizationService.GetLocalizedString("QuotationLineService.QuotationLineNotFound"),
                        _localizationService.GetLocalizedString("QuotationLineService.QuotationLineNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(updateQuotationLineDto, existing);
                existing.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.QuotationLines.UpdateAsync(existing);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<QuotationLineDto>(existing);
                return ApiResponse<QuotationLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("QuotationLineService.QuotationLineUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<QuotationLineDto>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationLineService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationLineService.UpdateExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<object>> DeleteQuotationLineAsync(long id)
        {
            try
            {
                var existing = await _unitOfWork.QuotationLines.GetByIdAsync(id);
                if (existing == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("QuotationLineService.QuotationLineNotFound"),
                        _localizationService.GetLocalizedString("QuotationLineService.QuotationLineNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.QuotationLines.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("QuotationLineService.QuotationLineDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationLineService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationLineService.DeleteExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<List<QuotationLineGetDto>>> GetQuotationLinesByQuotationIdAsync(long quotationId)
        {
            try
            {
                var lines = await _unitOfWork.QuotationLines.FindAsync(q => q.QuotationId == quotationId);
                var dtos = _mapper.Map<List<QuotationLineGetDto>>(lines.ToList());
                return ApiResponse<List<QuotationLineGetDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("QuotationLineService.QuotationLinesByQuotationRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<QuotationLineGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("QuotationLineService.InternalServerError"),
                    _localizationService.GetLocalizedString("QuotationLineService.GetByQuotationIdExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }
    }
}