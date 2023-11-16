using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using MoravianStar.Dao;
using MoravianStar.GraphQL.Attributes;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.Core.Filters.Test;
using MoravianStar_Demo.Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using MS = MoravianStar.Dao;

namespace MoravianStar_Demo.Mobile.Services.GraphQL.Queries.Test
{
    [ExtendObjectType(typeof(Query))]
    public class VehicleQueries
    {
        [UseServiceLocator]
        [UseOffsetPaging]
        [UseProjection]
        [GraphQLDescription("Gets the queryable vehicles.")]
        public IQueryable<VehicleEntity> GetVehicles(List<Sort> sorts)
        {
            return MS.Persistence.ForDbContext<SystemContext>().ForEntity<VehicleEntity>().ReadQuery<VehicleFilter>(null, sorts, trackable: false);
        }

        [Obsolete("To be deleted after the demo.")]
        [UseServiceLocator]
        [UseOffsetPaging]
        [UseProjection]
        [UseFiltering(typeof(VehicleFilterType))] // I tried to add custom filter type in order to create custom filters, but no success.
        [UseSorting(typeof(VehicleSortType))] // When there is a spatial type property in the entity, the sorting is not working (neither ignoring the property). That's why a custom sorting type should be created.
        [GraphQLDescription("Gets the queryable vehicles.")]
        public IQueryable<VehicleEntity> GetVehiclesObsolete()
        {
            return MS.Persistence.ForDbContext<SystemContext>().ForEntity<VehicleEntity>().ReadQuery<VehicleFilter>(trackable: false);
        }
    }
}