using crm_api.DTOs;
using crm_api.DTOs.ReportBuilderDto;

namespace crm_api.Interfaces
{
    public interface IReportingConnectionService
    {
        ApiResponse<List<ConnectionDto>> GetConnections();
        ApiResponse<string> ResolveConnectionString(string connectionKey);
    }
}
