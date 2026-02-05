using Hangfire;
using Infrastructure.BackgroundJobs.Interfaces;
using crm_api.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundJobs
{
    [DisableConcurrentExecution(timeoutInSeconds: 300)]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 120, 300 })]
    public class StockSyncJob : IStockSyncJob
    {
        private readonly IStockService _stockService;
        private readonly ICustomerService _customerService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger<StockSyncJob> _logger;

        public StockSyncJob(
            IStockService stockService,
            ICustomerService customerService,
            ILocalizationService localizationService,
            ILogger<StockSyncJob> logger)
        {
            _stockService = stockService;
            _customerService = customerService;
            _localizationService = localizationService;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                _logger.LogInformation(_localizationService.GetLocalizedString("StockSyncJob.Started"));
                
                await _stockService.SyncStocksFromErpAsync();

                _logger.LogInformation(_localizationService.GetLocalizedString("CustomerSyncJob.Started"));
                await _customerService.SyncCustomersFromErpAsync();
                _logger.LogInformation(_localizationService.GetLocalizedString("CustomerSyncJob.Completed"));

                _logger.LogInformation(_localizationService.GetLocalizedString("StockSyncJob.Completed"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _localizationService.GetLocalizedString("StockSyncJob.Failed"));
                throw;
            }
        }
    }
}
