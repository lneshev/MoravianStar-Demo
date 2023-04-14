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

namespace MoravianStar_Demo.Mobile.Services.GraphQL.Queries.Test
{
    [ExtendObjectType(typeof(Query))]
    public class BlockQueries
    {
        [UseDataLayerDbContext(typeof(DataLayer_ClientDMLContext))]
        [UseOffsetPaging]
        [UseProjection]
        [GraphQLDescription("Gets the queryable blocks.")]
        public IQueryable<BlockEntity> GetBlocks(BlockFilter filter, List<Sort> sorts, [ScopedService] ClientRepository repository)
        {
            return repository.ReadQuery<BlockEntity, BlockFilter>(filter, sorts, trackable: false);
        }
    }
}