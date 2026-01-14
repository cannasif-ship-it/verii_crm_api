using AutoMapper;
using cms_webapi.DTOs;
using cms_webapi.Interfaces;
using cms_webapi.Models;
using cms_webapi.UnitOfWork;
using cms_webapi.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace cms_webapi.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<CustomerGetDto>>> GetAllCustomersAsync(PagedRequest request)
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

                var query = _unitOfWork.Customers
                    .Query()
                    .Where(c => !c.IsDeleted)
                    .Include(c => c.Countries)
                    .Include(c => c.Cities)
                    .Include(c => c.Districts)
                    .Include(c => c.CustomerTypes)
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.UpdatedByUser)
                    .Include(c => c.DeletedByUser)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(Customer.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<CustomerGetDto>(x)).ToList();

                var pagedResponse = new PagedResponse<CustomerGetDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<CustomerGetDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("CustomerService.CustomersRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<CustomerGetDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("CustomerService.InternalServerError"),
                    _localizationService.GetLocalizedString("CustomerService.GetAllCustomersExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<CustomerGetDto>> GetCustomerByIdAsync(long id)
        {
            try
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(id);
                if (customer == null)
                {
                    return ApiResponse<CustomerGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("CustomerService.CustomerNotFound"),
                        _localizationService.GetLocalizedString("CustomerService.CustomerNotFound"),
                        StatusCodes.Status404NotFound);
                }

                // Reload with navigation properties for mapping
                var customerWithNav = await _unitOfWork.Customers
                    .Query()
                    .Include(c => c.Countries)
                    .Include(c => c.Cities)
                    .Include(c => c.Districts)
                    .Include(c => c.CustomerTypes)
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.UpdatedByUser)
                    .Include(c => c.DeletedByUser)
                    .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

                var customerDto = _mapper.Map<CustomerGetDto>(customerWithNav ?? customer);
                return ApiResponse<CustomerGetDto>.SuccessResult(customerDto, _localizationService.GetLocalizedString("CustomerService.CustomerRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<CustomerGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("CustomerService.InternalServerError"),
                    _localizationService.GetLocalizedString("CustomerService.GetCustomerByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<CustomerGetDto>> CreateCustomerAsync(CustomerCreateDto customerCreateDto)
        {
            try
            {
                var customer = _mapper.Map<Customer>(customerCreateDto);
                await _unitOfWork.Customers.AddAsync(customer);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var customerWithNav = await _unitOfWork.Customers
                    .Query()
                    .Include(c => c.Countries)
                    .Include(c => c.Cities)
                    .Include(c => c.Districts)
                    .Include(c => c.CustomerTypes)
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.UpdatedByUser)
                    .Include(c => c.DeletedByUser)
                    .FirstOrDefaultAsync(c => c.Id == customer.Id && !c.IsDeleted);

                if (customerWithNav == null)
                {
                    return ApiResponse<CustomerGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("CustomerService.CustomerNotFound"),
                        _localizationService.GetLocalizedString("CustomerService.CustomerNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var customerDto = _mapper.Map<CustomerGetDto>(customerWithNav);

                return ApiResponse<CustomerGetDto>.SuccessResult(customerDto, _localizationService.GetLocalizedString("CustomerService.CustomerCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<CustomerGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("CustomerService.InternalServerError"),
                    _localizationService.GetLocalizedString("CustomerService.CreateCustomerExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<CustomerGetDto>> UpdateCustomerAsync(long id, CustomerUpdateDto customerUpdateDto)
        {
            try
            {
                var customer = await _unitOfWork.Customers.GetByIdForUpdateAsync(id);
                if (customer == null)
                {
                    return ApiResponse<CustomerGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("CustomerService.CustomerNotFound"),
                        _localizationService.GetLocalizedString("CustomerService.CustomerNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(customerUpdateDto, customer);
                await _unitOfWork.Customers.UpdateAsync(customer);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var customerWithNav = await _unitOfWork.Customers
                    .Query()
                    .Include(c => c.Countries)
                    .Include(c => c.Cities)
                    .Include(c => c.Districts)
                    .Include(c => c.CustomerTypes)
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.UpdatedByUser)
                    .Include(c => c.DeletedByUser)
                    .FirstOrDefaultAsync(c => c.Id == customer.Id && !c.IsDeleted);

                if (customerWithNav == null)
                {
                    return ApiResponse<CustomerGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("CustomerService.CustomerNotFound"),
                        _localizationService.GetLocalizedString("CustomerService.CustomerNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var customerDto = _mapper.Map<CustomerGetDto>(customerWithNav);

                return ApiResponse<CustomerGetDto>.SuccessResult(customerDto, _localizationService.GetLocalizedString("CustomerService.CustomerUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<CustomerGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("CustomerService.InternalServerError"),
                    _localizationService.GetLocalizedString("CustomerService.UpdateCustomerExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteCustomerAsync(long id)
        {
            try
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(id);
                if (customer == null)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("CustomerService.CustomerNotFound"),
                        _localizationService.GetLocalizedString("CustomerService.CustomerNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.Customers.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("CustomerService.CustomerDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("CustomerService.InternalServerError"),
                    _localizationService.GetLocalizedString("CustomerService.DeleteCustomerExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
