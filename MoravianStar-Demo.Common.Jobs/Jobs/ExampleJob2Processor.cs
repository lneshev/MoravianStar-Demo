using System;
using System.Threading;

namespace MoravianStar_Demo.Common.Jobs.Jobs
{
    public class ExampleJob2Processor : IExampleJob2Processor
    {
        public void Process()
        {
            Console.WriteLine("Start");
            Thread.Sleep(3000);
            Console.WriteLine("Done");
        }
    }
}