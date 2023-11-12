using MoravianStar_Demo.Common.Core.DTOs.Test;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Common.Services.Test
{
    public interface IExampleJobSender
    {
        Task SendJob(ExampleJobSenderMessage message);
    }
}