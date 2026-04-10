using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace crm_api.Modules.System.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly ILocalizationService _localizationService;

        public DashboardController(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        [HttpGet]
        public IActionResult GetDashboard()
        {
            // Return empty dashboard data to prevent 404 errors
            var dashboardData = new
            {
                Stats = new { },
                RecentActivities = new object[] { }
            };
            return Ok(ApiResponse<object>.SuccessResult(
                dashboardData,
                _localizationService.GetLocalizedString("DashboardController.DashboardRetrieved")));
        }

        [HttpGet("currencyRates")]
        public IActionResult GetCurrencyRates()
        {
            // Return empty response to prevent 404 errors
            return Ok(ApiResponse<object>.SuccessResult(
                null,
                _localizationService.GetLocalizedString("DashboardController.CurrencyRatesNotImplemented")));
        }
    }
}
