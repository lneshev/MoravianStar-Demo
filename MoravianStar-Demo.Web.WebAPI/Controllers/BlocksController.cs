using Microsoft.AspNetCore.Mvc;
using MoravianStar.Dao;
using MoravianStar.WebAPI.Attributes;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.Core.Filters.Test;
using MoravianStar_Demo.Persistence.DbContexts;
using MoravianStar_Demo.Web.Core.Models.Test;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Web.WebAPI.Controllers
{
    public class BlocksController : ClientEntityRestController<BlockEntity, int, BlockModel, BlockFilter>
    {
        // By not executing in a transaction the perfomance might be increased, but there might be differences
        // in the number of returned results (when paging is not applied) and the TotalCount property if somebody
        // inserts or deletes records in this small time window.
        [ExecuteInTransactionAsync(false)]
        public override async Task<ActionResult<PageResult<BlockModel>>> Read([FromQuery] BlockFilter filter, [FromQuery] List<Sort> sorts, [FromQuery] Page page)
        {
            return await base.Read(filter, sorts, page);
        }

        // When you need to work with a different DbContext compared to the default one, you should specify it explicitly.
        [ExecuteInTransactionAsync(typeof(ClientDMLContext))]
        public override async Task<ActionResult<BlockModel>> Post([FromBody] BlockModel model)
        {
            return await base.Post(model);
        }
    }
}