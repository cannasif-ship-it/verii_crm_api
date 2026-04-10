using crm_api.Modules.ReportBuilder.Application.Dtos;

namespace crm_api.Modules.ReportBuilder.Application.Services
{
    public interface IReportingCatalogService
    {
        Task<ApiResponse<List<FieldSchemaDto>>> CheckAndGetSchemaAsync(string connectionKey, string type, string name);
        Task<ApiResponse<List<DataSourceCatalogItemDto>>> ListDataSourcesAsync(string connectionKey, string type, string? search);
        Task<ApiResponse<List<DataSourceParameterDto>>> GetParametersAsync(string connectionKey, string type, string name);
    }
}
