using LinqKit;
using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.Entities.Test;
using System.Linq;

namespace MoravianStar_Demo.Common.Core.Filters.Test
{
    public class VehicleFilter : FilterSorterBase<VehicleEntity>
    {
        public string LicensePlateEqualsInsensitive { get; set; }

        public override IQueryable<VehicleEntity> Filter<TDbContext>(IQueryable<VehicleEntity> query, IEntityRepository<VehicleEntity, TDbContext> entityRepository)
        {
            query = base.Filter(query, entityRepository);

            var rootCriteria = PredicateBuilder.New<VehicleEntity>(x => true);
            var mainCriteria = PredicateBuilder.New<VehicleEntity>(x => true);

            if (!string.IsNullOrEmpty(LicensePlateEqualsInsensitive))
            {
                mainCriteria = mainCriteria.And(x => x.LicensePlate.ToLower() == LicensePlateEqualsInsensitive.ToLower());
            }

            rootCriteria = mainCriteria;

            return query.Where(rootCriteria);
        }
    }
}