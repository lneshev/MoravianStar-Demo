using MoravianStar_Demo.Common.Core.DTOs.Test;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Common.Jobs.Jobs
{
    public interface IExampleJobProcessor
    {
        //[ExecuteInTransaction] // // Commented, because the current Hangfire version doesn't suport async job execution
        Task Process(ExampleJobSenderMessage message);
    }
}