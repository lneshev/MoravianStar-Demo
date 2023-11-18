using MoravianStar_Demo.Common.Core.DTOs.Test;
using MoravianStar_Demo.Common.Jobs.Common;
using MoravianStar_Demo.Persistence.DbContexts;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Common.Jobs.Jobs
{
    public class ExampleJobFlow : JobFlowBase, IExampleJobFlow
    {
        private readonly IServiceProvider serviceProviderBase;
        private readonly IExampleJobProcessor exampleJobProcessor;

        public ExampleJobFlow(IServiceProvider serviceProviderBase, IExampleJobProcessor exampleJobProcessor)
        {
            this.serviceProviderBase = serviceProviderBase;
            this.exampleJobProcessor = exampleJobProcessor;
        }

        protected override void UseMiddlewares(ICollection<IJobFlowMiddleware> middlewares)
        {
            base.UseMiddlewares(middlewares);
            middlewares.Add(new ServiceLocatorJobFlowMiddleware(serviceProviderBase));
            middlewares.Add(new ExecuteInTransactionJobFlowMiddleware(typeof(SystemContext)));
        }

        protected override async Task ProcessJob(params object[] args)
        {
            var message = (args[0] as JObject).ToObject<ExampleJobSenderMessage>();
            await exampleJobProcessor.Process(message);
        }
    }
}