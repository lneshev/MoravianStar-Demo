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
    public class AddressModelMappingService : ModelsMappingService<AddressModel, AddressEntity>
    {
        public override Expression<Func<AddressEntity, IProjectionBase>> Project()
        {
            // Projecting each entity to AddressModel
            return entity => new AddressModel()
            {
                Id = entity.Id,
                Address = entity.Address
            };
        }

        public override async Task<List<ProjectionModelPair<IProjectionBase, AddressModel>>> ToModels(List<ProjectionModelPair<IProjectionBase, AddressModel>> pairs)
        {
            // The base logic will fill partially the models from the projections
            pairs = await base.ToModels(pairs);

            // Read all clients that have a main address
            var clientsWithMainAddress = await Persistence.ForEntity<ClientEntity>().ReadAsync<ClientFilter, ClientWithAddressesProjection>(
                filter: new ClientFilter() { HasMainAddress = true },
                projection: x => new ClientWithAddressesProjection()
                {
                    ClientId = x.Id,
                    ClientName = x.Name,
                    AddressIds = new List<Guid>() { x.MainAddressId.Value }
                },
                trackable: false);

            // Read all clients that have additional addresses
            var clientsWithAdditionalAddresses = await Persistence.ForEntity<ClientEntity>().ReadAsync<ClientFilter, ClientWithAddressesProjection>(
                filter: new ClientFilter() { HasAdditionalAddress = true },
                projection: x => new ClientWithAddressesProjection
                {
                    ClientId = x.Id,
                    ClientName = x.Name,
                    AddressIds = x.Addresses.Select(x => x.Id).ToList()
                },
                trackable: false);

            // Add all clients that have a main address and all clients that have additional addresses
            // in dictionary where the key is the address and the value is the client
            var dict = new Dictionary<Guid, ClientWithAddressesProjection>();

            foreach (var client in clientsWithMainAddress.Items)
            {
                dict.TryAdd(client.AddressIds[0], client);
            }

            foreach (var client in clientsWithAdditionalAddresses.Items)
            {
                foreach (var addressId in client.AddressIds)
                {
                    dict.Add(addressId, new ClientWithAddressesProjection()
                    {
                        ClientId = client.ClientId,
                        ClientName = client.ClientName
                    });
                }
            }

            // Fill the address models with the missing client data
            foreach (var pair in pairs)
            {
                var proj = (AddressModel)pair.Projection; // See Project() method
                pair.Model.ClientId = dict.ContainsKey(proj.Id) ? dict[proj.Id].ClientId : null;
                pair.Model.ClientName = dict.ContainsKey(proj.Id) ? dict[proj.Id].ClientName : null;
            }

            return pairs;
        }

        private class ClientWithAddressesProjection : IProjectionBase
        {
            public ClientWithAddressesProjection()
            {
                AddressIds = new List<Guid>();
            }

            public int ClientId { get; set; }
            public string ClientName { get; set; }
            public List<Guid> AddressIds { get; set; }
        }
    }
}