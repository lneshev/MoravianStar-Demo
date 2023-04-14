using DataLayer.Common.Core.Entities.Test;
using DataLayer.Common.Core.Interfaces;
using DataLayer.Common.Core.Projections;
using DataLayer.Common.Services;
using DataLayer.Web.Core.Models.Test;
using DataLayer.Web.Core.Projections.Test;
using DataLayer.Persistence.DbContexts;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Web.Services.Test
{
    public class ClientModel2Service : SystemModelService<ClientEntity, int, ClientModel2>
    {
        public ClientModel2Service(IRepository<DataLayer_SystemContext> repository) : base(repository)
        {
        }

        public override Expression<Func<ClientEntity, IProjectionBase>> Project()
        {
            return entity => new ClientProjection2() // ClientEntity
            {
                Id = entity.Id,
                Name = entity.Name,
                Status = entity.Status
            };
        }

        public override async Task<ClientModel2> MapAsync(IProjectionBase projection)
        {
            var proj = (ClientProjection2)projection; // ClientEntity

            return await Task.FromResult(new ClientModel2()
            {
                Id = proj.Id,
                Name = proj.Name,
                Status = proj.Status,
                StatusText = proj.Status.ToString()
            });
        }
    }
}