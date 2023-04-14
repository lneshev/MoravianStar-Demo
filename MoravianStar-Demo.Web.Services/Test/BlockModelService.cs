using DataLayer.Common.Core.Entities.Test;
using DataLayer.Common.Core.Interfaces;
using DataLayer.Common.Core.Projections;
using DataLayer.Common.Services;
using DataLayer.Web.Core.Models.Test;
using DataLayer.Persistence.DbContexts;
using System;
using System.Linq.Expressions;

namespace MoravianStar_Demo.Web.Services.Test
{
    public class BlockModelService : ClientModelService<BlockEntity, int, BlockModel>
    {
        public BlockModelService(IRepository<DataLayer_ClientDMLContext> repository) : base(repository)
        {
        }

        public override Expression<Func<BlockEntity, IProjectionBase>> Project()
        {
            return entity => new BlockModel()
            {
                Id = entity.Id,
                ClientName = entity.Client.Name,
                Boundaries = entity.Boundaries,
                BoundariesArea = entity.Boundaries.Area
            };
        }
    }
}