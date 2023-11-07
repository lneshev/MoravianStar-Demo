using Microsoft.Extensions.DependencyInjection;

namespace MoravianStar_Demo.Common.Jobs.Server
{
    public static class HangfireServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a Hangfire server.
        /// </summary>
        /// <param name="services">The collection of services.</param>
        /// <returns>The collection of services.</returns>
        public static IServiceCollection AddHangfireServer(this IServiceCollection services)
        {
            return Hangfire.HangfireServiceCollectionExtensions.AddHangfireServer(services);
        }
    }
}