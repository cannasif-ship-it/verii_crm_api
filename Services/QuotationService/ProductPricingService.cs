using AutoMapper;
using crm_api.DTOs;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.UnitOfWork;
using crm_api.Helpers;
using crm_api.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace crm_api.Services
{
    public class ProductPricingService : IProductPricingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly CmsDbContext _context;

        public ProductPricingService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, CmsDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _context = context;
        }

        public async Task<ApiResponse<PagedResponse<ProductPricingGetDto>>> GetAllProductPricingsAsync(PagedRequest request)
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

                var query = _context.ProductPricings
                    .AsNoTracking()
                    .Where(pp => !pp.IsDeleted)
                    .Include(pp => pp.CreatedByUser)
                    .Include(pp => pp.UpdatedByUser)
                    .Include(pp => pp.DeletedByUser)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(ProductPricing.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<ProductPricingGetDto>(x)).ToList();

                var pagedResponse = new PagedResponse<ProductPricingGetDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<ProductPricingGetDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("ProductPricingService.ProductPricingsRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<ProductPricingGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductPricingService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductPricingService.GetAllProductPricingsExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ProductPricingGetDto>> GetProductPricingByIdAsync(long id)
        {
            try
            {
                var productPricing = await _unitOfWork.ProductPricings.GetByIdAsync(id);
                if (productPricing == null)
                {
                    return ApiResponse<ProductPricingGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductPricingService.ProductPricingNotFound"),
                        _localizationService.GetLocalizedString("ProductPricingService.ProductPricingNotFound"),
                        StatusCodes.Status404NotFound);
                }

                // Reload with navigation properties for mapping
                var productPricingWithNav = await _context.ProductPricings
                    .AsNoTracking()
                    .Include(pp => pp.CreatedByUser)
                    .Include(pp => pp.UpdatedByUser)
                    .Include(pp => pp.DeletedByUser)
                    .FirstOrDefaultAsync(pp => pp.Id == id && !pp.IsDeleted);

                var productPricingDto = _mapper.Map<ProductPricingGetDto>(productPricingWithNav ?? productPricing);
                return ApiResponse<ProductPricingGetDto>.SuccessResult(productPricingDto, _localizationService.GetLocalizedString("ProductPricingService.ProductPricingRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductPricingGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductPricingService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductPricingService.GetProductPricingByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ProductPricingGetDto>> CreateProductPricingAsync(ProductPricingCreateDto createProductPricingDto)
        {
            try
            {
                var productPricing = _mapper.Map<ProductPricing>(createProductPricingDto);
                productPricing.CreatedDate = DateTime.UtcNow;

                await _unitOfWork.ProductPricings.AddAsync(productPricing);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var productPricingWithNav = await _context.ProductPricings
                    .AsNoTracking()
                    .Include(pp => pp.CreatedByUser)
                    .Include(pp => pp.UpdatedByUser)
                    .Include(pp => pp.DeletedByUser)
                    .FirstOrDefaultAsync(pp => pp.Id == productPricing.Id && !pp.IsDeleted);

                var productPricingDto = _mapper.Map<ProductPricingGetDto>(productPricingWithNav ?? productPricing);
                return ApiResponse<ProductPricingGetDto>.SuccessResult(productPricingDto, _localizationService.GetLocalizedString("ProductPricingService.ProductPricingCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductPricingGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductPricingService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductPricingService.CreateProductPricingExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ProductPricingGetDto>> UpdateProductPricingAsync(long id, ProductPricingUpdateDto updateProductPricingDto)
        {
            try
            {
                var existingProductPricing = await _unitOfWork.ProductPricings.GetByIdAsync(id);
                if (existingProductPricing == null)
                {
                    return ApiResponse<ProductPricingGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductPricingService.ProductPricingNotFound"),
                        _localizationService.GetLocalizedString("ProductPricingService.ProductPricingNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(updateProductPricingDto, existingProductPricing);
                existingProductPricing.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.ProductPricings.UpdateAsync(existingProductPricing);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var productPricingWithNav = await _context.ProductPricings
                    .AsNoTracking()
                    .Include(pp => pp.CreatedByUser)
                    .Include(pp => pp.UpdatedByUser)
                    .Include(pp => pp.DeletedByUser)
                    .FirstOrDefaultAsync(pp => pp.Id == existingProductPricing.Id && !pp.IsDeleted);

                var productPricingDto = _mapper.Map<ProductPricingGetDto>(productPricingWithNav ?? existingProductPricing);
                return ApiResponse<ProductPricingGetDto>.SuccessResult(productPricingDto, _localizationService.GetLocalizedString("ProductPricingService.ProductPricingUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductPricingGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductPricingService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductPricingService.UpdateProductPricingExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteProductPricingAsync(long id)
        {
            try
            {
                var productPricing = await _unitOfWork.ProductPricings.GetByIdAsync(id);
                if (productPricing == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ProductPricingService.ProductPricingNotFound"),
                        _localizationService.GetLocalizedString("ProductPricingService.ProductPricingNotFound"),
                        StatusCodes.Status404NotFound);
                }

                productPricing.IsDeleted = true;
                productPricing.DeletedDate = DateTime.UtcNow;

                await _unitOfWork.ProductPricings.UpdateAsync(productPricing);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("ProductPricingService.ProductPricingDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("ProductPricingService.InternalServerError"),
                    _localizationService.GetLocalizedString("ProductPricingService.DeleteProductPricingExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
