using crm_api.Modules.System.Domain.Entities;
using crm_api.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace crm_api.Infrastructure.Startup
{
    public class SystemSettingsBootstrapHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SystemSettingsBootstrapHostedService> _logger;

        public SystemSettingsBootstrapHostedService(
            IServiceProvider serviceProvider,
            ILogger<SystemSettingsBootstrapHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var hasSystemSettings = await unitOfWork.SystemSettings
                .Query()
                .AsNoTracking()
                .AnyAsync(x => !x.IsDeleted, cancellationToken)
                .ConfigureAwait(false);

            if (hasSystemSettings)
            {
                return;
            }

            await unitOfWork.SystemSettings.AddAsync(new SystemSetting
            {
                NumberFormat = "tr-TR",
                DecimalPlaces = 2,
                RestrictCustomersBySalesRepMatch = false,
                IsDeleted = false,
                CreatedDate = DateTimeProvider.Now,
                CreatedBy = null,
                UpdatedBy = null,
                DeletedBy = null,
            }).ConfigureAwait(false);

            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            _logger.LogInformation("Default system settings row created because none existed.");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
