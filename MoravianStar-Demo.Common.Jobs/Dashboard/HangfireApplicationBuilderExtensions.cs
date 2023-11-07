using Hangfire;
using Microsoft.AspNetCore.Builder;

namespace MoravianStar_Demo.Common.Jobs.Dashboard
{
    public static class HangfireApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds a Hangfire dashboard UI.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="pathMatch">The path to the dashboard.</param>
        /// <param name="options">The dashboard options.</param>
        /// <param name="storage">Specifies from which job storage to retreive the data.</param>
        /// <returns>The application builder.</returns>
        public static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app, string pathMatch = "/hangfire", DashboardOptions options = null, JobStorage storage = null)
        {
            return Hangfire.HangfireApplicationBuilderExtensions.UseHangfireDashboard(app, pathMatch, options, storage);
        }
    }
}