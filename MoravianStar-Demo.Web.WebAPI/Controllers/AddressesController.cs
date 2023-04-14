using Microsoft.AspNetCore.Mvc;
using MoravianStar.Dao;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.Core.Filters.Test;
using MoravianStar_Demo.Web.Core.Models.Test;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Web.WebAPI.Controllers
{
    public class AddressesController : SystemEntityRestController<AddressEntity, Guid, AddressModel, AddressFilter>
    {
        public override Task<ActionResult<PageResult<AddressModel>>> Read([FromQuery] AddressFilter filter, [FromQuery] List<Sort> sorts, [FromQuery] Page page)
        {
            return base.Read(filter, sorts, page);
        }
    }
}