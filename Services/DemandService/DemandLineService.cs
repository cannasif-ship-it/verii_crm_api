using AutoMapper;
using crm_api.DTOs;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.UnitOfWork;
using crm_api.Data;
using Microsoft.AspNetCore.Http;
using crm_api.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace crm_api.Services
{
    public class DemandLineService : IDemandLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly CmsDbContext _context;

        public DemandLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, CmsDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _context = context;
        }

        public async Task<ApiResponse<PagedResponse<DemandLineGetDto>>> GetAllDemandLinesAsync(PagedRequest request)
        {
            try
            {
                var query = _unitOfWork.Repository<DemandLine>().Query()
                    .AsNoTracking()
                    .Where(ql => !ql.IsDeleted)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(DemandLine.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<DemandLineGetDto>(x)).ToList();

                var pagedResponse = new PagedResponse<DemandLineGetDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<DemandLineGetDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("DemandLineService.DemandLinesRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<DemandLineGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("DemandLineService.InternalServerError"),
                    _localizationService.GetLocalizedString("DemandLineService.GetAllExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<DemandLineGetDto>> GetDemandLineByIdAsync(long id)
        {
            try
            {
                var line = await _unitOfWork.Repository<DemandLine>().GetByIdAsync(id);
                if (line == null)
                {
                    return ApiResponse<DemandLineGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("DemandLineService.DemandLineNotFound"),
                        _localizationService.GetLocalizedString("DemandLineService.DemandLineNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var dto = _mapper.Map<DemandLineGetDto>(line);
                return ApiResponse<DemandLineGetDto>.SuccessResult(dto, _localizationService.GetLocalizedString("DemandLineService.DemandLineRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<DemandLineGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("DemandLineService.InternalServerError"),
                    _localizationService.GetLocalizedString("DemandLineService.GetByIdExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<DemandLineDto>> CreateDemandLineAsync(CreateDemandLineDto createDemandLineDto)
        {
            try
            {
                var entity = _mapper.Map<DemandLine>(createDemandLineDto);
                entity.CreatedDate = DateTime.UtcNow;

                await _unitOfWork.Repository<DemandLine>().AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<DemandLineDto>(entity);
                return ApiResponse<DemandLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("DemandLineService.DemandLineCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<DemandLineDto>.ErrorResult(
                    _localizationService.GetLocalizedString("DemandLineService.InternalServerError"),
                    _localizationService.GetLocalizedString("DemandLineService.CreateExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<DemandLineDto>> UpdateDemandLineAsync(long id, UpdateDemandLineDto updateDemandLineDto)
        {
            try
            {
                var existing = await _unitOfWork.Repository<DemandLine>().GetByIdAsync(id);
                if (existing == null)
                {
                    return ApiResponse<DemandLineDto>.ErrorResult(
                        _localizationService.GetLocalizedString("DemandLineService.DemandLineNotFound"),
                        _localizationService.GetLocalizedString("DemandLineService.DemandLineNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(updateDemandLineDto, existing);
                existing.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.Repository<DemandLine>().UpdateAsync(existing);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<DemandLineDto>(existing);
                return ApiResponse<DemandLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("DemandLineService.DemandLineUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<DemandLineDto>.ErrorResult(
                    _localizationService.GetLocalizedString("DemandLineService.InternalServerError"),
                    _localizationService.GetLocalizedString("DemandLineService.UpdateExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<object>> DeleteDemandLineAsync(long id)
        {
            try
            {
                var existing = await _unitOfWork.Repository<DemandLine>().GetByIdAsync(id);
                if (existing == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("DemandLineService.DemandLineNotFound"),
                        _localizationService.GetLocalizedString("DemandLineService.DemandLineNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.Repository<DemandLine>().SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("DemandLineService.DemandLineDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("DemandLineService.InternalServerError"),
                    _localizationService.GetLocalizedString("DemandLineService.DeleteExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<ApiResponse<List<DemandLineGetDto>>> GetDemandLinesByDemandIdAsync(long demandId)
        {
            try
            {
                var dtos = await _unitOfWork.Repository<DemandLine>()
                    .Query()
                    .Where(q => q.DemandId == demandId && !q.IsDeleted)
                    .Join(
                        _unitOfWork.Stocks.Query(),
                        ql => ql.ProductCode,
                        s => s.ErpStockCode,
                        (ql, s) => new
                        {
                            DemandLine = ql,
                            ProductName = s.StockName,
                            GroupCode = s.GrupKodu
                        })
                    .Select(x => new DemandLineGetDto
                    {
                        DemandId = x.DemandLine.DemandId,
                        ProductCode = x.DemandLine.ProductCode,
                        ProductName = x.ProductName,
                        GroupCode = x.GroupCode,
                        Quantity = x.DemandLine.Quantity,
                        UnitPrice = x.DemandLine.UnitPrice,
                        DiscountRate1 = x.DemandLine.DiscountRate1,
                        DiscountAmount1 = x.DemandLine.DiscountAmount1,
                        DiscountRate2 = x.DemandLine.DiscountRate2,
                        DiscountAmount2 = x.DemandLine.DiscountAmount2,
                        DiscountRate3 = x.DemandLine.DiscountRate3,
                        DiscountAmount3 = x.DemandLine.DiscountAmount3,
                        VatRate = x.DemandLine.VatRate,
                        VatAmount = x.DemandLine.VatAmount,
                        LineTotal = x.DemandLine.LineTotal,
                        LineGrandTotal = x.DemandLine.LineGrandTotal,
                        Description = x.DemandLine.Description,
                        PricingRuleHeaderId = x.DemandLine.PricingRuleHeaderId,
                        RelatedStockId = x.DemandLine.RelatedStockId,
                        RelatedProductKey = x.DemandLine.RelatedProductKey,
                        IsMainRelatedProduct = x.DemandLine.IsMainRelatedProduct,
                        ApprovalStatus = x.DemandLine.ApprovalStatus
                    })
                    .ToListAsync();

                return ApiResponse<List<DemandLineGetDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("DemandLineService.DemandLinesByDemandRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<DemandLineGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("DemandLineService.InternalServerError"),
                    _localizationService.GetLocalizedString("DemandLineService.GetByDemandIdExceptionMessage", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }
    }
}
