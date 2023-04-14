using DataLayer.Common.Core.Entities.Test;
using DataLayer.Common.Core.Interfaces;
using DataLayer.Common.Core.Projections;
using DataLayer.Common.Services;
using DataLayer.Web.Core.Models.Test;
using DataLayer.Persistence.DbContexts;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MoravianStar_Demo.Web.Services.Test
{
    public class ClientModel3Service : SystemModelService<ClientEntity, int, ClientModel3>
    {
        public ClientModel3Service(IRepository<DataLayer_SystemContext> repository) : base(repository)
        {
        }

        public override Expression<Func<ClientEntity, IProjectionBase>> Project()
        {
            return entity => new ClientModel3() // ClientEntity
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