using Hangfire.Dashboard;

namespace Hastnama.Solico.Api.Filter
{
    public class CustomAuthorization : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}