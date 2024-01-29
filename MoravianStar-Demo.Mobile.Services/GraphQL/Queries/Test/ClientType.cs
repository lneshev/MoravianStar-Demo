using HotChocolate.Types;
using MoravianStar.Extensions;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.Core.Resources;

namespace MoravianStar_Demo.Mobile.Services.GraphQL.Queries.Test
{
    public class ClientType : ObjectType<ClientEntity>
    {
        protected override void Configure(IObjectTypeDescriptor<ClientEntity> descriptor)
        {
            // Checked: no base logic
            base.Configure(descriptor);

            // Override entity's name
            descriptor.Name("Client");

            // Description for the entity
            descriptor.Description("Description of ClientEntity (Client) goes here.");

            // This is necessary to have caching on client. So we have to write it always?!
            descriptor.Field(x => x.Id).Type<IdType>(); // .ID(); -> any difference?!

            // Add custom property in the schema, that doesn't exist in the entity
            descriptor
                .Field("statusText")
                .Resolve((context, ct) => context.Parent<ClientEntity>().Status.Translate(typeof(Strings)))
                .Description("Description of StatusText goes here");

            // I tried these resolvers before using [UseProjection] attribute in Query.cs

            //descriptor
            //    .Field(x => x.MainAddress)
            //    .ResolveWith<Resolvers>(x => x.GetMainAddress(default!, default!))
            //    .UseDbContext<SystemContext>();

            //descriptor
            //    .Field(x => x.Addresses)
            //    .ResolveWith<Resolvers>(x => x.GetAddresses(default!, default!))
            //    .UseDbContext<SystemContext>();

            //descriptor
            //    .Field(x => x.Vehicles)
            //    .ResolveWith<Resolvers>(x => x.GetVehicles(default!, default!))
            //    .UseDbContext<SystemContext>();
        }

        private class Resolvers
        {
            //public AddressEntity GetMainAddress([Parent] ClientEntity client, [ScopedService] SystemContext dbContext)
            //{
            //    return dbContext.Set<AddressEntity>().SingleOrDefault(x => x.Id == client.MainAddressId);
            //}

            //public IQueryable<AddressEntity> GetAddresses([Parent] ClientEntity client, [ScopedService] SystemContext dbContext)
            //{
            //    return dbContext.Set<AddressEntity>().Where(x => client.Addresses.Contains(x));
            //}

            //public IQueryable<VehicleEntity> GetVehicles([Parent] ClientEntity client, [ScopedService] SystemContext dbContext)
            //{
            //    return dbContext.Set<VehicleEntity>().Where(x => x.Clients.Contains(client));
            //}
        }
    }
}