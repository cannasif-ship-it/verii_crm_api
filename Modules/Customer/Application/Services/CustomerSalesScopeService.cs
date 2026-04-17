using crm_api.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CustomerEntity = crm_api.Modules.Customer.Domain.Entities.Customer;

namespace crm_api.Modules.Customer.Application.Services
{
    public class CustomerSalesScopeService : ICustomerSalesScopeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerSalesScopeService(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IQueryable<CustomerEntity>> ApplyScopeAsync(IQueryable<CustomerEntity> query, long? contextUserId = null)
        {
            var isRestrictionEnabled = await IsRestrictionEnabledAsync().ConfigureAwait(false);

            if (isRestrictionEnabled != true)
            {
                return query;
            }

            var effectiveUserId = contextUserId ?? GetCurrentUserId();
            if (effectiveUserId == null || effectiveUserId <= 0)
            {
                return query.Where(_ => false);
            }

            var branchCode = GetCurrentBranchCode();
            var matchesQuery = _unitOfWork.SalesRepCodeUserMatches
                .Query()
                .Where(x => !x.IsDeleted && x.UserId == effectiveUserId.Value)
                .Include(x => x.SalesRepCode)
                .AsQueryable();

            if (branchCode.HasValue && branchCode.Value > 0)
            {
                matchesQuery = matchesQuery.Where(x => x.SalesRepCode.BranchCode == branchCode.Value);
            }

            var allowedSalesRepCodes = (await matchesQuery
                    .Select(x => x.SalesRepCode.SalesRepCodeValue)
                    .ToListAsync()
                    .ConfigureAwait(false))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (allowedSalesRepCodes.Count == 0)
            {
                return query.Where(_ => false);
            }

            return query.Where(x =>
                x.SalesRepCode != null &&
                allowedSalesRepCodes.Contains(x.SalesRepCode));
        }

        public async Task<CustomerSalesRepCodeResolutionResult> ResolveCustomerSalesRepCodeAsync(string? requestedSalesRepCode, long? contextUserId = null)
        {
            var isRestrictionEnabled = await IsRestrictionEnabledAsync().ConfigureAwait(false);
            var normalizedRequestedSalesRepCode = NormalizeCode(requestedSalesRepCode);

            if (isRestrictionEnabled != true)
            {
                return new CustomerSalesRepCodeResolutionResult
                {
                    RestrictionEnabled = false,
                    Success = true,
                    SalesRepCode = normalizedRequestedSalesRepCode
                };
            }

            var effectiveUserId = contextUserId ?? GetCurrentUserId();
            if (effectiveUserId == null || effectiveUserId <= 0)
            {
                return new CustomerSalesRepCodeResolutionResult
                {
                    RestrictionEnabled = true,
                    Success = false,
                    ErrorCode = "missing_user"
                };
            }

            var allowedSalesRepCodes = await GetAllowedSalesRepCodesAsync(effectiveUserId.Value).ConfigureAwait(false);
            if (allowedSalesRepCodes.Count == 0)
            {
                return new CustomerSalesRepCodeResolutionResult
                {
                    RestrictionEnabled = true,
                    Success = false,
                    ErrorCode = "no_match"
                };
            }

            if (!string.IsNullOrWhiteSpace(normalizedRequestedSalesRepCode))
            {
                var matchedCode = allowedSalesRepCodes.FirstOrDefault(x => string.Equals(x, normalizedRequestedSalesRepCode, StringComparison.OrdinalIgnoreCase));
                if (matchedCode == null)
                {
                    return new CustomerSalesRepCodeResolutionResult
                    {
                        RestrictionEnabled = true,
                        Success = false,
                        ErrorCode = "not_allowed"
                    };
                }

                return new CustomerSalesRepCodeResolutionResult
                {
                    RestrictionEnabled = true,
                    Success = true,
                    SalesRepCode = matchedCode
                };
            }

            if (allowedSalesRepCodes.Count == 1)
            {
                return new CustomerSalesRepCodeResolutionResult
                {
                    RestrictionEnabled = true,
                    Success = true,
                    SalesRepCode = allowedSalesRepCodes[0]
                };
            }

            return new CustomerSalesRepCodeResolutionResult
            {
                RestrictionEnabled = true,
                Success = false,
                ErrorCode = "multiple_matches"
            };
        }

        private async Task<bool?> IsRestrictionEnabledAsync()
        {
            return await _unitOfWork.SystemSettings
                .Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Id)
                .Select(x => (bool?)x.RestrictCustomersBySalesRepMatch)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
        }

        private async Task<List<string>> GetAllowedSalesRepCodesAsync(long userId)
        {
            var branchCode = GetCurrentBranchCode();
            var matchesQuery = _unitOfWork.SalesRepCodeUserMatches
                .Query()
                .Where(x => !x.IsDeleted && x.UserId == userId)
                .Include(x => x.SalesRepCode)
                .AsQueryable();

            if (branchCode.HasValue && branchCode.Value > 0)
            {
                matchesQuery = matchesQuery.Where(x => x.SalesRepCode.BranchCode == branchCode.Value);
            }

            return (await matchesQuery
                    .Select(x => x.SalesRepCode.SalesRepCodeValue)
                    .ToListAsync()
                    .ConfigureAwait(false))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private static string? NormalizeCode(string? salesRepCode)
        {
            return string.IsNullOrWhiteSpace(salesRepCode) ? null : salesRepCode.Trim();
        }

        private long? GetCurrentUserId()
        {
            var rawUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return long.TryParse(rawUserId, out var userId) ? userId : null;
        }

        private short? GetCurrentBranchCode()
        {
            var rawBranchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"]?.ToString();
            return short.TryParse(rawBranchCode, out var branchCode) ? branchCode : null;
        }
    }
}
