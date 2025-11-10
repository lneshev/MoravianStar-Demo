using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using MoravianStar.Dao;
using MoravianStar.GraphQL.Attributes;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.Core.Filters.Test;
using MoravianStar_Demo.Common.DataAccess.DbContexts;
using System.Collections.Generic;
using System.Linq;

namespace MoravianStar_Demo.Mobile.Services.GraphQL.Queries.Test
{
    [ExtendObjectType(typeof(Query))]
    public class BlockQueries
    {
        [UseServiceLocator]
        [UseClientDMLContext]
        [UseOffsetPaging]
        [UseProjection]
        [GraphQLDescription("Gets the queryable blocks.")]
        public IQueryable<BlockEntity> GetBlocks(BlockFilter filter, List<Sort> sorts)
        {
            return Persistence.ForDbContext<ClientDMLContext>().ForEntity<BlockEntity>().ReadQuery(filter, sorts, trackable: false);
        }
    }
}