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
using MS = MoravianStar.Dao;

namespace MoravianStar_Demo.Mobile.Services.GraphQL.Queries.Test
{
    [ExtendObjectType(typeof(Query))]
    public class AddressQueries
    {
        [UseServiceLocator]
        [UseOffsetPaging]
        [UseProjection]
        [GraphQLDescription("Gets the queryable addresses.")]
        public IQueryable<AddressEntity> GetAddresses(AddressFilter filter, List<Sort> sorts)
        {
            return MS.Persistence.ForDbContext<SystemContext>().ForEntity<AddressEntity>().ReadQuery(filter, sorts, trackable: false);
        }
    }
}