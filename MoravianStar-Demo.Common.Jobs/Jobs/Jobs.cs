using Hangfire;
using MoravianStar_Demo.Common.Core.DTOs.Test;
using MoravianStar_Demo.Common.Jobs.Common;

namespace MoravianStar_Demo.Common.Jobs.Jobs
{
    public class Jobs
    {
        public static string ExampleJob(ExampleJobSenderMessage message)
        {
            var j1 = BackgroundJob.Enqueue<IExampleJobFlow>(x => ((IJobFlow)x).Process(message));
            return BackgroundJob.ContinueJobWith<IExampleJob2Processor>(j1, x => x.Process());
        }

        public static string ExampleJob2()
        {
            return BackgroundJob.Enqueue<IExampleJob2Processor>(x => x.Process());
        }
    }
}