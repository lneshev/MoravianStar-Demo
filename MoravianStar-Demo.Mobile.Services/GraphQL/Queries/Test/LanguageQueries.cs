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
    public class LanguageQueries
    {
        [UseMoravianStar]
        [UseOffsetPaging]
        [UseProjection]
        [GraphQLDescription("Gets the queryable languages.")]
        public IQueryable<LanguageEntity> GetLanguages(List<Sort> sorts)
        {
            return MS.Persistence.ForDbContext<SystemContext>().ForEntity<LanguageEntity>().ReadQuery<LanguageFilter>(null, sorts, trackable: false);
        }
    }
}