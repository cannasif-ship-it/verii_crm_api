using CustomerEntity = crm_api.Modules.Customer.Domain.Entities.Customer;

namespace crm_api.Modules.Customer.Application.Services
{
    public interface ICustomerSalesScopeService
    {
        Task<IQueryable<CustomerEntity>> ApplyScopeAsync(IQueryable<CustomerEntity> query, long? contextUserId = null);
        Task<CustomerSalesRepCodeResolutionResult> ResolveCustomerSalesRepCodeAsync(string? requestedSalesRepCode, long? contextUserId = null);
    }

    public sealed class CustomerSalesRepCodeResolutionResult
    {
        public bool RestrictionEnabled { get; init; }
        public bool Success { get; init; }
        public string? SalesRepCode { get; init; }
        public string? ErrorCode { get; init; }
    }
}
