using AutoMapper;
using crm_api.DTOs;
using crm_api.Interfaces;
using crm_api.Models;
using crm_api.UnitOfWork;
using crm_api.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace crm_api.Services
{
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public ContactService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<ContactDto>>> GetAllContactsAsync(PagedRequest request)
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
                    { "titleName", "Title.TitleName" },
                    { "customerName", "Customer.CustomerName" }
                };

                var query = _unitOfWork.Contacts
                    .Query()
                    .Where(c => !c.IsDeleted)
                    .Include(c => c.Customer)
                    .Include(c => c.Title)
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.UpdatedByUser)
                    .Include(c => c.DeletedByUser)
                    .ApplyFilters(request.Filters, request.FilterLogic, columnMapping);

                var sortBy = request.SortBy ?? nameof(Contact.Id);

                query = query.ApplySorting(sortBy, request.SortDirection, columnMapping);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<ContactDto>(x)).ToList();

                var pagedResponse = new PagedResponse<ContactDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<ContactDto>>.SuccessResult(pagedResponse, _localizationService.GetLocalizedString("ContactService.ContactsRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<ContactDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("ContactService.InternalServerError"),
                    _localizationService.GetLocalizedString("ContactService.GetAllContactsExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ContactDto>> GetContactByIdAsync(long id)
        {
            try
            {
                var contact = await _unitOfWork.Contacts.GetByIdAsync(id);
                if (contact == null)
                {
                    return ApiResponse<ContactDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ContactService.ContactNotFound"),
                        _localizationService.GetLocalizedString("ContactService.ContactNotFound"),
                        StatusCodes.Status404NotFound);
                }

                // Reload with navigation properties for mapping
                var contactWithNav = await _unitOfWork.Contacts
                    .Query()
                    .Include(c => c.Customer)
                    .Include(c => c.Title)
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.UpdatedByUser)
                    .Include(c => c.DeletedByUser)
                    .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

                var contactDto = _mapper.Map<ContactDto>(contactWithNav ?? contact);
                return ApiResponse<ContactDto>.SuccessResult(contactDto, _localizationService.GetLocalizedString("ContactService.ContactRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ContactDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ContactService.InternalServerError"),
                    _localizationService.GetLocalizedString("ContactService.GetContactByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ContactDto>> CreateContactAsync(CreateContactDto createContactDto)
        {
            try
            {
                NormalizeContactDto(createContactDto);
                var governanceError = await ValidateContactGovernanceAsync(createContactDto, null);
                if (governanceError != null)
                {
                    return governanceError;
                }

                var contact = _mapper.Map<Contact>(createContactDto);
                await _unitOfWork.Contacts.AddAsync(contact);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var contactWithNav = await _unitOfWork.Contacts
                    .Query()
                    .Include(c => c.Customer)
                    .Include(c => c.Title)
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.UpdatedByUser)
                    .Include(c => c.DeletedByUser)
                    .FirstOrDefaultAsync(c => c.Id == contact.Id && !c.IsDeleted);

                if (contactWithNav == null)
                {
                    return ApiResponse<ContactDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ContactService.ContactNotFound"),
                        _localizationService.GetLocalizedString("ContactService.ContactNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var contactDto = _mapper.Map<ContactDto>(contactWithNav);

                return ApiResponse<ContactDto>.SuccessResult(contactDto, _localizationService.GetLocalizedString("ContactService.ContactCreated"));
            }
            catch (DbUpdateException ex) when (DbUpdateExceptionHelper.TryGetUniqueViolation(ex, out _))
            {
                return ApiResponse<ContactDto>.ErrorResult(
                    _localizationService.GetLocalizedString("General.RecordAlreadyExists"),
                    _localizationService.GetLocalizedString("General.RecordAlreadyExists"),
                    StatusCodes.Status409Conflict);
            }
            catch (Exception ex)
            {
                return ApiResponse<ContactDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ContactService.InternalServerError"),
                    _localizationService.GetLocalizedString("ContactService.CreateContactExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ContactDto>> UpdateContactAsync(long id, UpdateContactDto updateContactDto)
        {
            try
            {
                NormalizeContactDto(updateContactDto);
                var governanceError = await ValidateContactGovernanceAsync(updateContactDto, id);
                if (governanceError != null)
                {
                    return governanceError;
                }

                // Get tracked entity for update
                var contact = await _unitOfWork.Contacts.GetByIdForUpdateAsync(id);
                if (contact == null)
                {
                    return ApiResponse<ContactDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ContactService.ContactNotFound"),
                        _localizationService.GetLocalizedString("ContactService.ContactNotFound"),
                        StatusCodes.Status404NotFound);
                }

                _mapper.Map(updateContactDto, contact);
                await _unitOfWork.Contacts.UpdateAsync(contact);
                await _unitOfWork.SaveChangesAsync();

                // Reload with navigation properties for mapping (read-only)
                var contactWithNav = await _unitOfWork.Contacts
                    .Query()
                    .Include(c => c.Customer)
                    .Include(c => c.Title)
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.UpdatedByUser)
                    .Include(c => c.DeletedByUser)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (contactWithNav == null)
                {
                    return ApiResponse<ContactDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ContactService.ContactNotFound"),
                        _localizationService.GetLocalizedString("ContactService.ContactNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var contactDto = _mapper.Map<ContactDto>(contactWithNav);

                return ApiResponse<ContactDto>.SuccessResult(contactDto, _localizationService.GetLocalizedString("ContactService.ContactUpdated"));
            }
            catch (DbUpdateException ex) when (DbUpdateExceptionHelper.TryGetUniqueViolation(ex, out _))
            {
                return ApiResponse<ContactDto>.ErrorResult(
                    _localizationService.GetLocalizedString("General.RecordAlreadyExists"),
                    _localizationService.GetLocalizedString("General.RecordAlreadyExists"),
                    StatusCodes.Status409Conflict);
            }
            catch (Exception ex)
            {
                return ApiResponse<ContactDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ContactService.InternalServerError"),
                    _localizationService.GetLocalizedString("ContactService.UpdateContactExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteContactAsync(long id)
        {
            try
            {
                var deleted = await _unitOfWork.Contacts.SoftDeleteAsync(id);
                if (!deleted)
                {
                    return ApiResponse<object>.ErrorResult(
                        _localizationService.GetLocalizedString("ContactService.ContactNotFound"),
                        _localizationService.GetLocalizedString("ContactService.ContactNotFound"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(null, _localizationService.GetLocalizedString("ContactService.ContactDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _localizationService.GetLocalizedString("ContactService.InternalServerError"),
                    _localizationService.GetLocalizedString("ContactService.DeleteContactExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        private async Task<ApiResponse<ContactDto>?> ValidateContactGovernanceAsync(CreateContactDto dto, long? excludedId)
        {
            NormalizeContactDto(dto);

            if (string.IsNullOrWhiteSpace(dto.FullName))
            {
                return ApiResponse<ContactDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ContactService.FullNameRequired"),
                    _localizationService.GetLocalizedString("ContactService.FullNameRequired"),
                    StatusCodes.Status400BadRequest);
            }

            if (!IsValidEmail(dto.Email))
            {
                return ApiResponse<ContactDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ContactService.InvalidEmail"),
                    _localizationService.GetLocalizedString("ContactService.InvalidEmail"),
                    StatusCodes.Status400BadRequest);
            }

            var normalizedEmail = NormalizeText(dto.Email);
            var normalizedPhone = NormalizeDigits(dto.Phone);
            var normalizedMobile = NormalizeDigits(dto.Mobile);
            var normalizedName = NormalizeText(dto.FullName);

            var duplicateQuery = _unitOfWork.Contacts
                .Query()
                .Where(c => !c.IsDeleted && c.CustomerId == dto.CustomerId);

            if (excludedId.HasValue)
            {
                duplicateQuery = duplicateQuery.Where(c => c.Id != excludedId.Value);
            }

            var duplicateCandidates = await duplicateQuery
                .Where(c =>
                    (!string.IsNullOrWhiteSpace(normalizedEmail) && !string.IsNullOrWhiteSpace(c.Email)) ||
                    ((!string.IsNullOrWhiteSpace(normalizedMobile) || !string.IsNullOrWhiteSpace(normalizedPhone)) &&
                     !string.IsNullOrWhiteSpace(c.FullName)))
                .Select(c => new
                {
                    c.Email,
                    c.FullName,
                    c.Mobile,
                    c.Phone
                })
                .ToListAsync();

            var hasDuplicate = duplicateCandidates.Any(c =>
                (!string.IsNullOrWhiteSpace(normalizedEmail) && NormalizeText(c.Email) == normalizedEmail) ||
                ((!string.IsNullOrWhiteSpace(normalizedMobile) || !string.IsNullOrWhiteSpace(normalizedPhone)) &&
                 NormalizeText(c.FullName) == normalizedName &&
                 (NormalizeDigits(c.Mobile) == normalizedMobile || NormalizeDigits(c.Phone) == normalizedPhone)));

            if (hasDuplicate)
            {
                return ApiResponse<ContactDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ContactService.DuplicateContact"),
                    _localizationService.GetLocalizedString("ContactService.DuplicateContact"),
                    StatusCodes.Status409Conflict);
            }

            return null;
        }

        private async Task<ApiResponse<ContactDto>?> ValidateContactGovernanceAsync(UpdateContactDto dto, long excludedId)
        {
            NormalizeContactDto(dto);

            var createLike = new CreateContactDto
            {
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Mobile = dto.Mobile,
                Notes = dto.Notes,
                CustomerId = dto.CustomerId,
                TitleId = dto.TitleId
            };

            return await ValidateContactGovernanceAsync(createLike, excludedId);
        }

        private static void NormalizeContactDto(CreateContactDto dto)
        {
            dto.FirstName = (dto.FirstName ?? string.Empty).Trim();
            dto.MiddleName = NormalizeNullable(dto.MiddleName);
            dto.LastName = (dto.LastName ?? string.Empty).Trim();
            dto.FullName = NormalizeNullable(dto.FullName) ?? BuildFullName(dto.FirstName, dto.MiddleName, dto.LastName);
            dto.Email = NormalizeNullable(dto.Email);
            dto.Phone = NormalizeNullable(dto.Phone);
            dto.Mobile = NormalizeNullable(dto.Mobile);
            dto.Notes = NormalizeNullable(dto.Notes);
        }

        private static void NormalizeContactDto(UpdateContactDto dto)
        {
            dto.FirstName = (dto.FirstName ?? string.Empty).Trim();
            dto.MiddleName = NormalizeNullable(dto.MiddleName);
            dto.LastName = (dto.LastName ?? string.Empty).Trim();
            dto.FullName = NormalizeNullable(dto.FullName) ?? BuildFullName(dto.FirstName, dto.MiddleName, dto.LastName);
            dto.Email = NormalizeNullable(dto.Email);
            dto.Phone = NormalizeNullable(dto.Phone);
            dto.Mobile = NormalizeNullable(dto.Mobile);
            dto.Notes = NormalizeNullable(dto.Notes);
        }

        private static string? NormalizeNullable(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static string? BuildFullName(string? firstName, string? middleName, string? lastName)
        {
            var fullName = string.Join(" ", new[] { firstName, middleName, lastName }
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!.Trim()))
                .Trim();

            return string.IsNullOrWhiteSpace(fullName) ? null : fullName;
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
