using Microsoft.AspNetCore.Mvc;
using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.Core.Filters.Test;
using MoravianStar_Demo.Web.Core.Models.Test;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Web.WebAPI.Controllers
{
    public class BlocksController : ClientEntityRestController<BlockEntity, int, BlockModel, BlockFilter>
    {
        public override async Task<ActionResult<PageResult<BlockModel>>> Read([FromQuery] BlockFilter filter, [FromQuery] List<Sort> sorts, [FromQuery] Page page)
        {
            return await base.Read(filter, sorts, page);
        }
    }
}