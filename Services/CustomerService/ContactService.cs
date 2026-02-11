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

                var query = _unitOfWork.Contacts
                    .Query()
                    .Where(c => !c.IsDeleted)
                    .Include(c => c.Customer)
                    .Include(c => c.Title)
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.UpdatedByUser)
                    .Include(c => c.DeletedByUser)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(Contact.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

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

            var hasDuplicate = await duplicateQuery.AnyAsync(c =>
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
            var createLike = new CreateContactDto
            {
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
