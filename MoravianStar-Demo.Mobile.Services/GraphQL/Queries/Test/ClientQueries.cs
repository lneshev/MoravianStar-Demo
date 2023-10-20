using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using MoravianStar.Dao;
using MoravianStar.GraphQL.Attributes;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.Core.Filters.Test;
using MoravianStar_Demo.Persistence.DbContexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MS = MoravianStar.Dao;

namespace MoravianStar_Demo.Mobile.Services.GraphQL.Queries.Test
{
    [ExtendObjectType(typeof(Query))]
    public class ClientQueries
    {
        [UseMoravianStar]
        [UseOffsetPaging]
        [UseProjection]
        [GraphQLDescription("Gets the queryable clients.")]
        public IQueryable<ClientEntity> GetClients(ClientFilter filter, List<Sort> sorts)
        {
            return MS.Persistence.ForDbContext<SystemContext>().ForEntity<ClientEntity>().ReadQuery(filter, sorts, trackable: false);
        }

        [UseMoravianStar]
        [GraphQLDescription("Returns the count of clients.")]
        public async Task<int> CountClientsAsync(ClientFilter filter)
        {
            return await MS.Persistence.ForDbContext<SystemContext>().ForEntity<ClientEntity>().CountAsync(filter);
        }

        [UseMoravianStar]
        [GraphQLDescription("Checks if clients exist.")]
        public async Task<bool> ExistClientsAsync(ClientFilter filter)
        {
            return await MS.Persistence.ForDbContext<SystemContext>().ForEntity<ClientEntity>().ExistAsync(filter);
        }
    }
}