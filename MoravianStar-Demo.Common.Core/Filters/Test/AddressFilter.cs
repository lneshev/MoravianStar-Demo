using LinqKit;
using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.Entities.Test;
using System;
using System.Linq;

namespace MoravianStar_Demo.Common.Core.Filters.Test
{
    public class AddressFilter : FilterSorterBase<AddressEntity>
    {
        public string ClientNameContainsInsensitive { get; set; }

        public override IQueryable<AddressEntity> Filter<TDbContext>(IQueryable<AddressEntity> query, IEntityRepository<AddressEntity, TDbContext> repository)
        {
            query = base.Filter(query, repository);

            var mainCriteria = PredicateBuilder.New<AddressEntity>(x => true);

            if (!string.IsNullOrEmpty(ClientNameContainsInsensitive))
            {
                //V1
                //var clientNameContainsInsensitiveQueryAddressIds = Persistence.ForDbContext<TDbContext>().ForEntity<ClientEntity>().ReadQuery<ClientFilter>()
                //    .Where(x => x.Name.ToLower().Contains(ClientNameContainsInsensitive.ToLower()))
                //    .Select(x => x.MainAddressId);

                //V2
                var clientNameContainsInsensitiveQueryAddressIds = Persistence.ForDbContext<TDbContext>().ForEntity<ClientEntity>().ReadQuery(
                    new ClientFilter() { NameContainsInsensitive = ClientNameContainsInsensitive })
                    .Select(x => x.MainAddressId);

                mainCriteria = mainCriteria.And(x => clientNameContainsInsensitiveQueryAddressIds.Contains(x.Id));
            }

            return query.Where(mainCriteria);
        }
    }
}