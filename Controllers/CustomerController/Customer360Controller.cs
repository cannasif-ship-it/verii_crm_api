using crm_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/customers")]
    [Authorize]
    public class Customer360Controller : ControllerBase
    {
        private readonly ICustomer360Service _customer360Service;

        public Customer360Controller(ICustomer360Service customer360Service)
        {
            _customer360Service = customer360Service;
        }

        [HttpGet("{id}/overview")]
        public async Task<IActionResult> GetOverview(long id)
        {
            var result = await _customer360Service.GetOverviewAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}/analytics/summary")]
        public async Task<IActionResult> GetAnalyticsSummary(long id)
        {
            var result = await _customer360Service.GetAnalyticsSummaryAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}/analytics/charts")]
        public async Task<IActionResult> GetAnalyticsCharts(long id, [FromQuery] int months = 12)
        {
            var result = await _customer360Service.GetAnalyticsChartsAsync(id, months);
            return StatusCode(result.StatusCode, result);
        }
    }
}
