using DataLayer.Common.Core.DTOs;
using DataLayer.Common.Core.Entities.Test;
using DataLayer.Common.Core.Filters.Test;
using DataLayer.Common.Services;
using DataLayer.Persistence.DbContexts;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoravianStar_Demo.Mobile.Services.GraphQL.Queries.Test
{
    [ExtendObjectType(typeof(Query))]
    public class VehicleQueries
    {
        [UseDataLayerDbContext(typeof(DataLayer_SystemContext))]
        [UseOffsetPaging]
        [UseProjection]
        [GraphQLDescription("Gets the queryable vehicles.")]
        public IQueryable<VehicleEntity> GetVehicles(List<Sort> sorts, [ScopedService] SystemRepository repository)
        {
            return repository.ReadQuery<VehicleEntity, VehicleFilter>(null, sorts, trackable: false);
        }

        [Obsolete("To be deleted after the demo.")]
        [UseDataLayerDbContext(typeof(DataLayer_SystemContext))]
        [UseOffsetPaging]
        [UseProjection]
        [UseFiltering(typeof(VehicleFilterType))] // I tried to add custom filter type in order to create custom filters, but no success.
        [UseSorting(typeof(VehicleSortType))] // When there is a spatial type property in the entity, the sorting is not working (neither ignoring the property). That's why a custom sorting type should be created.
        [GraphQLDescription("Gets the queryable vehicles.")]
        public IQueryable<VehicleEntity> GetVehiclesObsolete([ScopedService] SystemRepository repository)
        {
            return repository.GetAllQuery<VehicleEntity>().AsNoTracking();
        }
    }
}