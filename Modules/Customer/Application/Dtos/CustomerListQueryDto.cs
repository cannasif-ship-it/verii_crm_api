using crm_api.Shared.Common.Application;

namespace crm_api.Modules.Customer.Application.Dtos
{
    public class CustomerListQueryDto : PagedRequest
    {
        public long? ContextUserId { get; set; }
    }
}
