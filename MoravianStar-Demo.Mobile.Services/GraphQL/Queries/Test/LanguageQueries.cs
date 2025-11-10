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
    public class LanguageQueries
    {
        [UseServiceLocator]
        [UseOffsetPaging]
        [UseProjection]
        [GraphQLDescription("Gets the queryable languages.")]
        public IQueryable<LanguageEntity> GetLanguages(List<Sort> sorts)
        {
            return Persistence.ForDbContext<SystemContext>().ForEntity<LanguageEntity>().ReadQuery<LanguageFilter>(null, sorts, trackable: false);
        }
    }
}