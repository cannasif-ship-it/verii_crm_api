using AutoMapper;
using crm_api.UnitOfWork;
using crm_api.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace crm_api.Modules.Customer.Application.Services
{
    public class CustomerTypeService : ICustomerTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public CustomerTypeService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<CustomerTypeGetDto>>> GetAllCustomerTypesAsync(PagedRequest request)
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

                var query = _unitOfWork.CustomerTypes
                    .Query()
                    .Where(ct => !ct.IsDeleted)
                    .Include(ct => ct.CreatedByUser)
                    .Include(ct => ct.UpdatedByUser)
                    .Include(ct => ct.DeletedByUser)
                    .ApplySearch(request.Search, QueryHelper.CommonSearchableColumns)
                    .ApplyFilters(request.Filters, request.FilterLogic);

                var sortBy = request.SortBy ?? nameof(CustomerType.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync().ConfigureAwait(false);

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync().ConfigureAwait(false);

                var dtos = items.Select(x => _mapper.Map<CustomerTypeGetDto>(x)).ToList();

                var pagedResponse = new PagedResponse<CustomerTypeGetDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<CustomerTypeGetDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("CustomerTypeService.CustomerTypesRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<CustomerTypeGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("CustomerTypeService.InternalServerError"),
                    _localizationService.GetLocalizedString("CustomerTypeService.GetAllCustomerTypesExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<CustomerTypeGetDto>> GetCustomerTypeByIdAsync(long id)
        {
            try
            {
                var customerType = await _unitOfWork.CustomerTypes.GetByIdAsync(id).ConfigureAwait(false);
                if (customerType == null)
                {
                    return ApiResponse<CustomerTypeGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("CustomerTypeService.CustomerTypeNotFound"),
                        _localizationService.GetLocalizedString("CustomerTypeService.CustomerTypeNotFound"),
                        StatusCodes.Status404NotFound);
                }

                // Reload with navigation properties for mapping
                var customerTypeWithNav = await _unitOfWork.CustomerTypes
                    .Query()
                    .Include(ct => ct.CreatedByUser)
                    .Include(ct => ct.UpdatedByUser)
                    .Include(ct => ct.DeletedByUser)
                    .FirstOrDefaultAsync(ct => ct.Id == id && !ct.IsDeleted).ConfigureAwait(false);

                var customerTypeDto = _mapper.Map<CustomerTypeGetDto>(customerTypeWithNav ?? customerType);
                return ApiResponse<CustomerTypeGetDto>.SuccessResult(customerTypeDto, _localizationService.GetLocalizedString("CustomerTypeService.CustomerTypeRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<CustomerTypeGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("CustomerTypeService.InternalServerError"),
                    _localizationService.GetLocalizedString("CustomerTypeService.GetCustomerTypeByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<CustomerTypeGetDto>> CreateCustomerTypeAsync(CustomerTypeCreateDto customerTypeCreateDto)
        {
            try
            {
                var customerType = _mapper.Map<CustomerType>(customerTypeCreateDto);
                await _unitOfWork.CustomerTypes.AddAsync(customerType).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                // Reload with navigation properties for mapping
                var customerTypeWithNav = await _unitOfWork.CustomerTypes
                    .Query()
                    .Include(ct => ct.CreatedByUser)
                    .Include(ct => ct.UpdatedByUser)
                    .Include(ct => ct.DeletedByUser)
                    .FirstOrDefaultAsync(ct => ct.Id == customerType.Id && !ct.IsDeleted).ConfigureAwait(false);

                if (customerTypeWithNav == null)
                {
                    return ApiResponse<CustomerTypeGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("CustomerTypeService.CustomerTypeNotFound"),
                        _localizationService.GetLocalizedString("CustomerTypeService.CustomerTypeNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var customerTypeDto = _mapper.Map<CustomerTypeGetDto>(customerTypeWithNav);

                return ApiResponse<CustomerTypeGetDto>.SuccessResult(customerTypeDto, _localizationService.GetLocalizedString("CustomerTypeService.CustomerTypeCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<CustomerTypeGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("CustomerTypeService.InternalServerError"),
                    _localizationService.GetLocalizedString("CustomerTypeService.CreateCustomerTypeExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<CustomerTypeGetDto>> UpdateCustomerTypeAsync(long id, CustomerTypeUpdateDto customerTypeUpdateDto)
        {
            try
            {
                // Get tracked entity for update
                var customerType = await _unitOfWork.CustomerTypes.GetByIdForUpdateAsync(id).ConfigureAwait(false);
                if (customerType == null)
                {
                    return ApiResponse<CustomerTypeGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("CustomerTypeService.CustomerTypeNotFound"),
                        _localizationService.GetLocalizedString("CustomerTypeService.CustomerTypeNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(customerTypeUpdateDto, customerType);
                await _unitOfWork.CustomerTypes.UpdateAsync(customerType).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                // Reload with navigation properties for mapping (read-only)
                var customerTypeWithNav = await _unitOfWork.CustomerTypes
                    .Query()
                    .Include(ct => ct.CreatedByUser)
                    .Include(ct => ct.UpdatedByUser)
                    .Include(ct => ct.DeletedByUser)
                    .FirstOrDefaultAsync(ct => ct.Id == id).ConfigureAwait(false);

                if (customerTypeWithNav == null)
                {
                    return ApiResponse<CustomerTypeGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("CustomerTypeService.CustomerTypeNotFound"),
                        _localizationService.GetLocalizedString("CustomerTypeService.CustomerTypeNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var customerTypeDto = _mapper.Map<CustomerTypeGetDto>(customerTypeWithNav);

                return ApiResponse<CustomerTypeGetDto>.SuccessResult(customerTypeDto, _localizationService.GetLocalizedString("CustomerTypeService.CustomerTypeUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<CustomerTypeGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("CustomerTypeService.InternalServerError"),
                    _localizationService.GetLocalizedString("CustomerTypeService.UpdateCustomerTypeExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteCustomerTypeAsync(long id)
        {
            try
            {
                var deleted = await _unitOfWork.CustomerTypes.SoftDeleteAsync(id).ConfigureAwait(false);
                if (!deleted)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("CustomerTypeService.CustomerTypeNotFound"),
                        _localizationService.GetLocalizedString("CustomerTypeService.CustomerTypeNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("CustomerTypeService.CustomerTypeDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("CustomerTypeService.InternalServerError"),
                    _localizationService.GetLocalizedString("CustomerTypeService.DeleteCustomerTypeExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
