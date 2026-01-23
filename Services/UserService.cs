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
using System.Collections.Generic;
using System.Security.Claims;

namespace crm_api.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _loc;
        private readonly CmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IUnitOfWork uow, IMapper mapper, ILocalizationService loc, CmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow; _mapper = mapper; _loc = loc; _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<long>> GetCurrentUserIdAsync()
        {
            try
            {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return ApiResponse<long>.ErrorResult(
                    _loc.GetLocalizedString("UserService.InvalidUserId"),
                    _loc.GetLocalizedString("UserService.InvalidUserId"),
                    StatusCodes.Status400BadRequest);
                }
                return ApiResponse<long>.SuccessResult(userId, _loc.GetLocalizedString("UserService.UserIdRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<long>.ErrorResult(
                    _loc.GetLocalizedString("UserService.InternalServerError"),
                    _loc.GetLocalizedString("UserService.GetCurrentUserIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<PagedResponse<UserDto>>> GetAllUsersAsync(PagedRequest request)
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

                var query = _context.Users
                    .AsNoTracking()
                    .Where(u => !u.IsDeleted)
                    .Include(u => u.CreatedByUser)
                    .Include(u => u.UpdatedByUser)
                    .Include(u => u.DeletedByUser)
                    .ApplyFilters(request.Filters);

                var sortBy = request.SortBy ?? nameof(User.Id);
                var isDesc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                query = query.ApplySorting(sortBy, request.SortDirection);

                var totalCount = await query.CountAsync();

                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = items.Select(x => _mapper.Map<UserDto>(x)).ToList();

                var pagedResponse = new PagedResponse<UserDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return ApiResponse<PagedResponse<UserDto>>.SuccessResult(pagedResponse, _loc.GetLocalizedString("UserService.UsersRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<UserDto>>.ErrorResult(
                    _loc.GetLocalizedString("UserService.InternalServerError"),
                    _loc.GetLocalizedString("UserService.GetAllUsersExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<UserDto>> GetUserByIdAsync(long id)
        {
            try
            {
                var user = await _uow.Users.GetByIdAsync(id);
                if (user == null) return ApiResponse<UserDto>.ErrorResult(
                    _loc.GetLocalizedString("UserService.UserNotFound"),
                    _loc.GetLocalizedString("UserService.UserNotFound"),
                    StatusCodes.Status404NotFound);

                // Reload with navigation properties for mapping
                var userWithNav = await _context.Users
                    .AsNoTracking()
                    .Include(u => u.CreatedByUser)
                    .Include(u => u.UpdatedByUser)
                    .Include(u => u.DeletedByUser)
                    .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

                var dto = _mapper.Map<UserDto>(userWithNav ?? user);
                return ApiResponse<UserDto>.SuccessResult(dto, _loc.GetLocalizedString("UserService.UserRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.ErrorResult(
                    _loc.GetLocalizedString("UserService.InternalServerError"),
                    _loc.GetLocalizedString("UserService.GetUserByIdExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserDto dto)
        {
            try
            {
                var entity = _mapper.Map<User>(dto);
                entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                entity.CreatedDate = DateTime.UtcNow;
                await _uow.Users.AddAsync(entity);
                await _uow.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var userWithNav = await _context.Users
                    .AsNoTracking()
                    .Include(u => u.CreatedByUser)
                    .Include(u => u.UpdatedByUser)
                    .Include(u => u.DeletedByUser)
                    .FirstOrDefaultAsync(u => u.Id == entity.Id && !u.IsDeleted);

                var outDto = _mapper.Map<UserDto>(userWithNav ?? entity);
                return ApiResponse<UserDto>.SuccessResult(outDto, _loc.GetLocalizedString("UserService.UserCreated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.ErrorResult(
                    _loc.GetLocalizedString("UserService.InternalServerError"),
                    _loc.GetLocalizedString("UserService.CreateUserExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<UserDto>> UpdateUserAsync(long id, UpdateUserDto dto)
        {
            try
            {
                var entity = await _uow.Users.GetByIdAsync(id);
                if (entity == null) return ApiResponse<UserDto>.ErrorResult(
                    _loc.GetLocalizedString("UserService.UserNotFound"),
                    null,
                    StatusCodes.Status404NotFound);
                _mapper.Map(dto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                await _uow.Users.UpdateAsync(entity);
                await _uow.SaveChangesAsync();

                // Reload with navigation properties for mapping
                var userWithNav = await _context.Users
                    .AsNoTracking()
                    .Include(u => u.CreatedByUser)
                    .Include(u => u.UpdatedByUser)
                    .Include(u => u.DeletedByUser)
                    .FirstOrDefaultAsync(u => u.Id == entity.Id && !u.IsDeleted);

                var outDto = _mapper.Map<UserDto>(userWithNav ?? entity);
                return ApiResponse<UserDto>.SuccessResult(outDto, _loc.GetLocalizedString("UserService.UserUpdated"));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.ErrorResult(
                    _loc.GetLocalizedString("UserService.InternalServerError"),
                    _loc.GetLocalizedString("UserService.UpdateUserExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<object>> DeleteUserAsync(long id)
        {
            try
            {
                var entity = await _uow.Users.GetByIdAsync(id);
                if (entity == null) return ApiResponse<object>.ErrorResult(
                    _loc.GetLocalizedString("UserService.UserNotFound"),
                    _loc.GetLocalizedString("UserService.UserNotFound"),
                    StatusCodes.Status404NotFound);
                await _uow.Users.SoftDeleteAsync(id);
                await _uow.SaveChangesAsync();
                return ApiResponse<object>.SuccessResult(null, _loc.GetLocalizedString("UserService.UserDeleted"));
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult(
                    _loc.GetLocalizedString("UserService.InternalServerError"),
                    _loc.GetLocalizedString("UserService.DeleteUserExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<UserDto>> GetUserProfileAsync(string userId)
        {
            try
            {
                if (!long.TryParse(userId, out var userIdLong))
                {
                    return ApiResponse<UserDto>.ErrorResult(
                        _loc.GetLocalizedString("UserService.InvalidUserId"),
                        null,
                        StatusCodes.Status400BadRequest);
                }

                var user = await _uow.Users.GetByIdAsync(userIdLong);
                if (user == null)
                {
                    return ApiResponse<UserDto>.ErrorResult(
                        _loc.GetLocalizedString("UserService.UserNotFound"),
                        _loc.GetLocalizedString("UserService.UserNotFound"),
                        StatusCodes.Status404NotFound);
                }

                var dto = _mapper.Map<UserDto>(user);
                return ApiResponse<UserDto>.SuccessResult(dto, _loc.GetLocalizedString("UserService.UserRetrieved"));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.ErrorResult(
                    _loc.GetLocalizedString("UserService.InternalServerError"),
                    _loc.GetLocalizedString("UserService.GetUserProfileExceptionMessage", ex.Message),
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
