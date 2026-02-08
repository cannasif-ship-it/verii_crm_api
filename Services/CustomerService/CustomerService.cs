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
using System.Linq;
using System.Net.Mail;

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
                var governanceError = await ValidateCustomerGovernanceAsync(customerCreateDto, null);
                if (governanceError != null)
                {
                    return governanceError;
                }

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

                var governanceError = await ValidateCustomerGovernanceAsync(customerUpdateDto, id);
                if (governanceError != null)
                {
                    return governanceError;
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

            foreach (var erpCustomer in erpResponse.Data)
            {
                var code = erpCustomer.CariKod?.Trim();
                if (string.IsNullOrWhiteSpace(code))
                    continue;

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

            // ERP'de olmayan müşteriler silinmez; CRM'de manuel eklenen potansiyel müşteriler korunur.

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
                    "Customer sync completed: created={Created}, updated={Updated}, reactivated={Reactivated}.",
                    createdCount,
                    updatedCount,
                    reactivatedCount);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<ApiResponse<List<CustomerDuplicateCandidateDto>>> GetDuplicateCandidatesAsync()
        {
            try
            {
                var customers = await _unitOfWork.Customers
                    .Query()
                    .Where(x => !x.IsDeleted)
                    .AsNoTracking()
                    .ToListAsync();

                var candidates = new List<CustomerDuplicateCandidateDto>();
                candidates.AddRange(BuildDuplicateCandidates(customers, c => NormalizeDigits(c.TaxNumber), "TaxNumber", 0.95m));
                candidates.AddRange(BuildDuplicateCandidates(customers, c => NormalizeDigits(c.TcknNumber), "TcknNumber", 0.95m));
                candidates.AddRange(BuildDuplicateCandidates(customers, c => NormalizeText(c.CustomerCode), "CustomerCode", 0.85m));

                var merged = candidates
                    .GroupBy(x => new { x.MasterCustomerId, x.DuplicateCustomerId })
                    .Select(g => g.OrderByDescending(x => x.Score).First())
                    .OrderByDescending(x => x.Score)
                    .ThenBy(x => x.MasterCustomerId)
                    .ThenBy(x => x.DuplicateCustomerId)
                    .ToList();

                return ApiResponse<List<CustomerDuplicateCandidateDto>>.SuccessResult(
                    merged,
                    _localizationService.GetLocalizedString("CustomerService.DuplicateCandidatesRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CustomerDuplicateCandidateDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("CustomerService.InternalServerError"),
                    _localizationService.GetLocalizedString("CustomerService.DuplicateCandidatesExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<CustomerGetDto>> MergeCustomersAsync(CustomerMergeRequestDto request)
        {
            try
            {
                if (request.MasterCustomerId <= 0 || request.DuplicateCustomerId <= 0 || request.MasterCustomerId == request.DuplicateCustomerId)
                {
                    return ApiResponse<CustomerGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("CustomerService.InvalidMergeRequest"),
                        _localizationService.GetLocalizedString("CustomerService.InvalidMergeRequest"),
                        StatusCodes.Status400BadRequest);
                }

                var master = await _unitOfWork.Customers.GetByIdForUpdateAsync(request.MasterCustomerId);
                var duplicate = await _unitOfWork.Customers.GetByIdForUpdateAsync(request.DuplicateCustomerId);
                if (master == null || duplicate == null || master.IsDeleted || duplicate.IsDeleted)
                {
                    return ApiResponse<CustomerGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("CustomerService.CustomerNotFound"),
                        _localizationService.GetLocalizedString("CustomerService.CustomerNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var duplicateContacts = await _unitOfWork.Contacts
                    .Query(tracking: true)
                    .Where(x => !x.IsDeleted && x.CustomerId == duplicate.Id)
                    .ToListAsync();

                var masterContactKeys = await _unitOfWork.Contacts
                    .Query()
                    .Where(x => !x.IsDeleted && x.CustomerId == master.Id)
                    .Select(x => BuildContactKey(x.FullName, x.Email, x.Mobile, x.Phone))
                    .ToListAsync();

                var masterContactSet = masterContactKeys.ToHashSet(StringComparer.OrdinalIgnoreCase);
                foreach (var contact in duplicateContacts)
                {
                    var key = BuildContactKey(contact.FullName, contact.Email, contact.Mobile, contact.Phone);
                    if (masterContactSet.Contains(key))
                    {
                        await _unitOfWork.Contacts.SoftDeleteAsync(contact.Id);
                        continue;
                    }

                    contact.CustomerId = master.Id;
                    await _unitOfWork.Contacts.UpdateAsync(contact);
                    masterContactSet.Add(key);
                }

                if (!request.PreferMasterValues)
                {
                    master.CustomerName = FirstNonEmpty(master.CustomerName, duplicate.CustomerName) ?? master.CustomerName;
                    master.CustomerCode = FirstNonEmpty(master.CustomerCode, duplicate.CustomerCode);
                    master.TaxOffice = FirstNonEmpty(master.TaxOffice, duplicate.TaxOffice);
                    master.TaxNumber = FirstNonEmpty(master.TaxNumber, duplicate.TaxNumber);
                    master.TcknNumber = FirstNonEmpty(master.TcknNumber, duplicate.TcknNumber);
                    master.Email = FirstNonEmpty(master.Email, duplicate.Email);
                    master.Website = FirstNonEmpty(master.Website, duplicate.Website);
                    master.Phone1 = FirstNonEmpty(master.Phone1, duplicate.Phone1);
                    master.Phone2 = FirstNonEmpty(master.Phone2, duplicate.Phone2);
                    master.Address = FirstNonEmpty(master.Address, duplicate.Address);
                    master.Notes = FirstNonEmpty(master.Notes, duplicate.Notes);
                }

                await _unitOfWork.Customers.UpdateAsync(master);
                await _unitOfWork.Customers.SoftDeleteAsync(duplicate.Id);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation(
                    "Customer merge completed. Master: {MasterId}, Duplicate: {DuplicateId}, PreferMasterValues: {PreferMasterValues}",
                    master.Id,
                    duplicate.Id,
                    request.PreferMasterValues);

                var merged = await _unitOfWork.Customers
                    .Query()
                    .Include(c => c.Countries)
                    .Include(c => c.Cities)
                    .Include(c => c.Districts)
                    .Include(c => c.CustomerTypes)
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.UpdatedByUser)
                    .Include(c => c.DeletedByUser)
                    .FirstOrDefaultAsync(c => c.Id == master.Id && !c.IsDeleted);

                if (merged == null)
                {
                    return ApiResponse<CustomerGetDto>.ErrorResult(
                        _localizationService.GetLocalizedString("CustomerService.CustomerNotFound"),
                        _localizationService.GetLocalizedString("CustomerService.CustomerNotFound"),
                        StatusCodes.Status404NotFound);
                }

                return ApiResponse<CustomerGetDto>.SuccessResult(
                    _mapper.Map<CustomerGetDto>(merged),
                    _localizationService.GetLocalizedString("CustomerService.CustomerMerged"));
            }
            catch (Exception ex)
            {
                return ApiResponse<CustomerGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("CustomerService.InternalServerError"),
                    _localizationService.GetLocalizedString("CustomerService.MergeCustomersExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        private async Task<ApiResponse<CustomerGetDto>?> ValidateCustomerGovernanceAsync(CustomerCreateDto dto, long? excludedId)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return ApiResponse<CustomerGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("CustomerService.CustomerNameRequired"),
                    _localizationService.GetLocalizedString("CustomerService.CustomerNameRequired"),
                    StatusCodes.Status400BadRequest);
            }

            if (!IsValidTaxNumber(dto.TaxNumber))
            {
                return ApiResponse<CustomerGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("CustomerService.InvalidTaxNumber"),
                    _localizationService.GetLocalizedString("CustomerService.InvalidTaxNumber"),
                    StatusCodes.Status400BadRequest);
            }

            if (!IsValidTcknNumber(dto.TcknNumber))
            {
                return ApiResponse<CustomerGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("CustomerService.InvalidTcknNumber"),
                    _localizationService.GetLocalizedString("CustomerService.InvalidTcknNumber"),
                    StatusCodes.Status400BadRequest);
            }

            if (!IsValidEmail(dto.Email))
            {
                return ApiResponse<CustomerGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("CustomerService.InvalidEmail"),
                    _localizationService.GetLocalizedString("CustomerService.InvalidEmail"),
                    StatusCodes.Status400BadRequest);
            }

            var tax = NormalizeDigits(dto.TaxNumber);
            var tckn = NormalizeDigits(dto.TcknNumber);
            var customerCode = NormalizeText(dto.CustomerCode);
            var branchCode = dto.BranchCode;

            var duplicateQuery = _unitOfWork.Customers
                .Query()
                .Where(c => !c.IsDeleted);

            if (excludedId.HasValue)
            {
                duplicateQuery = duplicateQuery.Where(c => c.Id != excludedId.Value);
            }

            var isDuplicate = await duplicateQuery.AnyAsync(c =>
                (!string.IsNullOrWhiteSpace(tax) && NormalizeDigits(c.TaxNumber) == tax) ||
                (!string.IsNullOrWhiteSpace(tckn) && NormalizeDigits(c.TcknNumber) == tckn) ||
                (!string.IsNullOrWhiteSpace(customerCode) && c.BranchCode == branchCode && NormalizeText(c.CustomerCode) == customerCode));

            if (isDuplicate)
            {
                return ApiResponse<CustomerGetDto>.ErrorResult(
                    _localizationService.GetLocalizedString("CustomerService.DuplicateCustomer"),
                    _localizationService.GetLocalizedString("CustomerService.DuplicateCustomer"),
                    StatusCodes.Status409Conflict);
            }

            return null;
        }

        private async Task<ApiResponse<CustomerGetDto>?> ValidateCustomerGovernanceAsync(CustomerUpdateDto dto, long excludedId)
        {
            var createLike = new CustomerCreateDto
            {
                Name = dto.Name,
                CustomerCode = dto.CustomerCode,
                TaxOffice = dto.TaxOffice,
                TaxNumber = dto.TaxNumber,
                TcknNumber = dto.TcknNumber,
                Email = dto.Email,
                Website = dto.Website,
                Phone = dto.Phone,
                Phone2 = dto.Phone2,
                Address = dto.Address,
                Notes = dto.Notes,
                CountryId = dto.CountryId,
                CityId = dto.CityId,
                DistrictId = dto.DistrictId,
                CustomerTypeId = dto.CustomerTypeId,
                SalesRepCode = dto.SalesRepCode,
                GroupCode = dto.GroupCode,
                CreditLimit = dto.CreditLimit,
                BranchCode = dto.BranchCode,
                BusinessUnitCode = dto.BusinessUnitCode,
                IsCompleted = dto.IsCompleted
            };

            return await ValidateCustomerGovernanceAsync(createLike, excludedId);
        }

        private static List<CustomerDuplicateCandidateDto> BuildDuplicateCandidates(
            List<Customer> customers,
            Func<Customer, string> keySelector,
            string matchType,
            decimal score)
        {
            var result = new List<CustomerDuplicateCandidateDto>();
            var groups = customers
                .Select(c => new { Customer = c, Key = keySelector(c) })
                .Where(x => !string.IsNullOrWhiteSpace(x.Key))
                .GroupBy(x => x.Key, StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1);

            foreach (var group in groups)
            {
                var ordered = group.Select(x => x.Customer).OrderBy(x => x.Id).ToList();
                var master = ordered.First();
                foreach (var duplicate in ordered.Skip(1))
                {
                    result.Add(new CustomerDuplicateCandidateDto
                    {
                        MasterCustomerId = master.Id,
                        MasterCustomerName = master.CustomerName,
                        DuplicateCustomerId = duplicate.Id,
                        DuplicateCustomerName = duplicate.CustomerName,
                        MatchType = matchType,
                        Score = score
                    });
                }
            }

            return result;
        }

        private static string BuildContactKey(string? fullName, string? email, string? mobile, string? phone)
        {
            var normalizedName = NormalizeText(fullName);
            var normalizedEmail = NormalizeText(email);
            var normalizedMobile = NormalizeDigits(mobile);
            var normalizedPhone = NormalizeDigits(phone);
            return $"{normalizedName}|{normalizedEmail}|{normalizedMobile}|{normalizedPhone}";
        }

        private static string? FirstNonEmpty(string? first, string? second)
        {
            if (!string.IsNullOrWhiteSpace(first))
                return first;
            return string.IsNullOrWhiteSpace(second) ? null : second;
        }

        private static string NormalizeText(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim().ToUpperInvariant();
        }

        private static string NormalizeDigits(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return new string(value.Where(char.IsDigit).ToArray());
        }

        private static bool IsValidTaxNumber(string? value)
        {
            var normalized = NormalizeDigits(value);
            return string.IsNullOrWhiteSpace(normalized) || normalized.Length == 10;
        }

        private static bool IsValidTcknNumber(string? value)
        {
            var normalized = NormalizeDigits(value);
            return string.IsNullOrWhiteSpace(normalized) || normalized.Length == 11;
        }

        private static bool IsValidEmail(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return true;

            try
            {
                _ = new MailAddress(value.Trim());
                return true;
            }
            catch
            {
                return false;
            }
        }
   
    }
}
