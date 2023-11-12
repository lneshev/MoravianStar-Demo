using MoravianStar_Demo.Common.Core.DTOs.Test;
using MoravianStar_Demo.Common.Core.Entities.Test;
using System.Threading;
using System.Threading.Tasks;
using MS = MoravianStar.Dao;

namespace MoravianStar_Demo.Common.Jobs.Jobs
{
    public class ExampleJobProcessor : IExampleJobProcessor
    {
        public async Task Process(ExampleJobSenderMessage message)
        {
            Thread.Sleep(1000);

            var client = await MS.Persistence.ForEntity<ClientEntity, int>().GetAsync(message.ClientId);
            if (string.IsNullOrWhiteSpace(client.Description))
            {
                client.Description = "Description set from JobWebAPI successfully!";
            }
            await MS.Persistence.ForEntity<ClientEntity>().SaveAsync(client);
        }
    }
}