using Microsoft.AspNetCore.Localization;

namespace crm_api.Services
{
    /// <summary>
    /// Custom culture provider that reads language from "x-language" header
    /// Supports: tr, en, de, fr, es, it
    /// </summary>
    public class CustomHeaderRequestCultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue("x-language", out var headerValue))
            {
                var language = headerValue.ToString().ToLowerInvariant().Trim();
                
                return Task.FromResult<ProviderCultureResult?>(language switch
                {
                    "tr" or "tr-tr" => new ProviderCultureResult("tr-TR"),
                    "en" or "en-us" => new ProviderCultureResult("en-US"),
                    "de" or "de-de" => new ProviderCultureResult("de-DE"),
                    "fr" or "fr-fr" => new ProviderCultureResult("fr-FR"),
                    "es" or "es-es" => new ProviderCultureResult("es-ES"),
                    "it" or "it-it" => new ProviderCultureResult("it-IT"),
                    _ => null // Return null to fall back to default culture
                });
            }

            return Task.FromResult<ProviderCultureResult?>(null);
        }
    }
}
