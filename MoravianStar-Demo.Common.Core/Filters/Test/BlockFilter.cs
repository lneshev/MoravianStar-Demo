using LinqKit;
using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.Entities.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MoravianStar_Demo.Common.Core.Filters.Test
{
    public class BlockFilter : FilterSorterBase<BlockEntity>
    {
        public string ClientNameContainsInsensitive { get; set; }

        public override IQueryable<BlockEntity> Filter<TDbContext>(IQueryable<BlockEntity> query, IEntityRepository<BlockEntity, TDbContext> repository)
        {
            query = base.Filter(query, repository);

            var mainCriteria = PredicateBuilder.New<BlockEntity>(x => true);

            if (!string.IsNullOrEmpty(ClientNameContainsInsensitive))
            {
                mainCriteria = mainCriteria.And(x => x.Client.Name.ToLower().Contains(ClientNameContainsInsensitive.ToLower()));
            }

            return query.Where(mainCriteria);
        }

        public override List<(Expression<Func<BlockEntity, object>> expression, SortDirection direction)> Sort<TDbContext>(IEnumerable<Sort> sorts, IEntityRepository<BlockEntity, TDbContext> repository)
        {
            var result = base.Sort(sorts, repository);

            foreach (var sort in sorts)
            {
                if (sort.Field.Equals("ClientName", StringComparison.OrdinalIgnoreCase))
                {
                    result.Add((x => x.Client.Name, sort.Dir));
                }
            }

            return result;
        }
    }
}