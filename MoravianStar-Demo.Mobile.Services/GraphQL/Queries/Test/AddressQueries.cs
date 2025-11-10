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
    public class AddressQueries
    {
        [UseServiceLocator]
        [UseOffsetPaging]
        [UseProjection]
        [GraphQLDescription("Gets the queryable addresses.")]
        public IQueryable<AddressEntity> GetAddresses(AddressFilter filter, List<Sort> sorts)
        {
            return Persistence.ForDbContext<SystemContext>().ForEntity<AddressEntity>().ReadQuery(filter, sorts, trackable: false);
        }
    }
}