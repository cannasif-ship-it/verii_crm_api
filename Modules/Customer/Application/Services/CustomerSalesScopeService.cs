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
            var isRestrictionEnabled = await _unitOfWork.SystemSettings
                .Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Id)
                .Select(x => (bool?)x.RestrictCustomersBySalesRepMatch)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

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
