using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Common.Jobs.Common
{
    public abstract class JobFlowBase : IJobFlow
    {
        private List<IJobFlowMiddleware> middlewares = new List<IJobFlowMiddleware>();

        public void Process(params object[] args)
        {
            Task.Run(async () =>
            {
                UseMiddlewares(middlewares);
                var aggregatedMiddlewares = middlewares
                    .AsEnumerable()
                    .Reverse()
                    .Aggregate(() => ProcessJob(args), (next, middleware) => () => middleware.InvokeAsync(next));

                await aggregatedMiddlewares.Invoke();
            }).GetAwaiter().GetResult();
        }

        protected virtual void UseMiddlewares(ICollection<IJobFlowMiddleware> middlewares)
        {
        }

        protected abstract Task ProcessJob(params object[] args);
    }
}