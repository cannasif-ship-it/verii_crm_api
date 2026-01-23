using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using crm_api.DTOs;

namespace crm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetDashboard()
        {
            // Return empty dashboard data to prevent 404 errors
            var dashboardData = new
            {
                Stats = new { },
                RecentActivities = new object[] { }
            };
            return Ok(ApiResponse<object>.SuccessResult(dashboardData, "Dashboard data retrieved successfully"));
        }

        [HttpGet("currencyRates")]
        public IActionResult GetCurrencyRates()
        {
            // Return empty response to prevent 404 errors
            return Ok(ApiResponse<object>.SuccessResult(null, "Currency rates endpoint - not implemented"));
        }
    }
}
