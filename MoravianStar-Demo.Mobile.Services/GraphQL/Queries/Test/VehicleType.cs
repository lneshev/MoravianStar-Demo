using HotChocolate;
using HotChocolate.Types;
using MoravianStar.Dao;
using MoravianStar.GraphQL.Extensions;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.DataAccess.DbContexts;
using System.Linq;

namespace MoravianStar_Demo.Mobile.Services.GraphQL.Queries.Test
{
    public class VehicleType : ObjectType<VehicleEntity>
    {
        protected override void Configure(IObjectTypeDescriptor<VehicleEntity> descriptor)
        {
            base.Configure(descriptor);

            // The sorting for geometry types is not yet supported and throws error. I tried disabling it like this, but it didn't worked. It worked by adding "VehicleSortType".
            //descriptor.Ignore(x => x.CurrentLocation);
            //descriptor.Field(x => x.CurrentLocation).Ignore();

            //descriptor
            //    .Field("clientsCount")
            //    .Resolve((context, ct) => context.Parent<VehicleEntity>().Clients.Count) // Throws NullReference error if "Clients" property is not get by the client
            //    .Description("Description of clientsCount goes here");

            //descriptor
            //    .Field("clientsCount")
            //    .ResolveWith<Resolvers>(x => x.GetClientsCount(default!, default!))
            //    .UseDbContext<SystemContext>(); // Requires IDbContextFactory

            descriptor
                .Field("clientsCount")
                .ResolveWith<Resolvers>(x => x.GetClientsCount2(default!)) // N+1 problem
                .UseServiceLocator();
        }

        private class Resolvers
        {
            public int GetClientsCount([Parent] VehicleEntity vehicle, SystemContext dbContext)
            {
                // All leads to the N + 1 problem:
                return dbContext.Set<VehicleEntity>().Where(x => x.Id == vehicle.Id).Select(x => x.Clients.Count).SingleOrDefault();
                //return dbContext.Set<VehicleEntity>().Where(x => x.Id == vehicle.Id).Select(x => x.Clients.Count); // returns IQueryable<int>  which is interpreted as array by GraphQL
                //return vehicle.Clients.Count; // Throws NullReference error if "Clients" property is not get by the client
            }

            public int GetClientsCount2([Parent] VehicleEntity vehicle)
            {
                return Persistence.ForDbContext<SystemContext>().DbContext.Set<VehicleEntity>().Where(x => x.Id == vehicle.Id).Select(x => x.Clients.Count).SingleOrDefault();
            }
        }
    }
}