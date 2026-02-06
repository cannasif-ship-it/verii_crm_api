using crm_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/salesmen")]
    [Authorize]
    public class Salesmen360Controller : ControllerBase
    {
        private readonly ISalesmen360Service _salesmen360Service;

        public Salesmen360Controller(ISalesmen360Service salesmen360Service)
        {
            _salesmen360Service = salesmen360Service;
        }

        [HttpGet("{userId}/overview")]
        public async Task<IActionResult> GetOverview(long userId, [FromQuery] string? currency = null)
        {
            var effectiveCurrency = ResolveCurrency(currency);
            var result = await _salesmen360Service.GetOverviewAsync(userId, effectiveCurrency);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{userId}/analytics/summary")]
        public async Task<IActionResult> GetAnalyticsSummary(long userId, [FromQuery] string? currency = null)
        {
            var effectiveCurrency = ResolveCurrency(currency);
            var result = await _salesmen360Service.GetAnalyticsSummaryAsync(userId, effectiveCurrency);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{userId}/analytics/charts")]
        public async Task<IActionResult> GetAnalyticsCharts(long userId, [FromQuery] int months = 12, [FromQuery] string? currency = null)
        {
            var effectiveCurrency = ResolveCurrency(currency);
            var result = await _salesmen360Service.GetAnalyticsChartsAsync(userId, months, effectiveCurrency);
            return StatusCode(result.StatusCode, result);
        }

        private string? ResolveCurrency(string? currency)
        {
            if (!string.IsNullOrWhiteSpace(currency))
            {
                return currency;
            }

            if (Request.Headers.TryGetValue("X-Currency", out var headerCurrency) && !string.IsNullOrWhiteSpace(headerCurrency))
            {
                return headerCurrency.ToString();
            }

            if (Request.Headers.TryGetValue("Currency", out var plainHeaderCurrency) && !string.IsNullOrWhiteSpace(plainHeaderCurrency))
            {
                return plainHeaderCurrency.ToString();
            }

            return null;
        }
    }
}
