using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.Core.Filters.Test;
using MoravianStar_Demo.Web.Core.Models.Test;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Web.Services.Test
{
    public class AddressModelMappingService : ModelsMappingService<AddressModel, AddressEntity>
    {
        public override Expression<Func<AddressEntity, IProjectionBase>> Project()
        {
            return entity => new AddressModel()
            {
                Id = entity.Id,
                Address = entity.Address
            };
        }

        public override async Task<AddressModel> MapAsync(IProjectionBase projection)
        {
            var model = await base.MapAsync(projection);

            // This creates many DB requests!
            var client = (await Persistence.ForEntity<ClientEntity>().ReadAsync(new ClientFilter()
            {
                MainAddressId = model.Id
            })).Items.SingleOrDefault();

            model.ClientId = client?.Id;
            model.ClientName = client?.Name;

            return model;
        }
    }
}