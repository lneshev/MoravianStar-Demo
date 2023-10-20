using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Web.Core.Models.Test;
using System;
using System.Linq.Expressions;

namespace MoravianStar_Demo.Web.Services.Test
{
    public class BlockModelMappingService : ModelsMappingService<BlockModel, BlockEntity>
    {
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