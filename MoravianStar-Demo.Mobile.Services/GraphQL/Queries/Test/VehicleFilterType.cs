using DataLayer.Common.Core.Entities.Test;
using HotChocolate.Data.Filters;
using System;

namespace MoravianStar_Demo.Mobile.Services.GraphQL.Queries.Test
{
    [Obsolete("We are not going to use HotChocolate's filtering, so we won't need to create a filter input type.")]
    public class VehicleFilterType : FilterInputType<VehicleEntity>
    {
        protected override void Configure(IFilterInputTypeDescriptor<VehicleEntity> descriptor)
        {
            //descriptor.Field("ClientName").Type<StringType>();
        }
    }
}