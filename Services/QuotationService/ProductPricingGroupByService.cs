using AutoMapper;
using crm_api.DTOs;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.UnitOfWork;
using crm_api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using crm_api.Helpers;
using System;
using System.Collections.Generic;

namespace crm_api.Services
{
    public class ProductPricingGroupByService : IProductPricingGroupByService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly CmsDbContext _context;

        public ProductPricingGroupByService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, CmsDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _context = context;
        }

        public async Task<ApiResponse<PagedResponse<ProductPricingGroupByDto>>> GetAllProductPricingGroupBysAsync(PagedRequest request)
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

                var query = _context.ProductPricingGroupBys
                    .AsNoTracking()
                    .Where(ppgb => !ppgb.IsDeleted)
                    .Include(ppgb => ppgb.CreatedByUser)
                    .Include(ppgb => ppgb.UpdatedByUser)
                    .Include(ppgb => ppgb.DeletedByUser)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(ProductPricingGroupBy.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<ProductPricingGroupByDto>(x)).ToList();

                var pagedResponse = new PagedResponse<ProductPricingGroupByDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<ProductPricingGroupByDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("ProductPricingGroupByService.ProductPricingGroupBysRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<ProductPricingGroupByDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductPricingGroupByService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductPricingGroupByService.GetAllProductPricingGroupBysExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ProductPricingGroupByDto>> GetProductPricingGroupByByIdAsync(int id)
        {
            try
            {
                var productPricingGroupBy = await _unitOfWork.ProductPricingGroupBys.GetByIdAsync(id);
                if (productPricingGroupBy == null)
                {
                    return ApiResponse<ProductPricingGroupByDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductPricingGroupByService.ProductPricingGroupByNotFound"),
                        _localizationService.GetLocalizedString("ProductPricingGroupByService.ProductPricingGroupByNotFound"),
                        StatusCodes.Status404NotFound);
                }

                // Reload with navigation properties for mapping
                var productPricingGroupByWithNav = await _context.ProductPricingGroupBys
                    .AsNoTracking()
                    .Include(ppgb => ppgb.CreatedByUser)
                    .Include(ppgb => ppgb.UpdatedByUser)
                    .Include(ppgb => ppgb.DeletedByUser)
                    .FirstOrDefaultAsync(ppgb => ppgb.Id == id && !ppgb.IsDeleted);

                var productPricingGroupByDto = _mapper.Map<ProductPricingGroupByDto>(productPricingGroupByWithNav ?? productPricingGroupBy);
                return ApiResponse<ProductPricingGroupByDto>.SuccessResult(productPricingGroupByDto, _localizationService.GetLocalizedString("ProductPricingGroupByService.ProductPricingGroupByRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductPricingGroupByDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductPricingGroupByService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductPricingGroupByService.GetProductPricingGroupByByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ProductPricingGroupByDto>> CreateProductPricingGroupByAsync(CreateProductPricingGroupByDto createDto)
        {
            try
            {
                var productPricingGroupBy = _mapper.Map<ProductPricingGroupBy>(createDto);
                productPricingGroupBy.CreatedDate = DateTime.UtcNow;
                productPricingGroupBy.IsDeleted = false;

                await _unitOfWork.ProductPricingGroupBys.AddAsync(productPricingGroupBy);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var productPricingGroupByWithNav = await _context.ProductPricingGroupBys
                    .AsNoTracking()
                    .Include(ppgb => ppgb.CreatedByUser)
                    .Include(ppgb => ppgb.UpdatedByUser)
                    .Include(ppgb => ppgb.DeletedByUser)
                    .FirstOrDefaultAsync(ppgb => ppgb.Id == productPricingGroupBy.Id && !ppgb.IsDeleted);

                var productPricingGroupByDto = _mapper.Map<ProductPricingGroupByDto>(productPricingGroupByWithNav ?? productPricingGroupBy);
                return ApiResponse<ProductPricingGroupByDto>.SuccessResult(productPricingGroupByDto, _localizationService.GetLocalizedString("ProductPricingGroupByService.ProductPricingGroupByCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductPricingGroupByDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductPricingGroupByService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductPricingGroupByService.CreateProductPricingGroupByExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ProductPricingGroupByDto>> UpdateProductPricingGroupByAsync(int id, UpdateProductPricingGroupByDto updateDto)
        {
            try
            {
                var existingProductPricingGroupBy = await _unitOfWork.ProductPricingGroupBys.GetByIdAsync(id);
                if (existingProductPricingGroupBy == null)
                {
                    return ApiResponse<ProductPricingGroupByDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductPricingGroupByService.ProductPricingGroupByNotFound"),
                        _localizationService.GetLocalizedString("ProductPricingGroupByService.ProductPricingGroupByNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(updateDto, existingProductPricingGroupBy);
                existingProductPricingGroupBy.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.ProductPricingGroupBys.UpdateAsync(existingProductPricingGroupBy);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var productPricingGroupByWithNav = await _context.ProductPricingGroupBys
                    .AsNoTracking()
                    .Include(ppgb => ppgb.CreatedByUser)
                    .Include(ppgb => ppgb.UpdatedByUser)
                    .Include(ppgb => ppgb.DeletedByUser)
                    .FirstOrDefaultAsync(ppgb => ppgb.Id == existingProductPricingGroupBy.Id && !ppgb.IsDeleted);

                var productPricingGroupByDto = _mapper.Map<ProductPricingGroupByDto>(productPricingGroupByWithNav ?? existingProductPricingGroupBy);
                return ApiResponse<ProductPricingGroupByDto>.SuccessResult(productPricingGroupByDto, _localizationService.GetLocalizedString("ProductPricingGroupByService.ProductPricingGroupByUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductPricingGroupByDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductPricingGroupByService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductPricingGroupByService.UpdateProductPricingGroupByExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteProductPricingGroupByAsync(int id)
        {
            try
            {
                var productPricingGroupBy = await _unitOfWork.ProductPricingGroupBys.GetByIdAsync(id);
                if (productPricingGroupBy == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductPricingGroupByService.ProductPricingGroupByNotFound"),
                        _localizationService.GetLocalizedString("ProductPricingGroupByService.ProductPricingGroupByNotFound"),
                        StatusCodes.Status404NotFound);
                }

                productPricingGroupBy.IsDeleted = true;
                productPricingGroupBy.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.ProductPricingGroupBys.UpdateAsync(productPricingGroupBy);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("ProductPricingGroupByService.ProductPricingGroupByDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductPricingGroupByService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductPricingGroupByService.DeleteProductPricingGroupByExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
