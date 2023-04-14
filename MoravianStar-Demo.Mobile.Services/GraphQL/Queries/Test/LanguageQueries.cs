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
    public class LanguageQueries
    {
        [UseDataLayerDbContext(typeof(DataLayer_SystemContext))]
        [UseOffsetPaging]
        [UseProjection]
        [GraphQLDescription("Gets the queryable languages.")]
        public IQueryable<LanguageEntity> GetLanguages(List<Sort> sorts, [ScopedService] SystemRepository repository)
        {
            return repository.ReadQuery<LanguageEntity, LanguageFilter>(null, sorts, trackable: false);
        }
    }
}