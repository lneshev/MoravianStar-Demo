using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Mobile.Core.GraphQL.Mutations.Test;
using MoravianStar_Demo.Persistence.DbContexts;
using System;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Mobile.Services.GraphQL.Mutations.Test
{
    [ExtendObjectType(typeof(Mutation))]
    public class ClientMutations
    {
        [UseDataLayerDbContext(typeof(DataLayer_SystemContext))]
        [GraphQLDescription("Creates a client.")]
        public async Task<ClientEntity> CreateClientAsync(SaveClientInput input, [ScopedService] SystemRepository repository)
        {
            ClientEntity entity = null;

            using (var tx = await repository.BeginTransactionAsync())
            {
                entity = new ClientEntity();

                entity.Name = input.Name;
                entity.Description = input.Description;
                entity.MainAddress = input.MainAddress != null ? new AddressEntity() { Address = input.MainAddress.Address } : null;

                entity = await repository.AddAsync(entity);
                await repository.SaveChangesAsync();
                await tx.CommitAsync();

                // await eventSender.SendAsync(nameof(Subscription.OnClientCreated), entity);
            }

            return entity;
        }

        [UseDataLayerDbContext(typeof(DataLayer_SystemContext))]
        [GraphQLDescription("Updates a client.")]
        public async Task<ClientEntity> UpdateClientAsync([ID] int id, SaveClientInput input, [ScopedService] SystemRepository repository)
        {
            ClientEntity entity = null;

            using (var tx = await repository.BeginTransactionAsync())
            {
                entity = await repository.GetAsync<ClientEntity, int>(id, x => x.Include(y => y.MainAddress));

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
                    await repository.DeleteAsync<AddressEntity, Guid>(entity.MainAddressId.Value);
                }
                else if (entity.MainAddressId.HasValue && input.MainAddress != null)
                {
                    // Update the existing address
                    entity.MainAddress.Address = input.MainAddress.Address;
                }

                entity = repository.Update(entity);
                await repository.SaveChangesAsync();
                await tx.CommitAsync();

                // await eventSender.SendAsync(nameof(Subscription.OnClientCreated), entity);
            }

            return entity;
        }

        [UseDataLayerDbContext(typeof(DataLayer_SystemContext))]
        [GraphQLDescription("Deletes a client.")]
        public async Task<bool> DeleteClientAsync([ID] int id, [ScopedService] SystemRepository repository)
        {
            using (var tx = await repository.BeginTransactionAsync())
            {
                var entity = await repository.GetAsync<ClientEntity, int>(id);
                if (entity.MainAddressId.HasValue)
                {
                    await repository.DeleteAsync<AddressEntity, Guid>(entity.MainAddressId.Value);
                }
                repository.Delete(entity);
                await repository.SaveChangesAsync();
                await tx.CommitAsync();

                // await eventSender.SendAsync(nameof(Subscription.OnClientDeleted), entity); // Entity vs Id vs ?!
            }

            return true;
        }
    }
}