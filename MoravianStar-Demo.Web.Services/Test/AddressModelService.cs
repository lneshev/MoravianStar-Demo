using DataLayer.Common.Core.Entities.Test;
using DataLayer.Common.Core.Filters.Test;
using DataLayer.Common.Core.Interfaces;
using DataLayer.Common.Core.Projections;
using DataLayer.Common.Services;
using DataLayer.Web.Core.Models.Test;
using DataLayer.Persistence.DbContexts;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Web.Services.Test
{
    public class AddressModelService : SystemModelService<AddressEntity, Guid, AddressModel>
    {
        private readonly IRepository<DataLayer_SystemContext> repository;

        public AddressModelService(IRepository<DataLayer_SystemContext> repository) : base(repository)
        {
            this.repository = repository;
        }

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
            var client = (await repository.ReadAsync<ClientEntity, ClientFilter>(new ClientFilter()
            {
                MainAddressId = model.Id
            })).SingleOrDefault();

            model.ClientId = client?.Id;
            model.ClientName = client?.Name;

            return model;
        }
    }
}