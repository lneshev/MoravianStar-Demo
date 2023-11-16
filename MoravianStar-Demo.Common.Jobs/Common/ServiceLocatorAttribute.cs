using Hangfire.Common;
using Hangfire.Server;
using Microsoft.Extensions.DependencyInjection;
using MoravianStar.DependencyInjection;
using System;

namespace MoravianStar_Demo.Common.Jobs.Common
{
    /// <summary>
    /// An <see cref="IServerFilter"/> attribute that initializes the <see cref="ServiceLocator"/>.
    /// </summary>
    public class ServiceLocatorAttribute : JobFilterAttribute, IServerFilter
    {
        private readonly IServiceProvider serviceProviderBase;
        private IServiceScope scope;

        public ServiceLocatorAttribute(IServiceProvider serviceProviderBase)
        {
            this.serviceProviderBase = serviceProviderBase;
        }

        public void OnPerforming(PerformingContext context)
        {
            scope = serviceProviderBase.CreateScope();
            new ServiceLocator(() => scope.ServiceProvider);
        }

        public void OnPerformed(PerformedContext context)
        {
            scope.Dispose();
        }
    }
}