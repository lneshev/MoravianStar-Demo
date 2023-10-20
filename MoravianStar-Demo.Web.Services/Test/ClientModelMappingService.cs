using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Web.Core.Models.Test;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Web.Services.Test
{
    public class ClientModelMappingService : ModelsMappingService<ClientModel, ClientEntity>
    {
        public override Expression<Func<ClientEntity, IProjectionBase>> Project()
        {
            return entity => new ClientModel()
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public override async Task<List<EntityModelPair<ClientEntity, ClientModel>>> ToEntities(List<EntityModelPair<ClientEntity, ClientModel>> pairs)
        {
            pairs = await base.ToEntities(pairs);

            foreach (var pair in pairs)
            {
                pair.Entity.Name = pair.Model.Name;
            }

            return pairs;
        }
    }
}