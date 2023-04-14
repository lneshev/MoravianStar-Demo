using DataLayer.Common.Core.Entities.Test;
using HotChocolate.Data.Sorting;
using System;

namespace MoravianStar_Demo.Mobile.Services.GraphQL.Queries.Test
{
    [Obsolete("We are not going to use HotChocolate's sorting, so we won't need to create a sort input type.")]
    public class VehicleSortType : SortInputType<VehicleEntity>
    {
        protected override void Configure(ISortInputTypeDescriptor<VehicleEntity> descriptor)
        {
            descriptor.Ignore(x => x.CurrentLocation);
        }
    }
}