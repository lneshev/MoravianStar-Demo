using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.Entities.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Common.Services.Test
{
    public class ClientDeleting : IEntityDeleting<ClientEntity>
    {
        public async Task DeletingAsync(ClientEntity entity, IDictionary<string, object> additionalParameters = null)
        {
            if (entity.MainAddressId.HasValue)
            {
                await Persistence.ForEntity<AddressEntity, Guid>().DeleteAsync(entity.MainAddress);
            }
            foreach (var addressEntity in entity.Addresses.ToList())
            {
                await Persistence.ForEntity<AddressEntity, Guid>().DeleteAsync(addressEntity);
            }
        }
    }
}