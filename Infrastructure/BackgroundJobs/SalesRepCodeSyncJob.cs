using Hangfire;
using Infrastructure.BackgroundJobs.Interfaces;
using crm_api.Modules.Definitions.Application.Services;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundJobs
{
    [DisableConcurrentExecution(timeoutInSeconds: 300)]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 120, 300 })]
    public class SalesRepCodeSyncJob : ISalesRepCodeSyncJob
    {
        private readonly ISalesRepCodeService _salesRepCodeService;
        private readonly ILogger<SalesRepCodeSyncJob> _logger;

        public SalesRepCodeSyncJob(
            ISalesRepCodeService salesRepCodeService,
            ILogger<SalesRepCodeSyncJob> logger)
        {
            _salesRepCodeService = salesRepCodeService;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Sales rep code sync job started.");
            var result = await _salesRepCodeService.SyncSalesRepCodesFromErpAsync().ConfigureAwait(false);

            if (!result.Success)
            {
                _logger.LogWarning(
                    "Sales rep code sync job failed. Message: {Message} Exception: {Exception}",
                    result.Message,
                    result.ExceptionMessage);
                return;
            }

            _logger.LogInformation(
                "Sales rep code sync job completed. Created={Created}, Updated={Updated}, Deactivated={Deactivated}",
                result.Data?.CreatedCount ?? 0,
                result.Data?.UpdatedCount ?? 0,
                result.Data?.DeactivatedCount ?? 0);
        }
    }
}
