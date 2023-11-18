using System;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Common.Jobs.Common
{
    public interface IJobFlowMiddleware
    {
        Task InvokeAsync(Func<Task> next);
    }
}