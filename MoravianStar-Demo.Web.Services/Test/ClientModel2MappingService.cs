using MoravianStar.Dao;
using MoravianStar.Extensions;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.Core.Resources;
using MoravianStar_Demo.Web.Core.Models.Test;
using MoravianStar_Demo.Web.Core.Projections.Test;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Web.Services.Test
{
    public class ClientModel2MappingService : ModelsMappingService<ClientModel2, ClientEntity>
    {
        public override Expression<Func<ClientEntity, IProjectionBase>> Project()
        {
            return entity => new ClientProjection2()
            {
                Id = entity.Id,
                Name = entity.Name,
                Status = entity.Status
            };
        }

        public override async Task<ClientModel2> MapAsync(IProjectionBase projection)
        {
            var proj = (ClientProjection2)projection;

            return await Task.FromResult(new ClientModel2()
            {
                Id = proj.Id,
                Name = proj.Name,
                Status = proj.Status,
                StatusText = proj.Status.Translate(typeof(Strings))
            });
        }
    }
}