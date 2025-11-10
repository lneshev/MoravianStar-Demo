using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.DTOs.Test;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.DataAccess.DbContexts;
using System.Collections.Generic;
using System.Threading.Tasks;
using MS = MoravianStar.Dao;

namespace MoravianStar_Demo.Common.Services.Test
{
    public class ClientSaved : IEntitySaved<ClientEntity>
    {
        private readonly IExampleJobSender exampleJobSender;

        public ClientSaved(IExampleJobSender exampleJobSender)
        {
            this.exampleJobSender = exampleJobSender;
        }

        public async Task SavedAsync(ClientEntity entity, ClientEntity originalEntity, bool entityWasNew, IDictionary<string, object> additionalParameters = null)
        {
            MS.Persistence.ForDbContext<SystemContext>().DbTransaction.Committed += (sender, eventArgs) =>
            {
                // Not awaited intentionally. Fire and forget.
                exampleJobSender.SendJob(new ExampleJobSenderMessage()
                {
                    ClientId = entity.Id
                });
            };

            await Task.CompletedTask;
        }
    }
}