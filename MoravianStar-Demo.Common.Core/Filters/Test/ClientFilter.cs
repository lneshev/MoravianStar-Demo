using LinqKit;
using MoravianStar.Dao;
using MoravianStar.Extensions;
using MoravianStar_Demo.Common.Core.Entities.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MoravianStar_Demo.Common.Core.Filters.Test
{
    public class ClientFilter : FilterSorterBase<ClientEntity>
    {
        public string NameEquals { get; set; }
        public string NameEqualsInsensitive { get; set; }
        public string NameContains { get; set; }
        public string NameContainsInsensitive { get; set; }
        public int? AdditionalId { get; set; }
        public Guid? MainAddressId { get; set; }
        public string MainAddressAddressContainsInsensitive { get; set; }
        public bool? HasMainAddress { get; set; }
        public bool? HasAdditionalAddress { get; set; }

        public override IQueryable<ClientEntity> Filter<TDbContext>(IQueryable<ClientEntity> query, IEntityRepository<ClientEntity, TDbContext> entityRepository)
        {
            query = base.Filter(query, entityRepository);

            var rootCriteria = PredicateBuilder.New<ClientEntity>(x => true);
            var mainCriteria = PredicateBuilder.New<ClientEntity>(x => true);
            var additionalIdCriteria = PredicateBuilder.New<ClientEntity>(x => true);

            if (!string.IsNullOrEmpty(NameEquals))
            {
                mainCriteria = mainCriteria.And(x => x.Name == NameEquals);
            }

            if (!string.IsNullOrEmpty(NameEqualsInsensitive))
            {
                mainCriteria = mainCriteria.And(x => x.Name.ToLower() == NameEqualsInsensitive.ToLower());
            }

            if (!string.IsNullOrEmpty(NameContains))
            {
                mainCriteria = mainCriteria.And(x => x.Name.Contains(NameContains));
            }

            if (!string.IsNullOrEmpty(NameContainsInsensitive))
            {
                mainCriteria = mainCriteria.And(x => x.Name.ToLower().Contains(NameContainsInsensitive.ToLower()));
            }

            if (!MainAddressId.IsNullOrEmpty())
            {
                mainCriteria = mainCriteria.And(x => x.MainAddressId == MainAddressId);
            }

            if (!string.IsNullOrEmpty(MainAddressAddressContainsInsensitive))
            {
                mainCriteria = mainCriteria.And(x => x.MainAddress.Address.ToLower().Contains(MainAddressAddressContainsInsensitive.ToLower()));
            }

            if (HasMainAddress.HasValue)
            {
                mainCriteria = mainCriteria.And(x => x.MainAddressId.HasValue == HasMainAddress);
            }

            if (HasAdditionalAddress.HasValue)
            {
                mainCriteria = mainCriteria.And(x => x.Addresses.Any() == HasAdditionalAddress);
            }

            if (AdditionalId.HasValue)
            {
                additionalIdCriteria = additionalIdCriteria.And(x => x.Id == AdditionalId);
            }

            rootCriteria = AdditionalId.HasValue ? mainCriteria.Or(additionalIdCriteria) : mainCriteria;

            return query.Where(rootCriteria);
        }

        public override List<(Expression<Func<ClientEntity, object>> expression, SortDirection direction)> Sort<TDbContext>(IEnumerable<Sort> sorts, IEntityRepository<ClientEntity, TDbContext> repository)
        {
            var result = base.Sort(sorts, repository);

            foreach (var sort in sorts)
            {
                if (sort.Field.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    result.Add((x => x.Name, sort.Dir));
                }
                else if (sort.Field.Equals("Status", StringComparison.OrdinalIgnoreCase))
                {
                    result.Add((x => x.Status, sort.Dir));
                }
                else if (sort.Field.Equals("AddressesCount", StringComparison.OrdinalIgnoreCase))
                {
                    result.Add((x => x.Addresses.Count, sort.Dir));
                }
            }

            return result;
        }
    }
}