using DataLayer.Common.Core.DTOs;
using DataLayer.Common.Core.Entities.Test;
using DataLayer.Common.Core.Filters.Test;
using DataLayer.Common.Services;
using DataLayer.Persistence.DbContexts;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Mobile.Services.GraphQL.Queries.Test
{
    [ExtendObjectType(typeof(Query))]
    public class ClientQueries
    {
        [UseDataLayerDbContext(typeof(DataLayer_SystemContext))]
        [UseOffsetPaging]
        [UseProjection]
        [GraphQLDescription("Gets the queryable clients.")]
        public IQueryable<ClientEntity> GetClients(ClientFilter filter, List<Sort> sorts, [ScopedService] SystemRepository repository)
        {
            return repository.ReadQuery<ClientEntity, ClientFilter>(filter, sorts, trackable: false);
        }

        [UseDataLayerDbContext(typeof(DataLayer_SystemContext))]
        [GraphQLDescription("Returns the count of clients.")]
        public async Task<int> CountClientsAsync(ClientFilter filter, [ScopedService] SystemRepository repository)
        {
            return await repository.CountAsync<ClientEntity, ClientFilter>(filter);
        }

        [UseDataLayerDbContext(typeof(DataLayer_SystemContext))]
        [GraphQLDescription("Checks if clients exist.")]
        public async Task<bool> ExistClientsAsync(ClientFilter filter, [ScopedService] SystemRepository repository)
        {
            return await repository.ExistAsync<ClientEntity, ClientFilter>(filter);
        }
    }
}