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
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace crm_api.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;
        private readonly ILogger<CustomerService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILocalizationService localizationService,
            IErpService erpService,
            ILogger<CustomerService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
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

                var columnMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "name", "CustomerName" },
                    { "phone", "Phone1" },
                    { "countryName", "Country.Name" },
                    { "cityName", "City.Name" },
                    { "districtName", "District.Name" },
                    { "customerTypeName", "CustomerType.Name" }
                };

                var query = _unitOfWork.Customers
                    .Query()
                    .Where(c => !c.IsDeleted)
                    .Include(c => c.Country)
                    .Include(c => c.City)
                    .Include(c => c.District)
                    .Include(c => c.CustomerType)
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.UpdatedByUser)
                    .Include(c => c.DeletedByUser)
                    .ApplyFilters(request.Filters, request.FilterLogic, columnMapping);

                var sortBy = request.SortBy ?? nameof(Customer.Id);

                query = query.ApplySorting(sortBy, request.SortDirection, columnMapping);

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
                    .Include(c => c.Country)
                    .Include(c => c.City)
                    .Include(c => c.District)
                    .Include(c => c.CustomerType)
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

        public async Task<ApiResponse<List<NearbyCustomerPinDto>>> GetNearbyCustomersAsync(CustomerNearbyQueryDto query)
        {
            try
            {
                if (query == null)
                {
                    return ApiResponse<List<NearbyCustomerPinDto>>.ErrorResult(
                        _localizationService.GetLocalizedString("CustomerService.InvalidRequest"),
                        _localizationService.GetLocalizedString("CustomerService.InvalidRequest"),
                        StatusCodes.Status400BadRequest);
                }

                if (query.Latitude < -90 || query.Latitude > 90 || query.Longitude < -180 || query.Longitude > 180)
                {
                    return ApiResponse<List<NearbyCustomerPinDto>>.ErrorResult(
                        _localizationService.GetLocalizedString("CustomerService.InvalidCoordinates"),
                        _localizationService.GetLocalizedString("CustomerService.InvalidCoordinates"),
                        StatusCodes.Status400BadRequest);
                }

                var radiusKm = query.RadiusKm <= 0 ? 10d : Math.Min(query.RadiusKm, 50d);

                var columnMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "name", "CustomerName" },
                    { "customerCode", "CustomerCode" },
                    { "customerTypeName", "CustomerType.Name" },
                    { "countryName", "Country.Name" },
                    { "cityName", "City.Name" },
                    { "districtName", "District.Name" },
                    { "phone", "Phone1" },
                    { "branchCode", "BranchCode" }
                };

                var parsedFilters = ParseFiltersJson(query.Filters);

                var customerQuery = _unitOfWork.Customers
                    .Query()
                    .Where(c => !c.IsDeleted)
                    .Include(c => c.CustomerType)
                    .Include(c => c.Country)
                    .Include(c => c.City)
                    .Include(c => c.District)
                    .Include(c => c.ShippingAddresses)
                        .ThenInclude(sa => sa.Country)
                    .Include(c => c.ShippingAddresses)
                        .ThenInclude(sa => sa.City)
                    .Include(c => c.ShippingAddresses)
                        .ThenInclude(sa => sa.District)
                    .ApplyFilters(parsedFilters, query.FilterLogic, columnMapping);

                var branchCodeStr = _httpContextAccessor.HttpContext?.Items["BranchCode"]?.ToString();
                if (short.TryParse(branchCodeStr, out var branchCode) && branchCode > 0)
                {
                    customerQuery = customerQuery.Where(c => c.BranchCode == branchCode);
                }

                var customers = await customerQuery.ToListAsync();
                var result = new List<NearbyCustomerPinDto>();

                foreach (var customer in customers)
                {
                    if (customer.Latitude.HasValue && customer.Longitude.HasValue)
                    {
                        var distance = CalculateDistanceKm(
                            query.Latitude,
                            query.Longitude,
                            (double)customer.Latitude.Value,
                            (double)customer.Longitude.Value);

                        if (distance <= radiusKm)
                        {
                            result.Add(new NearbyCustomerPinDto
                            {
                                Id = customer.Id,
                                CustomerId = customer.Id,
                                CustomerCode = customer.CustomerCode,
                                Name = customer.CustomerName,
                                AddressDisplay = BuildAddressDisplay(customer.Address, customer.District?.Name, customer.City?.Name, customer.Country?.Name),
                                Latitude = (double)customer.Latitude.Value,
                                Longitude = (double)customer.Longitude.Value,
                                Source = "main",
                                ShippingAddressId = null,
                                CustomerTypeName = customer.CustomerType?.Name,
                                Phone = FirstNonEmpty(customer.Phone1, customer.Phone2)
                            });
                        }
                    }

                    if (!query.IncludeShippingAddresses || customer.ShippingAddresses == null)
                        continue;

                    foreach (var shipping in customer.ShippingAddresses.Where(x => !x.IsDeleted))
                    {
                        if (!shipping.Latitude.HasValue || !shipping.Longitude.HasValue)
                            continue;

                        var distance = CalculateDistanceKm(
                            query.Latitude,
                            query.Longitude,
                            (double)shipping.Latitude.Value,
                            (double)shipping.Longitude.Value);

                        if (distance > radiusKm)
                            continue;

                        result.Add(new NearbyCustomerPinDto
                        {
                            Id = (customer.Id * 1_000_000L) + shipping.Id,
                            CustomerId = customer.Id,
                            CustomerCode = customer.CustomerCode,
                            Name = customer.CustomerName,
                            AddressDisplay = BuildAddressDisplay(shipping.Address, shipping.District?.Name, shipping.City?.Name, shipping.Country?.Name),
                            Latitude = (double)shipping.Latitude.Value,
                            Longitude = (double)shipping.Longitude.Value,
                            Source = "shipping",
                            ShippingAddressId = shipping.Id,
                            CustomerTypeName = customer.CustomerType?.Name,
                            Phone = FirstNonEmpty(shipping.Phone, FirstNonEmpty(customer.Phone1, customer.Phone2))
                        });
                    }
                }

                const int maxItems = 500;
                var ordered = result
                    .OrderBy(x => CalculateDistanceKm(query.Latitude, query.Longitude, x.Latitude, x.Longitude))
                    .Take(maxItems)
                    .ToList();

                return ApiResponse<List<NearbyCustomerPinDto>>.SuccessResult(
                    ordered,
                    _localizationService.GetLocalizedString("CustomerService.CustomersRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<NearbyCustomerPinDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("CustomerService.InternalServerError"),
                    _localizationService.GetLocalizedString("CustomerService.GetAllCustomersExceptionMessage", ex.Message),
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
                    .Include(c => c.Country)
                    .Include(c => c.City)
                    .Include(c => c.District)
                    .Include(c => c.CustomerType)
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

        public async Task<ApiResponse<CustomerCreateFromMobileResultDto>> CreateCustomerFromMobileAsync(CustomerCreateFromMobileDto request)
        {
            try
            {
                string? normalizeNullable(string? value)
                {
                    return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
                }

                bool tryFillIfEmpty(string? currentValue, string? source, out string? updatedValue)
                {
                    updatedValue = currentValue;
                    if (!string.IsNullOrWhiteSpace(currentValue))
                        return false;

                    var normalized = normalizeNullable(source);
                    if (string.IsNullOrWhiteSpace(normalized))
                        return false;

                    updatedValue = normalized;
                    return true;
                }

                (string FirstName, string? MiddleName, string LastName) splitNameParts(string fullName)
                {
                    var tokens = fullName
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .ToList();

                    if (tokens.Count == 0)
                        return ("-", null, "-");

                    if (tokens.Count == 1)
                        return (tokens[0], null, tokens[0]);

                    var firstName = tokens.First();
                    var lastName = tokens.Last();
                    var middleTokens = tokens.Skip(1).Take(tokens.Count - 2).ToList();
                    var middleName = middleTokens.Count > 0 ? string.Join(" ", middleTokens) : null;

                    return (firstName, middleName, lastName);
                }

                bool isMobile(string? value)
                {
                    var digits = NormalizeDigits(value);
                    if (string.IsNullOrWhiteSpace(digits)) return false;
                    if (digits.Length == 12 && digits.StartsWith("90")) return digits.Substring(2).StartsWith("5");
                    if (digits.Length == 11 && digits.StartsWith("0")) return digits.Substring(1).StartsWith("5");
                    if (digits.Length == 10) return digits.StartsWith("5");
                    return false;
                }

                string? selectMobile(string? phone1, string? phone2)
                {
                    var candidate1 = normalizeNullable(phone1);
                    var candidate2 = normalizeNullable(phone2);
                    if (isMobile(candidate1)) return candidate1;
                    if (isMobile(candidate2)) return candidate2;
                    return null;
                }

                string? selectLandline(string? phone1, string? phone2)
                {
                    var candidate1 = normalizeNullable(phone1);
                    var candidate2 = normalizeNullable(phone2);
                    if (!string.IsNullOrWhiteSpace(candidate1) && !isMobile(candidate1)) return candidate1;
                    if (!string.IsNullOrWhiteSpace(candidate2) && !isMobile(candidate2)) return candidate2;
                    return null;
                }

                if (request == null || string.IsNullOrWhiteSpace(request.Name))
                {
                    return ApiResponse<CustomerCreateFromMobileResultDto>.ErrorResult(
                        _localizationService.GetLocalizedString("CustomerService.CustomerNameRequired"),
                        _localizationService.GetLocalizedString("CustomerService.CustomerNameRequired"),
                        StatusCodes.Status400BadRequest);
                }

                if (!IsValidEmail(request.Email))
                {
                    return ApiResponse<CustomerCreateFromMobileResultDto>.ErrorResult(
                        _localizationService.GetLocalizedString("CustomerService.InvalidEmail"),
                        _localizationService.GetLocalizedString("CustomerService.InvalidEmail"),
                        StatusCodes.Status400BadRequest);
                }

                var normalizedCustomerName = NormalizeText(request.Name);
                var normalizedEmail = NormalizeText(request.Email);
                var normalizedPhone = NormalizeDigits(request.Phone);
                var normalizedPhone2 = NormalizeDigits(request.Phone2);

                await _unitOfWork.BeginTransactionAsync();

                try
                {
                    var customerQuery = _unitOfWork.Customers
                        .Query(tracking: true)
                        .Where(c => !c.IsDeleted);

                    Customer? customer = null;

                    if (!string.IsNullOrWhiteSpace(normalizedEmail))
                    {
                        customer = await customerQuery.FirstOrDefaultAsync(c =>
                            c.Email != null && c.Email.Trim().ToUpper() == normalizedEmail);
                    }

                    if (customer == null && (!string.IsNullOrWhiteSpace(normalizedPhone) || !string.IsNullOrWhiteSpace(normalizedPhone2)))
                    {
                        var phoneCandidates = await customerQuery
                            .Select(c => new { Customer = c, c.Phone1, c.Phone2 })
                            .ToListAsync();

                        customer = phoneCandidates.FirstOrDefault(c =>
                            (!string.IsNullOrWhiteSpace(normalizedPhone) &&
                             (NormalizeDigits(c.Phone1) == normalizedPhone || NormalizeDigits(c.Phone2) == normalizedPhone)) ||
                            (!string.IsNullOrWhiteSpace(normalizedPhone2) &&
                             (NormalizeDigits(c.Phone1) == normalizedPhone2 || NormalizeDigits(c.Phone2) == normalizedPhone2))
                        )?.Customer;
                    }

                    if (customer == null)
                    {
                        customer = await customerQuery.FirstOrDefaultAsync(c => c.CustomerName.Trim().ToUpper() == normalizedCustomerName);
                    }

                    var customerCreated = false;
                    if (customer == null)
                    {
                        customer = new Customer
                        {
                            CustomerName = request.Name.Trim(),
                            Email = normalizeNullable(request.Email),
                            Phone1 = normalizeNullable(request.Phone),
                            Phone2 = normalizeNullable(request.Phone2),
                            Address = normalizeNullable(request.Address),
                            Website = normalizeNullable(request.Website),
                            Notes = normalizeNullable(request.Notes),
                            CountryId = request.CountryId,
                            CityId = request.CityId,
                            DistrictId = request.DistrictId,
                            CustomerTypeId = request.CustomerTypeId,
                            SalesRepCode = normalizeNullable(request.SalesRepCode),
                            GroupCode = normalizeNullable(request.GroupCode),
                            CreditLimit = request.CreditLimit,
                            BranchCode = request.BranchCode ?? 1,
                            BusinessUnitCode = request.BusinessUnitCode ?? 1
                        };

                        await _unitOfWork.Customers.AddAsync(customer);
                        customerCreated = true;
                    }
                    else
                    {
                        var changed = false;
                        if (tryFillIfEmpty(customer.Email, request.Email, out var customerEmail)) { customer.Email = customerEmail; changed = true; }
                        if (tryFillIfEmpty(customer.Phone1, request.Phone, out var customerPhone1)) { customer.Phone1 = customerPhone1; changed = true; }
                        if (tryFillIfEmpty(customer.Phone2, request.Phone2, out var customerPhone2)) { customer.Phone2 = customerPhone2; changed = true; }
                        if (tryFillIfEmpty(customer.Address, request.Address, out var customerAddress)) { customer.Address = customerAddress; changed = true; }
                        if (tryFillIfEmpty(customer.Website, request.Website, out var customerWebsite)) { customer.Website = customerWebsite; changed = true; }
                        if (tryFillIfEmpty(customer.Notes, request.Notes, out var customerNotes)) { customer.Notes = customerNotes; changed = true; }

                        if (!customer.CountryId.HasValue && request.CountryId.HasValue) { customer.CountryId = request.CountryId; changed = true; }
                        if (!customer.CityId.HasValue && request.CityId.HasValue) { customer.CityId = request.CityId; changed = true; }
                        if (!customer.DistrictId.HasValue && request.DistrictId.HasValue) { customer.DistrictId = request.DistrictId; changed = true; }
                        if (!customer.CustomerTypeId.HasValue && request.CustomerTypeId.HasValue) { customer.CustomerTypeId = request.CustomerTypeId; changed = true; }
                        if (string.IsNullOrWhiteSpace(customer.SalesRepCode) && !string.IsNullOrWhiteSpace(request.SalesRepCode)) { customer.SalesRepCode = request.SalesRepCode.Trim(); changed = true; }
                        if (string.IsNullOrWhiteSpace(customer.GroupCode) && !string.IsNullOrWhiteSpace(request.GroupCode)) { customer.GroupCode = request.GroupCode.Trim(); changed = true; }
                        if (!customer.CreditLimit.HasValue && request.CreditLimit.HasValue) { customer.CreditLimit = request.CreditLimit; changed = true; }
                        if (customer.BranchCode <= 0 && request.BranchCode.HasValue) { customer.BranchCode = request.BranchCode.Value; changed = true; }
                        if (customer.BusinessUnitCode <= 0 && request.BusinessUnitCode.HasValue) { customer.BusinessUnitCode = request.BusinessUnitCode.Value; changed = true; }

                        if (changed)
                        {
                            await _unitOfWork.Customers.UpdateAsync(customer);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();

                    long? titleId = null;
                    var titleCreated = false;
                    var normalizedTitle = normalizeNullable(request.Title);
                    if (!string.IsNullOrWhiteSpace(normalizedTitle))
                    {
                        var existingTitle = await _unitOfWork.Titles
                            .Query(tracking: true)
                            .FirstOrDefaultAsync(t => !t.IsDeleted && t.TitleName.Trim().ToUpper() == normalizedTitle.Trim().ToUpper());

                        if (existingTitle == null)
                        {
                            existingTitle = new Title
                            {
                                TitleName = normalizedTitle
                            };
                            await _unitOfWork.Titles.AddAsync(existingTitle);
                            await _unitOfWork.SaveChangesAsync();
                            titleCreated = true;
                        }

                        titleId = existingTitle.Id;
                    }

                    long? contactId = null;
                    var contactCreated = false;
                    var normalizedContactName = normalizeNullable(request.ContactName);
                    if (!string.IsNullOrWhiteSpace(normalizedContactName))
                    {
                        var (firstName, middleName, lastName) = splitNameParts(normalizedContactName);
                        var contactPhone = selectLandline(request.Phone, request.Phone2);
                        var contactMobile = selectMobile(request.Phone, request.Phone2);

                        var contactQuery = _unitOfWork.Contacts
                            .Query(tracking: true)
                            .Where(c => !c.IsDeleted && c.CustomerId == customer.Id);

                        Contact? contact = null;
                        if (!string.IsNullOrWhiteSpace(normalizedEmail))
                        {
                            contact = await contactQuery.FirstOrDefaultAsync(c =>
                                c.Email != null && c.Email.Trim().ToUpper() == normalizedEmail);
                        }

                        if (contact == null)
                        {
                            var normalizedContactPhone = NormalizeDigits(contactPhone);
                            var normalizedContactMobile = NormalizeDigits(contactMobile);
                            var normalizedFullName = NormalizeText(normalizedContactName);

                            var existingContacts = await contactQuery
                                .Select(c => new { Contact = c, c.FullName, c.Mobile, c.Phone })
                                .ToListAsync();

                            contact = existingContacts.FirstOrDefault(c =>
                                NormalizeText(c.FullName) == normalizedFullName &&
                                (
                                    (!string.IsNullOrWhiteSpace(normalizedContactMobile) && NormalizeDigits(c.Mobile) == normalizedContactMobile) ||
                                    (!string.IsNullOrWhiteSpace(normalizedContactPhone) && NormalizeDigits(c.Phone) == normalizedContactPhone)
                                )
                            )?.Contact;
                        }

                        if (contact == null)
                        {
                            contact = new Contact
                            {
                                Salutation = SalutationType.None,
                                FirstName = firstName,
                                MiddleName = middleName,
                                LastName = lastName,
                                FullName = normalizedContactName,
                                Email = normalizeNullable(request.Email),
                                Phone = normalizeNullable(contactPhone),
                                Mobile = normalizeNullable(contactMobile),
                                Notes = normalizeNullable(request.Notes),
                                CustomerId = customer.Id,
                                TitleId = titleId
                            };

                            await _unitOfWork.Contacts.AddAsync(contact);
                            contactCreated = true;
                        }
                        else
                        {
                            var changed = false;
                            if (!contact.TitleId.HasValue && titleId.HasValue) { contact.TitleId = titleId; changed = true; }
                            if (tryFillIfEmpty(contact.Email, request.Email, out var contactEmail)) { contact.Email = contactEmail; changed = true; }
                            if (tryFillIfEmpty(contact.Phone, contactPhone, out var contactPhoneFilled)) { contact.Phone = contactPhoneFilled; changed = true; }
                            if (tryFillIfEmpty(contact.Mobile, contactMobile, out var contactMobileFilled)) { contact.Mobile = contactMobileFilled; changed = true; }
                            if (tryFillIfEmpty(contact.Notes, request.Notes, out var contactNotes)) { contact.Notes = contactNotes; changed = true; }

                            if (changed)
                            {
                                await _unitOfWork.Contacts.UpdateAsync(contact);
                            }
                        }

                        await _unitOfWork.SaveChangesAsync();
                        contactId = contact.Id;
                    }

                    await _unitOfWork.CommitTransactionAsync();

                    var response = new CustomerCreateFromMobileResultDto
                    {
                        CustomerId = customer.Id,
                        CustomerCreated = customerCreated,
                        ContactId = contactId,
                        ContactCreated = contactCreated,
                        TitleId = titleId,
                        TitleCreated = titleCreated
                    };

                    return ApiResponse<CustomerCreateFromMobileResultDto>.SuccessResult(
                        response,
                        "Customer mobile OCR flow completed.");
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<CustomerCreateFromMobileResultDto>.ErrorResult(
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
                    .Include(c => c.Country)
                    .Include(c => c.City)
                    .Include(c => c.District)
                    .Include(c => c.CustomerType)
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
                    .Include(c => c.Country)
                    .Include(c => c.City)
                    .Include(c => c.District)
                    .Include(c => c.CustomerType)
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

        private static List<Filter> ParseFiltersJson(string? filtersJson)
        {
            if (string.IsNullOrWhiteSpace(filtersJson))
                return new List<Filter>();

            try
            {
                var parsed = JsonSerializer.Deserialize<List<Filter>>(filtersJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return parsed ?? new List<Filter>();
            }
            catch
            {
                return new List<Filter>();
            }
        }

        private static double CalculateDistanceKm(double lat1, double lon1, double lat2, double lon2)
        {
            const double radiusOfEarthKm = 6371.0;
            var dLat = DegreesToRadians(lat2 - lat1);
            var dLon = DegreesToRadians(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return radiusOfEarthKm * c;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180.0);
        }

        private static string BuildAddressDisplay(string? address, string? district, string? city, string? country)
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(address)) parts.Add(address.Trim());
            if (!string.IsNullOrWhiteSpace(district)) parts.Add(district.Trim());
            if (!string.IsNullOrWhiteSpace(city)) parts.Add(city.Trim());
            if (!string.IsNullOrWhiteSpace(country)) parts.Add(country.Trim());
            return parts.Count == 0 ? "-" : string.Join(", ", parts.Distinct(StringComparer.OrdinalIgnoreCase));
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
