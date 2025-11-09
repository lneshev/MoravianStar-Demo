using Microsoft.EntityFrameworkCore;
using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.Core.Filters.Test;
using MoravianStar_Demo.Web.Core.Models.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Web.Services.Test
{
    public class ClientModel3MappingService : ModelsMappingService<ClientModel3, ClientEntity>
    {
        public override Expression<Func<ClientEntity, IProjectionBase>> Project()
        {
            return entity => new ClientModel3()
            {
                Id = entity.Id,
                Name = entity.Name,
                MainAddressId = entity.MainAddressId,
                MainAddressAddress = entity.MainAddress != null ? entity.MainAddress.Address : null,
                AddressesCount = entity.Addresses.Count,
                VehiclesLicensePlates = entity.Vehicles.Select(x => x.LicensePlate).OrderBy(x => x).ToList()
            };
        }

        public override IQueryable<ClientEntity> GetIncludes(IQueryable<ClientEntity> query)
        {
            return base.GetIncludes(query)
                .Include(x => x.MainAddress)
                .Include(x => x.Addresses)
                .Include(x => x.Vehicles);
        }

        public override async Task<List<EntityModelPair<ClientEntity, ClientModel3>>> ToEntities(List<EntityModelPair<ClientEntity, ClientModel3>> pairs)
        {
            pairs = await base.ToEntities(pairs);

            foreach (var pair in pairs)
            {
                pair.Entity.Id = pair.Model.Id;
                pair.Entity.Name = pair.Model.Name;

                if (pair.Entity.IsNew())
                {
                    pair.Entity.MainAddress = new AddressEntity() { Address = $"Main address of {pair.Entity.Name}" };
                    pair.Entity.Addresses.Add(new AddressEntity() { Address = $"Additional address of {pair.Entity.Name}" });

                    var vehicleB1111AA = await Persistence.ForEntity<VehicleEntity, int>().ReadQuery(new VehicleFilter() { LicensePlateEqualsInsensitive = "B1111AA" }).SingleOrDefaultAsync();
                    if (vehicleB1111AA != null)
                    {
                        pair.Entity.Vehicles.Add(vehicleB1111AA);
                    }
                }
            }

            return pairs;
        }
    }
}