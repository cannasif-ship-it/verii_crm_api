using System.Globalization;
using crm_api.Shared.Infrastructure.Abstractions;
using Microsoft.AspNetCore.Http;

namespace crm_api.Shared.Infrastructure.Services.Localization;

public sealed class PragmaticLocalizationService : ILocalizationService
{
    private readonly IReadOnlyDictionary<string, Dictionary<string, string>> _messages;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PragmaticLocalizationService(
        LocalizationRegistry registry,
        IHttpContextAccessor httpContextAccessor)
    {
        _messages = registry.Messages;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetLocalizedString(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return string.Empty;
        }

        var culture = ResolveCulture();

        if (TryGetRegistryValue(culture, key, out var value))
        {
            return value;
        }

        if (TryGetRegistryValue("en", key, out var englishFallback))
        {
            return englishFallback;
        }

        return key;
    }

    public string GetLocalizedString(string key, params object[] arguments)
    {
        var localizedString = GetLocalizedString(key);

        try
        {
            return arguments == null || arguments.Length == 0
                ? localizedString
                : string.Format(localizedString, arguments);
        }
        catch
        {
            return localizedString;
        }
    }

    private bool TryGetRegistryValue(string culture, string key, out string value)
    {
        value = string.Empty;

        if (!_messages.TryGetValue(culture, out var cultureMessages))
        {
            return false;
        }

        if (!cultureMessages.TryGetValue(key, out var localizedValue) || string.IsNullOrWhiteSpace(localizedValue))
        {
            return false;
        }

        value = localizedValue;
        return true;
    }
    private string ResolveCulture()
    {
        var context = _httpContextAccessor.HttpContext;
        var requestedCulture =
            context?.Request.Headers["X-Language"].FirstOrDefault()
            ?? context?.Request.Headers.AcceptLanguage.FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(requestedCulture))
        {
            return NormalizeCulture(requestedCulture);
        }

        return NormalizeCulture(CultureInfo.CurrentUICulture.Name);
    }

    private static string NormalizeCulture(string value)
    {
        var normalized = value.Split(new[] { ',', ';', '-', '_' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .FirstOrDefault();

        return string.IsNullOrWhiteSpace(normalized) ? "tr" : normalized.ToLowerInvariant();
    }
}
