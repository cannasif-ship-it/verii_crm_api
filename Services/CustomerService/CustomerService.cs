using AutoMapper;
using crm_api.DTOs;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.UnitOfWork;
using crm_api.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace crm_api.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILocalizationService localizationService,
            IErpService erpService,
            ILogger<CustomerService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
            _logger = logger;
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

        public async Task SyncCustomersFromErpAsync()
        {
            var erpResponse = await _erpService.GetCarisAsync(null);

            if (erpResponse?.Data == null || erpResponse.Data.Count == 0)
            {
                _logger.LogInformation("Customer sync skipped: no ERP records returned.");
                return;
            }

            _logger.LogInformation("Customer sync fetched {Count} ERP records.", erpResponse.Data.Count);

            var existingCustomers = await _unitOfWork.Customers
                .Query(tracking: true, ignoreQueryFilters: true)
                .ToListAsync();

            var customerByCode = existingCustomers
                .Where(x => !string.IsNullOrWhiteSpace(x.CustomerCode))
                .ToDictionary(x => x.CustomerCode!, StringComparer.OrdinalIgnoreCase);

            var newCustomers = new List<Customer>();
            var hasAnyChange = false;
            var createdCount = 0;
            var updatedCount = 0;
            var reactivatedCount = 0;
            var deletedCount = 0;
            var erpCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var erpCustomer in erpResponse.Data)
            {
                var code = erpCustomer.CariKod?.Trim();
                if (string.IsNullOrWhiteSpace(code))
                    continue;
                erpCodes.Add(code);

                if (!customerByCode.TryGetValue(code, out var customer))
                {
                    var name = string.IsNullOrWhiteSpace(erpCustomer.CariIsim) ? code : erpCustomer.CariIsim!.Trim();
                    newCustomers.Add(new Customer
                    {
                        CustomerCode = code,
                        CustomerName = name,
                        TaxOffice = erpCustomer.VergiDairesi,
                        TaxNumber = erpCustomer.VergiNumarasi,
                        TcknNumber = erpCustomer.TcknNumber,
                        Email = erpCustomer.Email,
                        Website = erpCustomer.Web,
                        Phone1 = erpCustomer.CariTel,
                        Address = erpCustomer.CariAdres,
                        BranchCode = erpCustomer.SubeKodu,
                        BusinessUnitCode = erpCustomer.IsletmeKodu,
                        IsERPIntegrated = true,
                        ERPIntegrationNumber = code,
                        LastSyncDate = DateTime.UtcNow
                    });
                    createdCount++;
                    hasAnyChange = true;
                    continue;
                }

                var updated = false;
                var reactivated = false;
                var newName = string.IsNullOrWhiteSpace(erpCustomer.CariIsim) ? code : erpCustomer.CariIsim!.Trim();

                if (customer.CustomerName != newName) { customer.CustomerName = newName; updated = true; }
                if (customer.TaxOffice != erpCustomer.VergiDairesi) { customer.TaxOffice = erpCustomer.VergiDairesi; updated = true; }
                if (customer.TaxNumber != erpCustomer.VergiNumarasi) { customer.TaxNumber = erpCustomer.VergiNumarasi; updated = true; }
                if (customer.TcknNumber != erpCustomer.TcknNumber) { customer.TcknNumber = erpCustomer.TcknNumber; updated = true; }
                if (customer.Email != erpCustomer.Email) { customer.Email = erpCustomer.Email; updated = true; }
                if (customer.Website != erpCustomer.Web) { customer.Website = erpCustomer.Web; updated = true; }
                if (customer.Phone1 != erpCustomer.CariTel) { customer.Phone1 = erpCustomer.CariTel; updated = true; }
                if (customer.Address != erpCustomer.CariAdres) { customer.Address = erpCustomer.CariAdres; updated = true; }
                if (customer.BranchCode != erpCustomer.SubeKodu) { customer.BranchCode = erpCustomer.SubeKodu; updated = true; }
                if (customer.BusinessUnitCode != erpCustomer.IsletmeKodu) { customer.BusinessUnitCode = erpCustomer.IsletmeKodu; updated = true; }

                if (customer.IsDeleted)
                {
                    customer.IsDeleted = false;
                    customer.DeletedDate = null;
                    customer.DeletedBy = null;
                    updated = true;
                    reactivated = true;
                    reactivatedCount++;
                }

                if (customer.IsERPIntegrated != true) { customer.IsERPIntegrated = true; updated = true; }
                if (customer.ERPIntegrationNumber != code) { customer.ERPIntegrationNumber = code; updated = true; }

                if (updated)
                {
                    customer.LastSyncDate = DateTime.UtcNow;
                    hasAnyChange = true;
                    if (!reactivated)
                        updatedCount++;
                }
            }

            foreach (var existing in existingCustomers.Where(x =>
                         !x.IsDeleted &&
                         (x.IsERPIntegrated || !string.IsNullOrWhiteSpace(x.ERPIntegrationNumber) || !string.IsNullOrWhiteSpace(x.CustomerCode)) &&
                         !string.IsNullOrWhiteSpace(x.CustomerCode) &&
                         !erpCodes.Contains(x.CustomerCode!)))
            {
                var deleted = await _unitOfWork.Customers.SoftDeleteAsync(existing.Id);
                if (deleted)
                {
                    deletedCount++;
                    hasAnyChange = true;
                }
            }

            if (!hasAnyChange)
            {
                _logger.LogInformation("Customer sync completed: no changes detected.");
                return;
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                if (newCustomers.Count > 0)
                    await _unitOfWork.Customers.AddAllAsync(newCustomers);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation(
                    "Customer sync completed: created={Created}, updated={Updated}, reactivated={Reactivated}, deleted={Deleted}.",
                    createdCount,
                    updatedCount,
                    reactivatedCount,
                    deletedCount);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
