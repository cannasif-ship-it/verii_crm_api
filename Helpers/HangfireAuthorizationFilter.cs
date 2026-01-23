using Hangfire.Dashboard;

namespace crm_api.Helpers
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            // Allow all authenticated users to access Hangfire dashboard
            // You can add custom authorization logic here if needed
            return true;
        }
    }
}
