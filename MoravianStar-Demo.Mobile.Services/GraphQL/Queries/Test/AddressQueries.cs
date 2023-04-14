using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.Core.Filters.Test;
using MoravianStar_Demo.Persistence.DbContexts;
using System.Collections.Generic;
using System.Linq;

namespace MoravianStar_Demo.Mobile.Services.GraphQL.Queries.Test
{
    [ExtendObjectType(typeof(Query))]
    public class AddressQueries
    {
        [UseDataLayerDbContext(typeof(DataLayer_SystemContext))]
        [UseOffsetPaging]
        [UseProjection]
        [GraphQLDescription("Gets the queryable addresses.")]
        public IQueryable<AddressEntity> GetAddresses(AddressFilter filter, List<Sort> sorts, [ScopedService] SystemRepository repository)
        {
            return repository.ReadQuery<AddressEntity, AddressFilter>(filter, sorts, trackable: false);
        }
    }
}