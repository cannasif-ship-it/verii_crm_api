using crm_api.DTOs;
using crm_api.DTOs.ReportBuilderDto;

namespace crm_api.Interfaces
{
    public interface IReportingCatalogService
    {
        Task<ApiResponse<List<FieldSchemaDto>>> CheckAndGetSchemaAsync(string connectionKey, string type, string name);
    }
}
