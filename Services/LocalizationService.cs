using Microsoft.Extensions.Localization;
using cms_webapi.Interfaces;
using System.Globalization;
using System.Resources;
using System.Reflection;
using cms_webapi.UnitOfWork;

namespace cms_webapi.Services
{
    public class LocalizationService : ILocalizationService
    {
        private readonly ILogger<LocalizationService> _logger;
        private readonly ResourceManager _resourceManager;

        public LocalizationService(ILogger<LocalizationService> logger)
        {
            _logger = logger;
            
            // Get the current assembly
            var assembly = Assembly.GetExecutingAssembly();
            
            // Create ResourceManager for Messages resources
            _resourceManager = new ResourceManager("cms_webapi.Resources.Messages", assembly);
            
            _logger.LogInformation($"LocalizationService initialized with ResourceManager for cms_webapi.Resources.Messages");
        }

        public string GetLocalizedString(string key)
        {
            var currentCulture = CultureInfo.CurrentCulture;
            var currentUICulture = CultureInfo.CurrentUICulture;
            
            _logger.LogInformation($"LocalizationService - Key: {key}, CurrentCulture: {currentCulture.Name}, CurrentUICulture: {currentUICulture.Name}");
            
            try
            {
                var localizedString = _resourceManager.GetString(key, currentUICulture);
                
                if (string.IsNullOrEmpty(localizedString))
                {
                    _logger.LogWarning($"LocalizationService - Resource not found for key: {key}, culture: {currentUICulture.Name}");
                    return key;
                }
                
                _logger.LogInformation($"LocalizationService - Found localized string: {localizedString}");
                return localizedString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"LocalizationService - Error getting localized string for key: {key}");
                return key;
            }
        }

        public string GetLocalizedString(string key, params object[] arguments)
        {
            var localizedString = GetLocalizedString(key);
            
            try
            {
                return string.Format(localizedString, arguments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"LocalizationService - Error formatting localized string for key: {key}");
                return localizedString;
            }
        }
    }
}
