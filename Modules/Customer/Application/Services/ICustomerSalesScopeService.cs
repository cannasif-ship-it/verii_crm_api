using CustomerEntity = crm_api.Modules.Customer.Domain.Entities.Customer;

namespace crm_api.Modules.Customer.Application.Services
{
    public interface ICustomerSalesScopeService
    {
        Task<IQueryable<CustomerEntity>> ApplyScopeAsync(IQueryable<CustomerEntity> query, long? contextUserId = null);
    }
}
