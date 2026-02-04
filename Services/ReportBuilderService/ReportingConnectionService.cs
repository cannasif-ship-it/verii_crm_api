using crm_api.DTOs;
using crm_api.DTOs.ReportBuilderDto;
using crm_api.Interfaces;
using Microsoft.Extensions.Configuration;

namespace crm_api.Services.ReportBuilderService
{
    public class ReportingConnectionService : IReportingConnectionService
    {
        private readonly IConfiguration _configuration;
        private readonly ILocalizationService _localizationService;

        private static readonly Dictionary<string, string> KeyToConnectionName = new(StringComparer.OrdinalIgnoreCase)
        {
            { "CRM", "DefaultConnection" },
            { "ERP", "ErpConnection" }
        };

        public ReportingConnectionService(IConfiguration configuration, ILocalizationService localizationService)
        {
            _configuration = configuration;
            _localizationService = localizationService;
        }

        public ApiResponse<List<ConnectionDto>> GetConnections()
        {
            var list = new List<ConnectionDto>
            {
                new ConnectionDto { Key = "CRM", Name = "CRM" },
                new ConnectionDto { Key = "ERP", Name = "ERP" }
            };
            return ApiResponse<List<ConnectionDto>>.SuccessResult(list, _localizationService.GetLocalizedString("ReportingConnectionService.ConnectionsRetrieved"));
        }

        public ApiResponse<string> ResolveConnectionString(string connectionKey)
        {
            if (string.IsNullOrWhiteSpace(connectionKey))
                return ApiResponse<string>.ErrorResult(_localizationService.GetLocalizedString("ReportingConnectionService.ConnectionKeyRequired"), null, 400);

            if (!KeyToConnectionName.TryGetValue(connectionKey.Trim(), out var connName))
                return ApiResponse<string>.ErrorResult(_localizationService.GetLocalizedString("ReportingConnectionService.InvalidConnectionKey"), null, 400);

            var connStr = _configuration.GetConnectionString(connName);
            if (string.IsNullOrWhiteSpace(connStr))
                return ApiResponse<string>.ErrorResult(_localizationService.GetLocalizedString("ReportingConnectionService.ConnectionStringNotFound", connName), null, 400);

            return ApiResponse<string>.SuccessResult(connStr, _localizationService.GetLocalizedString("ReportingConnectionService.Ok"));
        }
    }
}
