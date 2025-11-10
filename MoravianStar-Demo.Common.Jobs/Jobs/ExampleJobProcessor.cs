using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.DTOs.Test;
using MoravianStar_Demo.Common.Core.Entities.Test;
using System.Threading;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Common.Jobs.Jobs
{
    public class ExampleJobProcessor : IExampleJobProcessor
    {
        public async Task Process(ExampleJobSenderMessage message)
        {
            Thread.Sleep(15000);

            var client = await Persistence.ForEntity<ClientEntity, int>().GetAsync(message.ClientId);
            if (string.IsNullOrWhiteSpace(client.Description))
            {
                client.Description = "Description set from JobWebAPI successfully!";
            }
            await Persistence.ForEntity<ClientEntity>().SaveAsync(client);
        }
    }
}