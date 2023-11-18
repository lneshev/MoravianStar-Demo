using Microsoft.Extensions.DependencyInjection;
using MoravianStar.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Common.Jobs.Common
{
    public class ServiceLocatorJobFlowMiddleware : IJobFlowMiddleware
    {
        private readonly IServiceProvider serviceProviderBase;

        public ServiceLocatorJobFlowMiddleware(IServiceProvider serviceProviderBase)
        {
            this.serviceProviderBase = serviceProviderBase;
        }

        public async Task InvokeAsync(Func<Task> next)
        {
            using (var scope = serviceProviderBase.CreateScope())
            {
                new ServiceLocator(() => scope.ServiceProvider);
                await next();
            }
        }
    }
}