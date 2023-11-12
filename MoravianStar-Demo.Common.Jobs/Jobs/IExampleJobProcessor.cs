using MoravianStar_Demo.Common.Core.DTOs.Test;
using MoravianStar_Demo.Common.Jobs.Common;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Common.Jobs.Jobs
{
    public interface IExampleJobProcessor
    {
        [ExecuteInTransaction]
        Task Process(ExampleJobSenderMessage message);
    }
}