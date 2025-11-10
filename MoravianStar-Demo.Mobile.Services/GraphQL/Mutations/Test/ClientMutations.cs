using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;
using MoravianStar.GraphQL.Attributes;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Mobile.Core.GraphQL.Mutations.Test;
using MoravianStar_Demo.Common.DataAccess.DbContexts;
using System;
using System.Threading.Tasks;
using MS = MoravianStar.Dao;

namespace MoravianStar_Demo.Mobile.Services.GraphQL.Mutations.Test
{
    [ExtendObjectType(typeof(Mutation))]
    public class ClientMutations
    {
        [UseServiceLocator]
        [GraphQLDescription("Creates a client.")]
        public async Task<ClientEntity> CreateClientAsync(SaveClientInput input)
        {
            var dbContextService = MS.Persistence.ForDbContext<SystemContext>();
            var dbTransaction = dbContextService.DbTransaction;

            await dbTransaction.BeginAsync();

            var entity = new ClientEntity();

            entity.Name = input.Name;
            entity.Description = input.Description;
            entity.MainAddress = input.MainAddress != null ? new AddressEntity() { Address = input.MainAddress.Address } : null;

            await dbContextService.ForEntity<ClientEntity>().SaveAsync(entity);

            await dbTransaction.CommitAsync();

            // await eventSender.SendAsync(nameof(Subscription.OnClientCreated), entity);

            return entity;
        }

        [UseServiceLocator]
        [UseTransaction(typeof(SystemContext))]
        [GraphQLDescription("Updates a client.")]
        public async Task<ClientEntity> UpdateClientAsync([ID] int id, SaveClientInput input)
        {
            var entity = await MS.Persistence.ForDbContext<SystemContext>().ForEntity<ClientEntity, int>().GetAsync(id, x => x.Include(y => y.MainAddress));

            entity.Name = input.Name;
            entity.Description = input.Description;

            if (!entity.MainAddressId.HasValue && input.MainAddress != null)
            {
                // Create a new address
                entity.MainAddress = new AddressEntity() { Address = input.MainAddress.Address };
            }
            else if (entity.MainAddressId.HasValue && input.MainAddress == null)
            {
                // Delete the address
                await MS.Persistence.ForDbContext<SystemContext>().ForEntity<AddressEntity, Guid>().DeleteAsync(entity.MainAddressId.Value);
            }
            else if (entity.MainAddressId.HasValue && input.MainAddress != null)
            {
                // Update the existing address
                entity.MainAddress.Address = input.MainAddress.Address;
            }

            await MS.Persistence.ForDbContext<SystemContext>().ForEntity<ClientEntity>().SaveAsync(entity);

            // await eventSender.SendAsync(nameof(Subscription.OnClientCreated), entity);

            return entity;
        }

        [UseServiceLocator]
        [UseTransaction]
        [GraphQLDescription("Deletes a client.")]
        public async Task<bool> DeleteClientAsync([ID] int id)
        {
            var entity = await MS.Persistence.ForEntity<ClientEntity, int>().GetAsync(id);

            if (entity.MainAddressId.HasValue)
            {
                await MS.Persistence.ForEntity<AddressEntity, Guid>().DeleteAsync(entity.MainAddressId.Value);
            }

            await MS.Persistence.ForEntity<ClientEntity>().DeleteAsync(entity);

            // await eventSender.SendAsync(nameof(Subscription.OnClientDeleted), entity); // Entity vs Id vs ?!

            return true;
        }
    }
}