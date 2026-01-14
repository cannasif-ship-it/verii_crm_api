using Microsoft.AspNetCore.Localization;

namespace cms_webapi.Services
{
    public class CustomHeaderRequestCultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue("x-language", out var headerValue))
            {
                var language = headerValue.ToString().ToLowerInvariant();
                
                return Task.FromResult<ProviderCultureResult?>(language switch
                {
                    "tr" or "tr-tr" => new ProviderCultureResult("tr-TR"),
                    "en" or "en-us" => new ProviderCultureResult("en-US"),
                    _ => null
                });
            }

            return Task.FromResult<ProviderCultureResult?>(null);
        }
    }
}
