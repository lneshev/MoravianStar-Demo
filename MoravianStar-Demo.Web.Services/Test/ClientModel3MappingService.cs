using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Web.Core.Models.Test;
using System;
using System.Linq;
using System.Linq.Expressions;

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
                MainAddressAddress = entity.MainAddress.Address,
                AddressesCount = entity.Addresses.Count,
                VehiclesLicensePlates = entity.Vehicles.Select(x => x.LicensePlate).OrderBy(x => x).ToList()
            };
        }
    }
}