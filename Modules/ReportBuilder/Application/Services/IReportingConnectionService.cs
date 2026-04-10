using crm_api.Modules.ReportBuilder.Application.Dtos;

namespace crm_api.Modules.ReportBuilder.Application.Services
{
    public interface IReportingConnectionService
    {
        ApiResponse<List<ConnectionDto>> GetConnections();
        ApiResponse<string> ResolveConnectionString(string connectionKey);
    }
}
