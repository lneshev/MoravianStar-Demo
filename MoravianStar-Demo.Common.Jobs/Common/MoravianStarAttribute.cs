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
    public class MoravianStarAttribute : JobFilterAttribute, IServerFilter
    {
        private readonly IServiceProvider serviceProviderBase;
        private IServiceScope scope;
        private ServiceLocator serviceLocator;

        public MoravianStarAttribute(IServiceProvider serviceProviderBase)
        {
            this.serviceProviderBase = serviceProviderBase;
        }

        public void OnPerforming(PerformingContext context)
        {
            scope = serviceProviderBase.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            serviceLocator = new ServiceLocator(serviceProvider);
        }

        public void OnPerformed(PerformedContext context)
        {
            serviceLocator.Dispose();
            scope.Dispose();
        }
    }
}