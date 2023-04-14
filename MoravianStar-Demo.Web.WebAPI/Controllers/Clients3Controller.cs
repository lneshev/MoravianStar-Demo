using Microsoft.AspNetCore.Mvc;
using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.Core.Filters.Test;
using MoravianStar_Demo.Web.Core.Models.Test;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Web.WebAPI.Controllers
{
    public class Clients3Controller : SystemEntityRestController<ClientEntity, int, ClientModel3, ClientFilter>
    {
        public override async Task<ActionResult<ClientModel3>> Get([FromRoute] int id)
        {
            return await base.Get(id);
        }

        public override async Task<ActionResult<PageResult<ClientModel3>>> Read([FromQuery] ClientFilter filter, [FromQuery] List<Sort> sorts, [FromQuery] Page page)
        {
            return await base.Read(filter, sorts, page);
        }
    }
}